using Il2CppAC;
using UnityEngine;
using Cue = SallyFaceAccess.ToneEngine.Cue;

namespace SallyFaceAccess;

/// <summary>
/// Positional audio for the top-down handheld game (Super Gear Boy, "In Seeking Providence"),
/// read live from the level's colliders, trap triggers and walking cultists:
///   walls      ticks speeding up toward the wall you walk at; a thud when you hit it;
///              a chirp from the side where a passage opens
///   traps      a buzz from the nearest spike or electric tile, faster as it gets closer
///   cultists   a hum per nearby cultist, following it
///   beacon     a bell from the thing last picked in the room scan (Page Up / Page Down)
/// Placement: pan for left/right, pitch for up/down (higher = above), volume for distance.
///   F7  wall distances in steps, all four ways
///   F8  positional audio on/off
/// </summary>
internal static class TopDownAudio
{
    private static readonly HashSet<string> Scenes = new() { "02_sgb" };

    private const float ProbeRange = 1.5f;     // how far wall probes look
    private const float TickRange = 1.0f;      // walls start ticking within this
    private const float TrapRange = 1.2f;
    private const float EnemyRange = 2.5f;
    private const float BeaconRange = 6f;
    private const float PlayerRadius = 0.1f;   // Greg's collision circle
    private const int MaxEnemies = 4;

    private static string _scene = "";
    private static bool _on = true;

    // Traps and cultists, gathered once a second.
    private readonly struct Rect { public readonly float X, Y, W, H; public Rect(Bounds b) { X = b.center.x; Y = b.center.y; W = b.size.x; H = b.size.y; } }
    private static readonly List<Rect> _traps = new();
    private static readonly List<Collider2D> _enemies = new();
    private static float _nextGather;

    private static readonly float[] _nextTick = new float[4];
    private static readonly OpeningDetector[] _openings = { new(), new(), new(), new() };
    private static float _nextTrap, _nextBeacon, _stuckSince;
    private static bool _thudded;
    private static Vector2 _lastPos;
    private static readonly HashSet<IntPtr> _running = new();
    private static readonly HashSet<IntPtr> _scratch = new();

    // Directions: 0 up, 1 down, 2 left, 3 right.
    private static readonly Vector2[] Dirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    public static bool IsScene(string scene) => Scenes.Contains(scene);

    public static void OnSceneLoaded(string scene)
    {
        _scene = scene;
        _traps.Clear();
        _enemies.Clear();
        _nextGather = 0;
        _running.Clear();
        foreach (var o in _openings) o.Reset();
        ToneEngine.StopAll();
    }

    public static void Tick()
    {
        if (!IsScene(_scene)) return;

        if (Input.GetKeyDown(KeyCode.F8))
        {
            _on = !_on;
            if (!_on) ToneEngine.StopAll();
            Speech.Say(_on ? Strings.AudioOn : Strings.AudioOff);
        }

        var player = KickStarter.player;
        if (player == null) return;
        Vector2 p = player.transform.position;

        if (Input.GetKeyDown(KeyCode.F7))
            Speech.Say(string.Format(Strings.Walls, TopDownLogic.WallSteps(Wall(p, 0)), TopDownLogic.WallSteps(Wall(p, 1)),
                TopDownLogic.WallSteps(Wall(p, 2)), TopDownLogic.WallSteps(Wall(p, 3))));

        Events();

        var playing = KickStarter.stateHandler?.gameState == GameState.Normal;
        if (!_on || !playing) { ToneEngine.StopLoops(); _lastPos = p; return; }

        if (Time.unscaledTime >= _nextGather) { _nextGather = Time.unscaledTime + 1f; Gather(); }

        Walls(p);
        Traps(p);
        Enemies(p);
        Beacon(p);
        _lastPos = p;
    }

    // ---- walls ------------------------------------------------------------------------------

    /// <summary>Free distance from the player's edge to the nearest wall that way; null if none within the probe.</summary>
    internal static float? Wall(Vector2 p, int dir)
    {
        var hits = Physics2D.RaycastAll(p, Dirs[dir], ProbeRange, 1 << LayerMask.NameToLayer("Default"));
        float? best = null;
        for (var i = 0; hits != null && i < hits.Length; i++)
        {
            var c = hits[i].collider;
            if (c == null || c.isTrigger) continue;
            if (c.GetComponentInParent<Il2CppAC.Char>() != null) continue; // people are not walls
            var d = Math.Max(0f, hits[i].distance - PlayerRadius);
            if (best == null || d < best) best = d;
        }
        return best;
    }

    private static void Walls(Vector2 p)
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var pushing = new[] { v > 0.5f, v < -0.5f, h < -0.5f, h > 0.5f };
        var moved = (p - _lastPos).magnitude > 0.002f;
        var now = Time.unscaledTime;

        // Ticks toward each wall being walked at.
        for (var d = 0; d < 4; d++)
        {
            if (!pushing[d]) continue;
            var dist = Wall(p, d);
            var interval = dist == null ? null : TopDownLogic.TickInterval(dist.Value, TickRange);
            if (interval != null && now >= _nextTick[d])
            {
                _nextTick[d] = now + interval.Value;
                ToneEngine.Play(Cue.Tick, Side(d), 0.6f);
            }
        }

        // Thud once when pushing but not moving.
        var anyPush = pushing[0] || pushing[1] || pushing[2] || pushing[3];
        if (!anyPush || moved) { _stuckSince = now; if (moved) _thudded = false; }
        else if (!_thudded && now - _stuckSince > 0.15f)
        {
            _thudded = true;
            var d = Array.IndexOf(pushing, true);
            ToneEngine.Play(Cue.Thud, Side(d));
        }

