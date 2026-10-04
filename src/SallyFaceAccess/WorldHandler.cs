using Il2CppAC;
using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// The room: announces the area on entry, the room within it as the gameplay camera
/// changes, the object the player is in range of (AC's active hotspot - what puts the prompt
/// on screen), and a room scan limited to the player's level and the camera's room.
///   Page Up / Page Down  previous / next object, left to right, with direction and distance
///   Home                 the current scan object again, with its updated distance
///   F4                   where am I: room and how many objects it has
///   F6                   describe the room
/// </summary>
internal static class WorldHandler
{
    // Scenes that are not places.
    private static readonly HashSet<string> NonPlaces = new() { "portableMooseSplash", "loadingScreen", "mainMenu" };

    // Rooms of one scene are stacked in levels about 11 units apart; things within half of
    // that of the player's height are on the player's level.
    private const float LevelHeight = 5.5f;
    // Half a screen's width, for cameras whose projection cannot be read.
    private const float FallbackHalfWidth = 9.6f;

    private static readonly List<Hotspot> _hotspots = new();
    private static readonly Dictionary<IntPtr, string> _names = new();
    private static readonly HashSet<string> _described = new();
    private static float _nextRefresh;
    private static IntPtr _active;
    private static int _scanIndex = -1;
    private static string _scene = "";
    private static string _camera;
    private static float _autoDumpAt;
    private static string _lastView = "";

    public static void OnSceneLoaded(string scene)
    {
        _scene = scene;
        _hotspots.Clear();
        _names.Clear();
        _active = IntPtr.Zero;
        _scanIndex = -1;
        _nextRefresh = 0;
        _camera = null;
        _lastView = ""; // forces a fresh debug snapshot once the scene's camera is set
        if (!NonPlaces.Contains(scene)) Speech.Say(WorldTables.Area(scene), interrupt: false);
    }

    public static void Tick()
    {
        if (KickStarter.player == null || NonPlaces.Contains(_scene)) return;

        // Rooms are told apart by gameplay camera; cutscene cameras are not rooms.
        if (KickStarter.stateHandler?.gameState == GameState.Normal) CheckRoom();

        // Objects appear and disappear as the story moves; re-collect about once a second.
        if (Time.unscaledTime >= _nextRefresh)
        {
            _nextRefresh = Time.unscaledTime + 1f;
            Refresh();
        }

        CheckActive();
        DebugSnapshots();

        if (Input.GetKeyDown(KeyCode.PageDown)) Scan(+1);
        if (Input.GetKeyDown(KeyCode.PageUp)) Scan(-1);
        if (Input.GetKeyDown(KeyCode.Home)) Scan(0);
        if (Input.GetKeyDown(KeyCode.F4)) Speech.Say(string.Format(Strings.RoomSummary, RoomName(), _hotspots.Count));
        if (Input.GetKeyDown(KeyCode.F6))
            Speech.Say(Descriptions.Area(_scene, CameraName()) ?? Strings.NoDescription);
    }

    private static void CheckRoom()
    {
        var cam = CameraName();
        if (cam == _camera) return;
        var first = _camera == null;
        _camera = cam;
        _scanIndex = -1;
        _nextRefresh = 0;
        // The area itself was announced on scene load; later camera changes are rooms.
        if (!first) Speech.Say(RoomName(), interrupt: false);

        // Each description is spoken once per session on arrival; F6 repeats it.
        var description = Descriptions.Area(_scene, cam);
        if (description != null && _described.Add(description)) Speech.Say(description, interrupt: false);
    }

    private static string RoomName() =>
        WorldTables.Room(_scene, CameraName()) ?? WorldText.RoomFromCamera(CameraName()) ?? WorldTables.Area(_scene);

    private static void Refresh()
    {
        var current = _scanIndex >= 0 && _scanIndex < _hotspots.Count ? _hotspots[_scanIndex]?.Pointer : null;
        _hotspots.Clear();
        _names.Clear();
        var (minX, maxX) = RoomRange();
        var py = KickStarter.player.transform.position.y;
        foreach (var h in UnityEngine.Object.FindObjectsOfType<Hotspot>())
        {
            if (h == null || !h.gameObject.activeInHierarchy || !h.IsOn()) continue;
            if (Math.Abs(Y(h) - py) > LevelHeight) continue;
            var x = X(h);
            if (x < minX || x > maxX) continue;
            if (Name(h).Length > 0) _hotspots.Add(h);
        }
        _hotspots.Sort((a, b) => X(a).CompareTo(X(b)));

        // Several things with the same name in one room are numbered left to right.
        foreach (var group in _hotspots.GroupBy(Name))
        {
            var n = 0;
            var many = group.Count() > 1;
            foreach (var h in group) _names[h.Pointer] = many ? WorldText.Numbered(group.Key, ++n) : group.Key;
        }

        // Keep the scan position on the same object when the list changes around it.
        _scanIndex = current == null ? -1 : _hotspots.FindIndex(h => h.Pointer == current);
    }

