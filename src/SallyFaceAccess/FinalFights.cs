using Il2CppAC;
using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// The last three fights against the Endless One, after the mouths:
///   05_endless_4_3D   - eight tentacles ("Endless 1".."Endless 8") lunge at Sal in turn; R ("Blast")
///                       fires lightning ("lightning" on Sal); a tentacle's "hitBox" meeting it kills
///                       it ("endlessN_death"); its "eatBox" reaching Sal eats him.
///   05_endless_5_Toon - three eyes ("Eye1".."Eye3") float in along their rows; R ("blast") strums a
///                       blast ("toonBlast"); "HitBox" -> "EyeDeathN", which counts variable 33 and
///                       sends the eye round again; six kills win. "EatBox" eats Sal.
///   05_endless_6_void - three orbs ("Enemy1".."Enemy3") come out of the spiked walls; R ("blast")
///                       ("blast1"); "deathBox" -> "death N", variable 33, twelve kills win. "eatBox" and
///                       the walls' "SpikeBox"es kill.
///
/// Each enemy hums from where it is (the nearest six; stereo for left and right, pitch for above and
/// below, louder when near), a chirp comes a moment before one is in reach of Sal's strike, and kills
/// are counted. Delete autoplays the fight: everything that kills Sal is switched off and he blasts
/// each enemy as it comes, until the fight is won.
/// </summary>
internal static class FinalFights
{
    private sealed record Fight(string[] Enemies, string HitBox, string Strike, float StrikeRadius, string Blast,
                                Func<int, string> Death, string[] Killers, int KillVar, int Kills, float Range);

    private static readonly Dictionary<string, Fight> Fights = new()
    {
        ["05_endless_4_3D"] = new(Enumerable.Range(1, 8).Select(i => $"Endless {i}").ToArray(), "hitBox", "lightning", 0.4f,
            "Blast", i => $"endless{i + 1}_death", new[] { "eatBox" }, -1, 8, 25f),
        ["05_endless_5_Toon"] = new(new[] { "Eye1", "Eye2", "Eye3" }, "HitBox", "toonBlast", 0.68f,
            "blast", i => $"EyeDeath{i + 1}", new[] { "EatBox" }, 33, 6, 30f),
        ["05_endless_6_void"] = new(new[] { "Enemy1", "Enemy2", "Enemy3" }, "deathBox", "blast1", 0.32f,
            "blast", i => $"death {i + 1}", new[] { "eatBox", "SpikeBox" }, 33, 12, 20f),
    };

    private const float Lead = 0.6f;        // seconds of warning before an enemy is in reach
    private const int Voices = 6;

    private static Fight _fight;
    private static Transform[] _enemies = Array.Empty<Transform>();
    private static Transform[] _hits = Array.Empty<Transform>();
    private static bool[] _seen = Array.Empty<bool>(), _close = Array.Empty<bool>(), _gone = Array.Empty<bool>();
    private static float[] _prevGap = Array.Empty<float>(), _closing = Array.Empty<float>(), _cooldown = Array.Empty<float>();
    private static Transform _strike;
    private static float _nextGather;
    private static int _lastKills = -1, _removed;
    private static bool _auto;
    private static float _nextAutoKill, _lastAutoKill;
    private static readonly Dictionary<string, ActionList> _lists = new();

    public static void OnSceneLoaded(string scene)
    {
        ToneEngine.StopLoops();
        _fight = Fights.TryGetValue(scene, out var f) ? f : null;
        var n = _fight?.Enemies.Length ?? 0;
        _enemies = new Transform[n];
        _hits = new Transform[n];
        _seen = new bool[n]; _close = new bool[n]; _gone = new bool[n];
        _prevGap = new float[n]; _closing = new float[n]; _cooldown = new float[n];
        _strike = null;
        _nextGather = 0;
        _lastKills = -1;
        _removed = 0;
        _auto = false;
        _lists.Clear();
    }

    private static Transform FindDeep(Transform t, string name)
    {
        if (t == null) return null;
        if (t.name == name) return t;
        for (var i = 0; i < t.childCount; i++)
            if (FindDeep(t.GetChild(i), name) is { } found) return found;
        return null;
    }

    private static ActionList List(string name)
    {
        if (_lists.TryGetValue(name, out var l) && l != null) return l;
        foreach (var a in UnityEngine.Object.FindObjectsOfType<ActionList>())
            if (a != null && a.gameObject.name == name) { _lists[name] = a; return a; }
        return null;
    }

    /// <summary>How far the enemy's hit box is from Sal's strike (0 or less: in reach).</summary>
    private static float Gap(int i)
    {
        var hit = _hits[i] ?? _enemies[i];
        var from = _strike != null ? _strike.position : KickStarter.player.transform.position;
        Bounds b;
        var c2 = hit.GetComponent<Collider2D>();
        if (c2 != null) b = c2.bounds;
        else
        {
            var c3 = hit.GetComponent<Collider>();
            if (c3 == null) return Vector3.Distance(hit.position, from) - _fight.StrikeRadius;
            b = c3.bounds;
        }
        if (c2 != null) from.z = b.center.z; // flat scenes: ignore depth
        return Mathf.Sqrt(b.SqrDistance(from)) - _fight.StrikeRadius;
    }