        // Openings at the sides of the way you're walking.
        var horizontal = pushing[2] || pushing[3];
        var vertical = pushing[0] || pushing[1];
        for (var d = 0; d < 4; d++)
        {
            var side = d < 2 ? horizontal && !vertical : vertical && !horizontal; // up/down sides when walking left/right
            if (!side) { _openings[d].Reset(); continue; }
            if (_openings[d].Update(Wall(p, d), moved)) ToneEngine.Play(Cue.Chirp, Side(d));
        }
    }

    /// <summary>A placement straight off to one side, for wall cues.</summary>
    private static TopDownLogic.Placement Side(int dir) => dir switch
    {
        0 => new TopDownLogic.Placement(0f, 1.4f, 1f),
        1 => new TopDownLogic.Placement(0f, 0.7f, 1f),
        2 => new TopDownLogic.Placement(-0.9f, 1f, 1f),
        _ => new TopDownLogic.Placement(0.9f, 1f, 1f),
    };

    // ---- traps and cultists -------------------------------------------------------------------

    internal static void Gather()
    {
        _traps.Clear();
        _enemies.Clear();
        foreach (var t in UnityEngine.Object.FindObjectsOfType<AC_Trigger>())
        {
            if (t == null || !t.gameObject.activeInHierarchy || !t.IsOn()) continue;
            var target = FirstRunTarget(t);
            var c = t.GetComponent<Collider2D>();
            if (c == null || !c.enabled) continue;
            if (TopDownLogic.IsTrapList(target)) _traps.Add(new Rect(c.bounds));
            else if (TopDownLogic.IsEnemyList(target)) _enemies.Add(c);
        }
    }

    private static string FirstRunTarget(AC_Trigger t)
    {
        var actions = t.source == ActionListSource.AssetFile && t.assetFile != null ? t.assetFile.actions : t.actions;
        for (var i = 0; actions != null && i < actions.Count; i++)
        {
            var run = actions[i]?.TryCast<ActionRunActionList>();
            if (run != null && run.isEnabled) return run.actionList?.gameObject.name;
        }
        return null;
    }

    /// <summary>For the self-test: distance to the nearest trap, and how many traps and cultists are known.</summary>
    internal static (float Nearest, int Traps, int Enemies) Survey(Vector2 p)
    {
        var best = float.MaxValue;
        foreach (var r in _traps) best = Math.Min(best, TopDownLogic.RectDistance(p.x, p.y, r.X, r.Y, r.W, r.H));
        return (best, _traps.Count, _enemies.Count);
    }

    private static void Traps(Vector2 p)
    {
        var best = float.MaxValue;
        (float X, float Y) at = default;
        foreach (var r in _traps)
        {
            var d = TopDownLogic.RectDistance(p.x, p.y, r.X, r.Y, r.W, r.H);
            if (d < best) { best = d; at = TopDownLogic.NearestPoint(p.x, p.y, r.X, r.Y, r.W, r.H); }
        }
        if (best > TrapRange || Time.unscaledTime < _nextTrap) return;
        _nextTrap = Time.unscaledTime + 0.12f + best / TrapRange * 0.5f;
        var place = TopDownLogic.Place(at.X - p.x, at.Y - p.y, TrapRange);
        // Keep the nearest trap audible even at the edge of range.
        ToneEngine.Play(Cue.Trap, place with { Volume = Math.Max(place.Volume, 0.08f) }, 0.7f);
    }

    private static void Enemies(Vector2 p)
    {
        var near = _enemies
            .Where(c => c != null && c.gameObject.activeInHierarchy)
            .Select(c => (Vector2)c.bounds.center)
            .OrderBy(e => (e - p).sqrMagnitude)
            .Take(MaxEnemies).ToList();
        for (var i = 0; i < MaxEnemies; i++)
        {
            if (i < near.Count)
            {
                var d = near[i] - p;
                ToneEngine.Loop(i, Cue.Hum, TopDownLogic.Place(d.x, d.y, EnemyRange), 0.8f);
            }
            else ToneEngine.Loop(i, Cue.Hum, default);
        }
    }

    // ---- beacon -------------------------------------------------------------------------------

    private static void Beacon(Vector2 p)
    {
        var target = WorldHandler.ScanTarget;
        if (target == null || Time.unscaledTime < _nextBeacon) return;
        _nextBeacon = Time.unscaledTime + 1.2f;
        var place = TopDownLogic.Place(target.Value.X - p.x, target.Value.Y - p.y, BeaconRange);
        ToneEngine.Play(Cue.Beacon, place with { Volume = Math.Max(place.Volume, 0.15f) }, 0.5f);
    }

    // ---- deaths and resets --------------------------------------------------------------------

    /// <summary>Speaks when a trap, a cultist or the dark maze's timer sends the player back.</summary>
    private static void Events()
    {
        var lists = KickStarter.actionListManager?.activeLists;
        if (lists == null) return;
        _scratch.Clear();
        for (var i = 0; i < lists.Count; i++)
        {
            var a = lists[i];
            var list = a?.actionList;
            if (list == null || !a.IsRunning()) continue;
            _scratch.Add(list.Pointer);
            if (_running.Contains(list.Pointer)) continue;
            var say = TopDownLogic.Event(list.gameObject.name);
            if (say != null) Speech.Say(say);
        }
        _running.Clear();
        _running.UnionWith(_scratch);
    }
}
