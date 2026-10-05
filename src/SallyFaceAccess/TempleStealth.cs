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
///     Each one in the player's row hums from where he stands, a little higher
///     when he faces the player.
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
    private static float _nextGather, _nextDoors;
    private static float _timerStart = -1f;
    private static int _lastCount;

    public static bool Active => _active;

    public static void OnSceneLoaded(string scene)
    {
        if (_active) ToneEngine.StopLoops();
        _active = scene.StartsWith("05_temple");
        _doorOn.Clear();
        _cultists.Clear();
        _nextGather = 0;
        _nextDoors = 0;
        _timerStart = -1f;
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
            // Facing the player sounds higher: the one to worry about.
            var facing = Facing(t);
            var toward = facing != 0 && Math.Sign(facing) == -Math.Sign(dx);
            ToneEngine.Loop(i, ToneEngine.Cue.Hum, place with { Pitch = toward ? 1.3f : 0.85f }, 0.8f);
        }
    }

    // Every cultist is an AC character "Cultist E", "Cultist N1"...; his sprite child is not counted again.
    private static bool IsCultist(string name) => name.StartsWith("Cultist ");

    /// <summary>+1 facing right, -1 left, 0 unknown.</summary>
    private static int Facing(Transform t)
    {
        try
        {
            var ch = t.GetComponent<Il2CppAC.Char>();
            if (ch != null)
            {
                var dir = ch.GetSpriteDirection() ?? "";
                if (dir.StartsWith("R")) return 1;
                if (dir.StartsWith("L")) return -1;
            }
            var sr = t.GetComponentInChildren<SpriteRenderer>();
            if (sr != null) return sr.flipX ? -1 : 1;
        }
        catch { }
        return 0;
    }

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
