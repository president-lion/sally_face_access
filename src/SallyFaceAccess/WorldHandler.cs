using Il2CppAC;
using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// The room: announces the area on entry, the object the player is in range of (AC's
/// active hotspot - what puts the prompt on screen), and a room scan.
///   Page Up / Page Down  previous / next object, left to right, with direction and distance
///   Home                 the current scan object again, with its updated distance
///   F4                   area name and how many objects it has
/// </summary>
internal static class WorldHandler
{
    // Scenes that are not places.
    private static readonly HashSet<string> NonPlaces = new() { "portableMooseSplash", "loadingScreen", "mainMenu" };

    private static readonly List<Hotspot> _hotspots = new();
    private static float _nextRefresh;
    private static IntPtr _active;
    private static int _scanIndex = -1;
    private static string _scene = "";
    private static float _autoDumpAt;
    private static string _lastView = "";

    public static void OnSceneLoaded(string scene)
    {
        _scene = scene;
        _hotspots.Clear();
        _active = IntPtr.Zero;
        _scanIndex = -1;
        _nextRefresh = 0;
        _lastView = ""; // forces a fresh debug snapshot once the scene's camera is set
        if (!NonPlaces.Contains(scene)) Speech.Say(WorldTables.Area(scene), interrupt: false);
    }

    public static void Tick()
    {
        if (KickStarter.player == null || NonPlaces.Contains(_scene)) return;

        // Objects appear and disappear as the story moves; re-collect about once a second.
        if (Time.unscaledTime >= _nextRefresh)
        {
            _nextRefresh = Time.unscaledTime + 1f;
            Refresh();
        }

        CheckActive();

        // Development aid: a screen dump per area, and per camera/walkable area within it,
        // for naming objects and working out how rooms inside a scene are told apart.
        if (Settings.DebugLog)
        {
            var view = $"{CameraName()}|{NavMeshName()}";
            if (view != _lastView)
            {
                Log.Debug("World", $"view {view}");
                _lastView = view;
                _autoDumpAt = Time.unscaledTime + 1.5f;
            }
        }
        if (_autoDumpAt > 0 && Time.unscaledTime >= _autoDumpAt)
        {
            _autoDumpAt = 0;
            var file = $"{_scene}__{CameraName()}__{NavMeshName()}.txt";
            UiDump.Write(Path.Combine(Log.Dir, "scenes", string.Join("_", file.Split(Path.GetInvalidFileNameChars()))));
        }

        if (Input.GetKeyDown(KeyCode.PageDown)) Scan(+1);
        if (Input.GetKeyDown(KeyCode.PageUp)) Scan(-1);
        if (Input.GetKeyDown(KeyCode.Home)) Scan(0);
        if (Input.GetKeyDown(KeyCode.F4)) Speech.Say(string.Format(Strings.RoomSummary, WorldTables.Area(_scene), _hotspots.Count));
    }

    private static void Refresh()
    {
        var current = _scanIndex >= 0 && _scanIndex < _hotspots.Count ? _hotspots[_scanIndex]?.Pointer : null;
        _hotspots.Clear();
        foreach (var h in UnityEngine.Object.FindObjectsOfType<Hotspot>())
            if (h != null && h.gameObject.activeInHierarchy && h.IsOn() && Name(h).Length > 0) _hotspots.Add(h);
        _hotspots.Sort((a, b) => X(a).CompareTo(X(b)));
        // Keep the scan position on the same object when the list changes around it.
        _scanIndex = current == null ? -1 : _hotspots.FindIndex(h => h.Pointer == current);
    }

    private static void CheckActive()
    {
        var h = KickStarter.playerInteraction?.GetActiveHotspot();
        var ptr = h?.Pointer ?? IntPtr.Zero;
        if (ptr == _active) return;
        _active = ptr;
        if (h == null) return;

        var name = Name(h);
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
        Speech.Say($"{Name(h)}, {WorldText.Direction(dx, Settings.StepSize)}");
    }

    private static int NearestIndex()
    {
        var px = KickStarter.player.transform.position.x;
        var best = 0;
        for (var i = 1; i < _hotspots.Count; i++)
            if (Math.Abs(X(_hotspots[i]) - px) < Math.Abs(X(_hotspots[best]) - px)) best = i;
        return best;
    }

    internal static string CameraName() => KickStarter.mainCamera?.attachedCamera?.gameObject.name ?? "<none>";

    internal static string NavMeshName() => KickStarter.sceneSettings?.navMesh?.gameObject.name ?? "<none>";

    internal static string Name(Hotspot h) =>
        WorldText.HotspotName(WorldTables.Hotspot(_scene, h.gameObject.name), h.gameObject.name, h.GetName(Options.GetLanguage()));

    /// <summary>The object's x: its collider's centre where it has one, else its transform.</summary>
    internal static float X(Hotspot h)
    {
        var c2 = h.GetComponent<Collider2D>();
        if (c2 != null) return c2.bounds.center.x;
        var c3 = h.GetComponent<Collider>();
        if (c3 != null) return c3.bounds.center.x;
        return h.transform.position.x;
    }
}
