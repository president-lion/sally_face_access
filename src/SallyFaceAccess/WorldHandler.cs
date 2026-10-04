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

    // The room scan: objects and meaningful triggers, left to right.
    private sealed class ScanItem
    {
        public IntPtr Ptr;
        public float X, Y;
        public Hotspot Hotspot;   // null for a trigger
        public string Label;      // the trigger's name
    }
    private static readonly List<ScanItem> _scan = new();
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
        _scan.Clear();
        _names.Clear();
        Triggers.OnSceneLoaded();
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
        if (!TopDownAudio.IsScene(_scene)) CheckEdge(); // top-down games thud instead
        DebugSnapshots();

        if (Input.GetKeyDown(KeyCode.PageDown)) Scan(+1);
        if (Input.GetKeyDown(KeyCode.PageUp)) Scan(-1);
        if (Input.GetKeyDown(KeyCode.Home)) Scan(0);
        if (Input.GetKeyDown(KeyCode.F4)) Speech.Say(string.Format(Strings.RoomSummary, RoomName(), _scan.Count));
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
        IntPtr? current = _scanIndex >= 0 && _scanIndex < _scan.Count ? _scan[_scanIndex].Ptr : null;
        _hotspots.Clear();
        _names.Clear();
        var (minX, maxX) = RoomRange();
        var py = KickStarter.player.transform.position.y;
        var (minY, maxY) = TopDownAudio.IsScene(_scene) ? RoomRangeY() : (py - LevelHeight, py + LevelHeight);
        foreach (var h in UnityEngine.Object.FindObjectsOfType<Hotspot>())
        {
            if (h == null || !h.gameObject.activeInHierarchy || !h.IsOn()) continue;
            if (Y(h) < minY || Y(h) > maxY) continue;
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

        _scan.Clear();
        foreach (var h in _hotspots) _scan.Add(new ScanItem { Ptr = h.Pointer, X = X(h), Y = Y(h), Hotspot = h });
        foreach (var t in Triggers.Collect())
            if (t.Y >= minY && t.Y <= maxY && t.X >= minX && t.X <= maxX)
                _scan.Add(new ScanItem { Ptr = t.Trigger.Pointer, X = t.X, Y = t.Y, Label = t.Name });
        _scan.Sort((a, b) => a.X.CompareTo(b.X));

        // Keep the scan position on the same thing when the list changes around it.
        _scanIndex = current == null ? -1 : _scan.FindIndex(i => i.Ptr == current);
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

    /// <summary>The y range the current camera can show, for top-down games whose rooms are stacked closely.</summary>
    private static (float, float) RoomRangeY()
    {
        var cam2d = KickStarter.mainCamera?.attachedCamera?.TryCast<GameCamera2D>();
        var unityCam = KickStarter.CameraMain;
        var half = unityCam != null && unityCam.orthographic ? unityCam.orthographicSize : 1.8f;
        if (cam2d == null) return (float.MinValue, float.MaxValue);
        var cy = cam2d.transform.position.y;
        if (cam2d.lockVertical) return (cy - half, cy + half);
        if (cam2d.limitVertical)
        {
            var lo = cam2d.constrainVertical.x - half;
            var hi = cam2d.constrainVertical.y + half;
            if (cy >= lo && cy <= hi) return (lo, hi);
        }
        return (cy - half * 1.5f, cy + half * 1.5f);
    }

    /// <summary>Where the item last picked in the scan is, for the top-down beacon; null when none.</summary>
    internal static (float X, float Y)? ScanTarget =>
        _scanIndex >= 0 && _scanIndex < _scan.Count ? (_scan[_scanIndex].X, _scan[_scanIndex].Y) : null;

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
        if (_scan.Count == 0) { Speech.Say(Strings.NothingHere); return; }
        if (_scanIndex < 0) _scanIndex = NearestIndex(); // first press starts at the nearest thing
        else _scanIndex = Math.Clamp(_scanIndex + delta, 0, _scan.Count - 1);

        var item = _scan[_scanIndex];
        if (item.Label == null && item.Hotspot == null) return; // object destroyed since the last refresh
        var name = item.Hotspot != null ? DisplayName(item.Hotspot) : item.Label;
        var pos = KickStarter.player.transform.position;
        var dx = item.X - pos.x;
        var where = TopDownAudio.IsScene(_scene) ? TopDownLogic.Direction(dx, item.Y - pos.y) : WorldText.Direction(dx, Settings.StepSize);
        Speech.Say($"{name}, {where}");
    }

    private static int NearestIndex()
    {
        var px = KickStarter.player.transform.position.x;
        var best = 0;
        for (var i = 1; i < _scan.Count; i++)
            if (Math.Abs(_scan[i].X - px) < Math.Abs(_scan[best].X - px)) best = i;
        return best;
    }

    private static float _lastX, _stuckSince;
    private static bool _edgeSaid;

    /// <summary>Says "edge" once when the player keeps walking but no longer moves.</summary>
    private static void CheckEdge()
    {
        var x = KickStarter.player.transform.position.x;
        var pushing = Math.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f
                      && KickStarter.stateHandler?.gameState == GameState.Normal;
        if (!pushing || Math.Abs(x - _lastX) > 0.01f)
        {
            _stuckSince = Time.unscaledTime;
            if (Math.Abs(x - _lastX) > 0.01f) _edgeSaid = false;
        }
        else if (!_edgeSaid && Time.unscaledTime - _stuckSince > 0.4f)
        {
            _edgeSaid = true;
            Speech.Say(Strings.Edge);
        }
        _lastX = x;
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
    private static string DisplayName(Hotspot h)
    {
        var name = _names.TryGetValue(h.Pointer, out var n) ? n : Name(h);
        var hint = ItemHints.For(h);
        return hint == null ? name : $"{name}, {hint}";
    }

    internal static string Name(Hotspot h) =>
        WorldText.HotspotName(WorldTables.Hotspot(_scene, h.gameObject.name, X(h)), h.gameObject.name, h.GetName(Options.GetLanguage()));

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