    public static void Tick()
    {
        if (_fight == null || KickStarter.player == null) return;
        if (Time.unscaledTime >= _nextGather)
        {
            _nextGather = Time.unscaledTime + 1f;
            _strike ??= FindDeep(KickStarter.player.transform, _fight.Strike);
            for (var i = 0; i < _enemies.Length; i++)
            {
                if (_enemies[i] != null) continue;
                var go = GameObject.Find(_fight.Enemies[i]);
                if (go == null) continue;
                _enemies[i] = go.transform;
                _hits[i] = FindDeep(go.transform, _fight.HitBox);
                _seen[i] = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Delete) && !_auto)
        {
            _auto = true;
            Speech.Say(Strings.FightAuto, interrupt: true);
        }

        var line = KickStarter.dialog?.GetLatestSpeech();
        var talking = line != null && line.isAlive && !line.isBackground;
        var p = KickStarter.player.transform.position;

        // Enemies in play, nearest first; the nearest six hum.
        var live = new List<(int I, float Gap)>();
        for (var i = 0; i < _enemies.Length; i++)
        {
            var e = _enemies[i];
            var alive = e != null && e.gameObject.activeInHierarchy;
            if (!alive)
            {
                // A tentacle that was there and is now removed has been killed.
                if (_seen[i] && !_gone[i] && _fight.KillVar < 0)
                {
                    _gone[i] = true;
                    _removed++;
                    Speech.Say(string.Format(Strings.ArenaKills, _removed, _fight.Kills), interrupt: true);
                }
                continue;
            }
            live.Add((i, Gap(i)));
        }
        live.Sort((a, b) => a.Gap.CompareTo(b.Gap));

        for (var v = 0; v < Voices; v++)
        {
            if (talking || v >= live.Count) { ToneEngine.Loop(v, ToneEngine.Cue.Hum, default); continue; }
            var d = _enemies[live[v].I].position - p;
            ToneEngine.Loop(v, ToneEngine.Cue.Hum, TopDownLogic.Place(d.x, d.y, _fight.Range), 0.8f);
        }

        var dt = Math.Max(Time.deltaTime, 0.001f);
        foreach (var (i, gap) in live)
        {
            if (_prevGap[i] != 0) _closing[i] = Mathf.Lerp(_closing[i], (_prevGap[i] - gap) / dt, 0.2f);
            _prevGap[i] = gap;
            var warnAt = Math.Max(Math.Max(_closing[i], 0f) * Lead, 0.3f);
            if (!_close[i] && gap < warnAt && !_auto)
            {
                _close[i] = true;
                var dx = _enemies[i].position.x - p.x;
                ToneEngine.Play(ToneEngine.Cue.Chirp, new TopDownLogic.Placement(Math.Clamp(dx / 4f, -1f, 1f), 1.3f, 1f));
            }
            else if (_close[i] && gap > warnAt + 1f) _close[i] = false;
        }

        if (_auto && !talking) Autoplay(live);

        if (_fight.KillVar >= 0)
        {
            var kills = GlobalVariables.GetVariable(_fight.KillVar)?.IntegerValue ?? 0;
            if (_lastKills >= 0 && kills > _lastKills && kills <= _fight.Kills)
                Speech.Say(string.Format(Strings.ArenaKills, kills, _fight.Kills), interrupt: true);
            _lastKills = kills;
        }
    }

    /// <summary>
    /// One frame of the autoplay: nothing can kill Sal; he blasts each enemy as it comes in
    /// (or, if none comes, the nearest after a few seconds), one at a time.
    /// </summary>
    private static void Autoplay(List<(int I, float Gap)> live)
    {
        foreach (var c in UnityEngine.Object.FindObjectsOfType<Collider2D>())
            if (c != null && c.enabled && _fight.Killers.Any(k => c.gameObject.name.StartsWith(k))) c.enabled = false;
        foreach (var c in UnityEngine.Object.FindObjectsOfType<Collider>())
            if (c != null && c.enabled && _fight.Killers.Any(k => c.gameObject.name.StartsWith(k))) c.enabled = false;

        var now = Time.unscaledTime;
        if (now < _nextAutoKill || live.Count == 0) return;
        foreach (var (i, gap) in live)
        {
            if (now < _cooldown[i]) continue;
            if (gap > 1.5f && now - _lastAutoKill < 3f) continue; // wait for it to come in
            Arena.AutoStrike(_fight.Blast);
            List(_fight.Death(i))?.Interact();
            _cooldown[i] = now + 2f;
            _lastAutoKill = now;
            _nextAutoKill = now + 0.8f;
            return;
        }
    }
}
