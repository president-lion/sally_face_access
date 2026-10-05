using Il2CppAC;
using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// The top-down arena where Sal fights with his guitar (Episode 5, 05_32_field): four blobs
/// ("enemy1".."enemy4") crawl at him from the corners; R blasts the ones close by; a touch is
/// game over; variable 33 counts kills and the arena ends after 14.
/// Each blob hums from where it is - stereo for left and right, pitch for above and below,
/// louder when near - and each kill is counted aloud.
/// </summary>
internal static class Arena
{
    private const float Range = 9f;   // about the arena's half width
    private const int Kills = 14;
    private static readonly string[] Enemies = { "enemy1", "enemy2", "enemy3", "enemy4" };

    private static bool _active;
    private static Transform[] _enemies = new Transform[4];
    private static float _nextGather;
    private static int _lastKills = -1;

    public static void OnSceneLoaded(string scene)
    {
        if (_active) for (var i = 0; i < Enemies.Length; i++) ToneEngine.Loop(i, ToneEngine.Cue.Hum, default);
        _active = scene == "05_32_field";
        _enemies = new Transform[4];
        _nextGather = 0;
        _lastKills = -1;
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
            if (talking || e == null || !e.gameObject.activeInHierarchy) { ToneEngine.Loop(i, ToneEngine.Cue.Hum, default); continue; }
            var d = e.position - p;
            ToneEngine.Loop(i, ToneEngine.Cue.Hum, TopDownLogic.Place(d.x, d.y, Range), 0.8f);
        }

        var kills = GlobalVariables.GetVariable(33)?.IntegerValue ?? 0;
        if (_lastKills >= 0 && kills > _lastKills && kills <= Kills)
            Speech.Say(string.Format(Strings.ArenaKills, kills, Kills), interrupt: true);
        _lastKills = kills;
    }
}