    /// <summary>
    /// The x range the current camera can show: a locked camera shows one screen, a limited
    /// one its limits plus half a screen either side. Anything else is unbounded.
    /// </summary>
    private static (float, float) RoomRange()
    {
        var cam2d = KickStarter.mainCamera?.attachedCamera?.TryCast<GameCamera2D>();
        if (cam2d == null) return (float.MinValue, float.MaxValue);
        var unityCam = KickStarter.CameraMain;
        var half = unityCam != null && unityCam.orthographic ? unityCam.orthographicSize * unityCam.aspect : FallbackHalfWidth;
        var cx = cam2d.transform.position.x;
        if (cam2d.lockHorizontal) return (cx - half, cx + half);
        if (cam2d.limitHorizontal)
        {
            var lo = cam2d.constrainHorizontal.x - half;
            var hi = cam2d.constrainHorizontal.y + half;
            // Only trust the limits if they contain where the camera actually is.
            if (cx >= lo && cx <= hi) return (lo, hi);
        }
        return (float.MinValue, float.MaxValue);
    }

    private static void CheckActive()
    {
        var h = KickStarter.playerInteraction?.GetActiveHotspot();
        var ptr = h?.Pointer ?? IntPtr.Zero;
        if (ptr == _active) return;
        _active = ptr;
        if (h == null) return;

        var name = DisplayName(h);
        Log.Debug("World", $"active {h.gameObject.name} -> '{name}'");
        if (name.Length > 0) Speech.Say(name);
    }

    private static void Scan(int delta)
    {
        if (_hotspots.Count == 0) { Speech.Say(Strings.NothingHere); return; }
        if (_scanIndex < 0) _scanIndex = NearestIndex(); // first press starts at the nearest object
        else _scanIndex = Math.Clamp(_scanIndex + delta, 0, _hotspots.Count - 1);

        var h = _hotspots[_scanIndex];
        if (h == null) return;
        var dx = X(h) - KickStarter.player.transform.position.x;
        Speech.Say($"{DisplayName(h)}, {WorldText.Direction(dx, Settings.StepSize)}");
    }

    private static int NearestIndex()
    {
        var px = KickStarter.player.transform.position.x;
        var best = 0;
        for (var i = 1; i < _hotspots.Count; i++)
            if (Math.Abs(X(_hotspots[i]) - px) < Math.Abs(X(_hotspots[best]) - px)) best = i;
        return best;
    }

    private static void DebugSnapshots()
    {
        // Development aid: a screen dump per area, and per camera/walkable area within it.
        if (!Settings.DebugLog) return;
        var view = $"{CameraName()}|{NavMeshName()}";
        if (view != _lastView)
        {
            Log.Debug("World", $"view {view}");
            _lastView = view;
            _autoDumpAt = Time.unscaledTime + 1.5f;
        }
        if (_autoDumpAt > 0 && Time.unscaledTime >= _autoDumpAt)
        {
            _autoDumpAt = 0;
            var file = $"{_scene}__{CameraName()}__{NavMeshName()}.txt";
            UiDump.Write(Path.Combine(Log.Dir, "scenes", string.Join("_", file.Split(Path.GetInvalidFileNameChars()))));
        }
    }

    internal static string CameraName() => KickStarter.mainCamera?.attachedCamera?.gameObject.name ?? "<none>";

    internal static string NavMeshName() => KickStarter.sceneSettings?.navMesh?.gameObject.name ?? "<none>";

    /// <summary>The name as spoken in this room - numbered if it shares its name with another.</summary>
    private static string DisplayName(Hotspot h) => _names.TryGetValue(h.Pointer, out var n) ? n : Name(h);

    internal static string Name(Hotspot h) =>
        WorldText.HotspotName(WorldTables.Hotspot(_scene, h.gameObject.name), h.gameObject.name, h.GetName(Options.GetLanguage()));

    /// <summary>The object's x: its collider's centre where it has one, else its transform.</summary>
    internal static float X(Hotspot h) => Centre(h).x;

    internal static float Y(Hotspot h) => Centre(h).y;

    private static Vector3 Centre(Hotspot h)
    {
        var c2 = h.GetComponent<Collider2D>();
        if (c2 != null) return c2.bounds.center;
        var c3 = h.GetComponent<Collider>();
        if (c3 != null) return c3.bounds.center;
        return h.transform.position;
    }
}
