using Il2CppAC;
using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// The cult temple's stealth section (Episode 5). The rooms are two rows of wide halls; a
/// patrolling cultist shuts the doorways on his route (the game turns their hotspots off)
/// while he walks and looks around, standing guards catch anyone who touches them, and a
/// broken jar starts a 12-second count before the room is searched.
///   - Doorways: a shut one stays in the room scan marked "blocked", and the change either
///     way is spoken for the room the player is in.
///   - Cultists: touching one (his 5.5-wide capture box) is capture, whichever way he faces.
///     Each one in the player's row hums from where he stands, higher while he is
///     coming closer.
///   - The jar timer (the game's autoKillTimer list): spoken at the start, then counted down.
/// </summary>
internal static class TempleStealth
{
    private const float RowHeight = 10f;   // the two rows of halls are 50 units apart
    private const float HumRange = 45f;    // about one hall
    private const int Voices = 4;
    private const float TimerLength = 12f; // autoKillTimer's pause

    private static bool _active;
    private static readonly Dictionary<IntPtr, bool> _doorOn = new();
    private static List<Transform> _cultists = new();
    private static readonly Dictionary<int, float> _lastGap = new();
    private static readonly Dictionary<int, bool> _closing = new();
    private static float _nextGather, _nextDoors;
    private static float _timerStart = -1f;
    private static int _lastCount;

    public static bool Active => _active;

    // The C-4 jars: West 33, East 34, Inner West 35, Inner East 36 (the last, in Inner East).
    private const int FirstJarVar = 33, LastJarVar = 36, C4Item = 56;
    private static bool _offered;

    public static void OnSceneLoaded(string scene)
    {
        if (_active) ToneEngine.StopLoops();
        _active = scene.StartsWith("05_temple");
        _doorOn.Clear();
        _cultists.Clear();
        _lastGap.Clear();
        _closing.Clear();
        _nextGather = 0;
        _nextDoors = 0;
        _timerStart = -1f;
        _offered = false;
    }

    /// <summary>A doorway a patrol can shut: "hs_W-N", not the locked doors.</summary>
    public static bool IsGuardedDoor(Hotspot h) =>
        _active && h != null && h.gameObject.name.StartsWith("hs_") && !h.gameObject.name.StartsWith("hs_locked");

    /// <summary>Extra words for the room scan: "blocked" for a shut doorway.</summary>
    public static string For(Hotspot h) => IsGuardedDoor(h) && !h.IsOn() ? Strings.Blocked : null;

    public static void Tick()
    {
        if (!_active || KickStarter.player == null) return;
        var p = KickStarter.player.transform.position;
        if (Time.unscaledTime >= _nextDoors) { _nextDoors = Time.unscaledTime + 0.25f; Doors(p); }
        Hum(p);
        Timer();
    }

    private static bool IsTemple1 => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "05_temple1";

    private static bool AllPlaced => (GlobalVariables.GetVariable(LastJarVar)?.IntegerValue ?? 0) == 1;

    private static void OfferSkip()
    {
        if (_offered || !IsTemple1 || KickStarter.runtimeVariables == null) return;
        _offered = true;
        if (!AllPlaced) Speech.Say(Strings.SkipAvailable, interrupt: false);
    }

    /// <summary>
    /// Delete: every jar counts as planted, the C-4 leaves the inventory, and the game's own
    /// dungeon doorway script (IE-D) runs - it checks the last jar, then plays the ending.
    /// </summary>
    private static void Skip()
    {
        if (KickStarter.stateHandler?.gameState != GameState.Normal || AllPlaced) return;
        ActionList toDungeon = null;
        foreach (var l in UnityEngine.Object.FindObjectsOfType<ActionList>())
            if (l != null && l.gameObject.name == "IE-D") { toDungeon = l; break; }
        if (toDungeon == null) { Log.Warn("TempleStealth", "no IE-D list to finish the temple"); return; }
        foreach (var l in UnityEngine.Object.FindObjectsOfType<ActionList>())
            if (l != null && (l.gameObject.name == "autoKillTimer" || l.gameObject.name.StartsWith("cultistsAfterJar"))) l.Kill();
        for (var v = FirstJarVar; v <= LastJarVar; v++) GlobalVariables.GetVariable(v)?.SetValue(1);
        try { KickStarter.runtimeInventory.Remove(C4Item, 0, true); } catch (Exception ex) { Log.Once("TempleStealth.C4", ex); }
        Log.Info("TempleStealth", "skipped the C-4 stealth section");
        Speech.Say(Strings.PuzzleSkipped);
        toDungeon.Interact();
    }

