using Il2CppAC;
using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// The top-down arenas where Sal fights with his guitar (Episode 5).
///
/// 05_32_field: four blobs ("enemy1".."enemy4") crawl at him from the corners; R blasts the ones
/// close by; a touch is game over; variable 33 counts kills and the arena ends after 14.
/// Each blob hums from where it is - stereo for left and right, pitch for above and below,
/// louder when near - and each kill is counted aloud.
///
/// 05_32_boss: the same four blobs, and two giant arms (Arm1 left, Arm2 right; each animates its
/// child "arm1"). Sal can only walk left and right. An arm opens ("armOpen"), a second later slams
/// down on a fixed spot of Sal's row ("armAttack"/"armAttack2") - eating him if he stands there -
/// and stays down three seconds ("armBack" pulls it up) while R can hit it; variable 33 counts
/// arm hits. The rise is announced with where it will land, and a beacon sounds at the hand while
/// it is down.
/// </summary>
internal static class Arena
{
    private const int Kills = 14;
    private static readonly string[] Enemies = { "enemy1", "enemy2", "enemy3", "enemy4" };

    private static bool _active, _boss;
    private static float _range = 9f;   // about the arena's half width
    private static Transform[] _enemies = new Transform[4];
    private static float _nextGather;
    private static int _lastKills = -1;
    private static readonly float[] _mutedUntil = new float[4];
    private const float DeathSilence = 1.2f; // the death animation, then the respawn at its corner
    private const float StrikeRange = 0.6f;  // a blob this close: press R now
    private static readonly bool[] _close = new bool[4];

    // The arms: 0 = left, 1 = right; loop voices 4 and 5.
    private const float Reach = 0.45f;      // how close to the strike spot counts as under it
    private static readonly Transform[] _arms = new Transform[2];
    private static readonly string[] _armClip = new string[2];
    private static readonly bool[] _armDown = new bool[2];

    /// <summary>A blob's death animation started: its hum stops now, not when it reappears.</summary>
    public static void Killed(Il2CppAC.Char c)
    {
        if (!_active || c == null) return;
        var i = Array.IndexOf(Enemies, c.gameObject.name);
        if (i < 0) return;
        _mutedUntil[i] = Time.unscaledTime + DeathSilence;
        ToneEngine.Loop(i, ToneEngine.Cue.Hum, default);
    }

    /// <summary>An ActionAnim started on an animator: in the boss arena, an arm moving.</summary>
    public static void ArmAnim(Animator a, string clip)
    {
        if (!_boss || a == null || clip == null || !clip.StartsWith("arm") || KickStarter.player == null) return;
        var side = a.transform.position.x < 0 ? 0 : 1;
        _arms[side] = a.transform;
        if (_armClip[side] == clip) return; // AC calls Run again while it waits
        _armClip[side] = clip;

        var name = side == 0 ? Strings.Left : Strings.Right;
        var where = TopDownLogic.Beside(Strike(side) - KickStarter.player.transform.position.x, Reach);
        switch (clip)
        {
            case "armOpen":
                Speech.Say(where == Strings.OnYou ? string.Format(Strings.ArmMove, name) : string.Format(Strings.ArmRising, name, where),
                    interrupt: true);
                break;
            case "armAttack":
            case "armAttack2":
                _armDown[side] = true;
                Speech.Say(string.Format(Strings.ArmDown, name, where), interrupt: true);
                break;
            case "armBack":
            case "armBack2":
                if (_armDown[side]) Speech.Say(string.Format(Strings.ArmBack, name), interrupt: false);
                _armDown[side] = false;
                break;
            default:
                _armDown[side] = false;
                break;
        }
    }

    /// <summary>
    /// Where an arm's hand lands on Sal's row: the hit trigger while it is down, otherwise the
    /// hand's position in the slam animation (about 1.2 inward of the arm's root).
    /// </summary>
    private static float Strike(int side)
    {
        var arm = _arms[side];
        if (arm == null) return 0f;
        var hit = arm.Find("Trigger_hit");
        if (hit != null && hit.gameObject.activeInHierarchy && _armDown[side]) return hit.position.x;
        return arm.position.x + (side == 0 ? 1.18f : -1.19f);
    }

    public static void OnSceneLoaded(string scene)
    {
        if (_active) for (var i = 0; i < 6; i++) ToneEngine.Loop(i, ToneEngine.Cue.Hum, default);
        _boss = scene == "05_32_boss";
        _active = scene == "05_32_field" || _boss;
        _range = _boss ? 6f : 9f;
        _enemies = new Transform[4];
        _nextGather = 0;
        _lastKills = -1;
        Array.Clear(_mutedUntil);
        Array.Clear(_close);
        Array.Clear(_arms);
        Array.Clear(_armClip);
        Array.Clear(_armDown);
    }

    public static void Tick()
    {
        if (!_active || KickStarter.player == null) return;
        if (Time.unscaledTime >= _nextGather)
        {
            _nextGather = Time.unscaledTime + 2f;
            foreach (var t in UnityEngine.Object.FindObjectsOfType<Il2CppAC.Char>())
                if (t != null && Array.IndexOf(Enemies, t.gameObject.name) is var i && i >= 0) _enemies[i] = t.transform;
        }

        // Silent while someone talks; the blobs only move in play.
        var line = KickStarter.dialog?.GetLatestSpeech();
        var talking = line != null && line.isAlive && !line.isBackground;
        var p = KickStarter.player.transform.position;
        for (var i = 0; i < _enemies.Length; i++)
        {
            var e = _enemies[i];
            if (talking || e == null || !e.gameObject.activeInHierarchy || Time.unscaledTime < _mutedUntil[i])
            {
                ToneEngine.Loop(i, ToneEngine.Cue.Hum, default);
                _close[i] = false;
                continue;
            }
            var d = e.position - p;
            ToneEngine.Loop(i, ToneEngine.Cue.Hum, TopDownLogic.Place(d.x, d.y, _range), 0.8f);

            // R only reaches a blob that is nearly touching (Sal's hit box 0.4, the blob's 0.4;
            // its bite box 0.35): a chirp says "now", once per approach.
            var dist = new Vector2(d.x, d.y).magnitude;
            if (!_close[i] && dist < StrikeRange)
            {
                _close[i] = true;
                ToneEngine.Play(ToneEngine.Cue.Chirp, new TopDownLogic.Placement(Math.Clamp(d.x * 2f, -1f, 1f), 1.3f, 1f));
            }
            else if (_close[i] && dist > StrikeRange + 0.3f) _close[i] = false;
        }

        // A downed arm's hand: a beacon to walk to and hit.
        for (var side = 0; side < 2; side++)
        {
            if (!_boss || talking || !_armDown[side]) { ToneEngine.Loop(4 + side, ToneEngine.Cue.Beacon, default); continue; }
            var dx = Strike(side) - p.x;
            ToneEngine.Loop(4 + side, ToneEngine.Cue.Beacon, TopDownLogic.Place(dx, 0f, _range), 1f);
        }

        var kills = GlobalVariables.GetVariable(33)?.IntegerValue ?? 0;
        if (_lastKills >= 0 && kills > _lastKills && (_boss || kills <= Kills))
            Speech.Say(_boss ? string.Format(Strings.ArmHit, kills) : string.Format(Strings.ArenaKills, kills, Kills), interrupt: true);
        _lastKills = kills;
    }
}
