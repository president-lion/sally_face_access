using Il2CppAC;
using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// Describes visual events that carry no text. A cutscene is an AC ActionList; when one
/// listed in <see cref="Descriptions"/> starts, its description is spoken.
///
/// Development aid (CaptureCutscenes setting): while the game is in a cutscene, saves a
/// screenshot every 1.5 s to UserData\SallyFaceAccess\cutscenes\scene\, with a log tying
/// each picture to the running ActionLists and the last line spoken - the raw material
/// for writing the descriptions.
/// </summary>
internal static class CutsceneHandler
{
    // Key "scene/ActionList name". Mod wording, English for now.
    private static readonly Dictionary<string, string> Descriptions = new()
    {
    };

    private static readonly HashSet<string> _running = new();
    private static readonly HashSet<string> _now = new();
    private static float _nextShot;
    private static int _shot;
    private static string _scene = "";

    public static void OnSceneLoaded(string scene)
    {
        _scene = scene;
        _running.Clear();
        _shot = 0;
    }

    public static void Tick()
    {
        var lists = KickStarter.actionListManager?.activeLists;
        if (lists == null) return;

        _now.Clear();
        for (var i = 0; i < lists.Count; i++)
        {
            var a = lists[i];
            if (a == null || !a.IsRunning()) continue;
            var name = a.actionList != null ? a.actionList.gameObject.name
                     : a.actionListAsset != null ? a.actionListAsset.name : null;
            if (name == null) continue;
            _now.Add(name);
            if (_running.Contains(name)) continue;

            Log.Debug("Cutscene", $"start {_scene}/{name}");
            if (Descriptions.TryGetValue($"{_scene}/{name}", out var text)) Speech.Say(text, interrupt: false);
        }
        _running.Clear();
        _running.UnionWith(_now);

        if (Settings.CaptureCutscenes) Capture();
    }

    private static void Capture()
    {
        if (KickStarter.stateHandler?.gameState != GameState.Cutscene || Time.unscaledTime < _nextShot) return;
        _nextShot = Time.unscaledTime + 1.5f;

        var dir = Path.Combine(Log.Dir, "cutscenes", _scene);
        Directory.CreateDirectory(dir);
        var file = $"{_shot++:000}.png";
        ScreenCapture.CaptureScreenshot(Path.Combine(dir, file));
        File.AppendAllText(Path.Combine(dir, "log.txt"),
            $"{file} lists=[{string.Join(", ", _now)}] last=\"{Speech.Last}\"\n");
    }
}
