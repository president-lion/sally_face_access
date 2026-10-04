using Il2CppAC;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace SallyFaceAccess;

/// <summary>
/// Development tool (SelfTest setting): loads the Super Gear Boy game, puts the player at
/// spots whose surroundings are known from the level data, and logs what the positional
/// audio reads there - wall distances, nearest trap, cultists - next to the expected values,
/// then plays each cue and checks the voices actually sound. Saves are blocked while it runs.
/// </summary>
internal static class SelfTest
{
    private sealed record Spot(string Name, float X, float Y, string Expect);

    // Expected values worked out by hand from tour/02_sgb/geometry.txt (player radius 0.1).
    private static readonly Spot[] Spots =
    {
        new("ch4 floor 1 middle", 24.3f, -17.0f,
            "left ~1.06 (wall L edge 23.14); right open (edge 26.41 -> 2.0); up ~1.0 (T edge -15.89); down ~0.74 (B edge -17.91)"),
        new("ch1 above the bottom wall", 4.95f, 4.25f,
            "down ~0.27 (wall top 3.88); trap 'spikeTrigger' (4.96,3.97 2.0x0.3) ~0.13 away"),
        new("ch7 test 1 by an electric wall", 42.55f, -6.2f,
            "trap 'electricHitTrigger' (42.36,-6.50 0.08x1.3) ~0.15 away"),
    };

    private enum Step { Idle, Loading, Settling, Spots, Cues, Done }
    private static Step _step = Step.Idle;
    private static float _at;
    private static int _spot;
    private static bool _loaded;

    public static void OnSceneLoaded(string scene) { if (scene == "02_sgb") _loaded = true; }

    public static void Tick()
    {
        if (!Settings.SelfTest || _step == Step.Done) return;
        var t = Time.unscaledTime;
        try
        {
            switch (_step)
            {
                case Step.Idle:
                    if (KickStarter.sceneChanger == null) return;
                    Log.Info("SelfTest", "loading 02_sgb");
                    for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                        if (System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)) == "02_sgb")
                            KickStarter.sceneChanger.ChangeScene(new SceneInfo(i), false, true);
                    _step = Step.Loading; _at = t;
                    break;

                case Step.Loading:
                    if (_loaded && t - _at > 3f) { _step = Step.Settling; _at = t; }
                    break;

                case Step.Settling:
                    if (t - _at < 4f) break;
                    KickStarter.actionListManager?.EndCutscene();
                    var menus = PlayerMenus.GetMenus(true);
                    for (var i = 0; menus != null && i < menus.Count; i++)
                        if (menus[i] != null && menus[i].IsOn()) menus[i].ForceOff(true);
                    TopDownAudio.Gather();
                    _step = Step.Spots; _spot = 0; _at = t;
                    Place(Spots[0]);
                    break;

                case Step.Spots:
                    if (t - _at < 0.5f) break; // let physics catch up with the teleport
                    Report(Spots[_spot]);
                    if (++_spot < Spots.Length) { Place(Spots[_spot]); _at = t; }
                    else { _step = Step.Cues; _at = t; }
                    break;

                case Step.Cues:
                    var p = new TopDownLogic.Placement(0f, 1f, 1f);
                    foreach (ToneEngine.Cue c in Enum.GetValues(typeof(ToneEngine.Cue)))
                        if (c != ToneEngine.Cue.Hum) ToneEngine.Play(c, p);
                    ToneEngine.Loop(0, ToneEngine.Cue.Hum, p);
                    Log.Info("SelfTest", $"cues started; voices sounding: {ToneEngine.Playing} (expect 6)");
                    ToneEngine.StopAll();
                    Log.Info("SelfTest", "done");
                    _step = Step.Done;
                    break;
            }
        }
        catch (Exception ex)
        {
            Log.Warn("SelfTest", $"failed at {_step}: {ex}");
            _step = Step.Done;
        }
    }

    private static void Place(Spot s)
    {
        KickStarter.player.Teleport(new Vector3(s.X, s.Y, 0f));
        Physics2D.SyncTransforms();
    }

    private static void Report(Spot s)
    {
        Vector2 p = KickStarter.player.transform.position;
        string W(int d) => TopDownAudio.Wall(p, d) is { } v ? v.ToString("0.00") : "open";
        var (trap, traps, enemies) = TopDownAudio.Survey(p);
        Log.Info("SelfTest", $"{s.Name} at ({p.x:0.00},{p.y:0.00}): up {W(0)} down {W(1)} left {W(2)} right {W(3)}; " +
                             $"nearest trap {trap:0.00} (of {traps}); cultists known {enemies}");
        Log.Info("SelfTest", $"  expected: {s.Expect}");
    }
}