    private static void Doors(Vector3 p)
    {
        foreach (var h in UnityEngine.Object.FindObjectsOfType<Hotspot>())
        {
            if (!IsGuardedDoor(h) || !h.gameObject.activeInHierarchy) continue;
            var on = h.IsOn();
            var known = _doorOn.TryGetValue(h.Pointer, out var was);
            _doorOn[h.Pointer] = on;
            if (!known || was == on) continue;
            // Only the room the player is in: same row, within a hall's width.
            var c = WorldHandler.Centre(h);
            if (Math.Abs(c.y - p.y) > RowHeight || Math.Abs(c.x - p.x) > HumRange * 1.5f) continue;
            var name = WorldHandler.Name(h);
            Speech.Say(string.Format(on ? Strings.WayClear : Strings.WayBlocked, name), interrupt: false);
        }
    }

    private static void Hum(Vector3 p)
    {
        if (Time.unscaledTime >= _nextGather)
        {
            _nextGather = Time.unscaledTime + 5f; // cultists are placed with the scene, never spawned
            _cultists = UnityEngine.Object.FindObjectsOfType<Transform>()
                .Where(t => t != null && IsCultist(t.gameObject.name))
                .ToList();
        }
        var near = _cultists
            .Where(t => t != null && t.gameObject.activeInHierarchy && Math.Abs(t.position.y - p.y) <= RowHeight)
            .OrderBy(t => Math.Abs(t.position.x - p.x))
            .Take(Voices).ToList();
        for (var i = 0; i < Voices; i++)
        {
            if (i >= near.Count) { ToneEngine.Loop(i, ToneEngine.Cue.Hum, default); continue; }
            var t = near[i];
            var dx = t.position.x - p.x;
            var place = TopDownLogic.Place(dx, 0f, HumRange);
            // Coming closer sounds higher: the one to back away from. Standing or leaving is low.
            var id = t.GetInstanceID();
            var was = _lastGap.TryGetValue(id, out var g) ? g : Math.Abs(dx);
            var gap = Math.Abs(dx);
            if (Math.Abs(gap - was) > 0.02f) _closing[id] = gap < was;
            _lastGap[id] = gap;
            var toward = _closing.TryGetValue(id, out var c) && c;
            ToneEngine.Loop(i, ToneEngine.Cue.Hum, place with { Pitch = toward ? 1.3f : 0.85f }, 0.8f);
        }
    }

    // Every cultist is an AC character "Cultist E", "Cultist N1"...; his sprite child is not counted again.
    private static bool IsCultist(string name) => name.StartsWith("Cultist ");

    private static void Timer()
    {
        var running = false;
        var lists = KickStarter.actionListManager?.activeLists;
        for (var i = 0; lists != null && i < lists.Count; i++)
        {
            var a = lists[i];
            if (a != null && a.IsRunning() && a.actionList != null && a.actionList.gameObject.name == "autoKillTimer") { running = true; break; }
        }
        if (running && _timerStart < 0)
        {
            _timerStart = Time.unscaledTime;
            _lastCount = (int)TimerLength;
            Speech.Say(string.Format(Strings.GetOut, (int)TimerLength), interrupt: true);
        }
        else if (!running && _timerStart >= 0)
        {
            _timerStart = -1f;
        }
        else if (running)
        {
            var left = (int)Math.Ceiling(TimerLength - (Time.unscaledTime - _timerStart));
            if (left < _lastCount && left > 0 && left % 3 == 0)
            {
                _lastCount = left;
                Speech.Say(left.ToString(), interrupt: true);
            }
        }
    }
}
