using System.Text;
using Il2CppAC;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;
using BindingFlags = Il2CppSystem.Reflection.BindingFlags;

namespace SallyFaceAccess;

/// <summary>
/// Development tool (Tour setting): visits every scene in the build unattended and writes,
/// per scene, to UserData\SallyFaceAccess\tour\scene\:
///   scene.txt       objects, cameras, and every ActionList's actions (the cutscene scripts)
///   *.png           a screenshot through every camera; scrolling cameras are panned across
///                   the room by placing the camera directly - the player is never moved,
///                   so no trigger, cutscene or scene change can fire from the tour itself.
/// Progress is saved after each scene, so a crash resumes where it stopped.
/// </summary>
internal static class TourMode
{
    private enum Step { Idle, Loading, Settling, Shooting, Done }

    private static readonly HashSet<string> Skip = new() { "portableMooseSplash", "loadingScreen", "mainMenu" };
    private const int MaxShotsPerCamera = 8;

    private static Step _step = Step.Idle;
    private static int _index;
    private static float _stepStarted, _nextAt;
    private static bool _sceneLoaded, _skippedCutscene;
    private static readonly Queue<System.Action> _shots = new();

    public static bool Active => _step is not (Step.Idle or Step.Done);
    private static string Dir => Path.Combine(Log.Dir, "tour");
    private static string ProgressFile => Path.Combine(Dir, "progress.txt");

    public static void OnSceneLoaded(string scene) => _sceneLoaded = true;

    public static void Tick()
    {
        if (!Settings.Tour || _step == Step.Done) return;
        if (_step == Step.Idle)
        {
            // Start once AC is up (the title screen is enough).
            if (KickStarter.sceneChanger == null) return;
            Directory.CreateDirectory(Dir);
            _index = File.Exists(ProgressFile) && int.TryParse(File.ReadAllText(ProgressFile), out var i) ? i : 0;
            Speech.Muted = true;
            Log.Info("Tour", $"starting at build index {_index} of {SceneManager.sceneCountInBuildSettings}");
            NextScene();
            return;
        }

        var t = Time.unscaledTime;
        try
        {
            switch (_step)
            {
                case Step.Loading:
                    if (_sceneLoaded && SceneManager.GetActiveScene().buildIndex == _index && t - _stepStarted > 2f)
                        Begin(Step.Settling);
                    else if (t - _stepStarted > 40f) { Log.Warn("Tour", $"scene {_index} did not load"); Finish(); }
                    break;

                case Step.Settling:
                    // Let the opening cutscene play; skip it if it runs long, then go anyway.
                    var normal = KickStarter.stateHandler?.gameState == GameState.Normal;
                    if (!normal && !_skippedCutscene && t - _stepStarted > 8f)
                    {
                        _skippedCutscene = true;
                        KickStarter.actionListManager?.EndCutscene();
                    }
                    if ((normal && t - _stepStarted > 1.5f) || t - _stepStarted > 16f)
                    {
                        KickStarter.mainCamera?.FadeIn(0f);
                        WriteSceneText();
                        QueueShots();
                        Begin(Step.Shooting);
                    }
                    break;

                case Step.Shooting:
                    if (t < _nextAt) break;
                    if (_shots.Count == 0) { Finish(); break; }
                    _shots.Dequeue()();
                    break;
            }
        }
        catch (Exception ex)
        {
            Log.Once($"Tour.{_index}", ex);
            Finish();
        }
    }

    private static void Begin(Step s)
    {
        _step = s;
        _stepStarted = Time.unscaledTime;
    }

    private static void NextScene()
    {
        var count = SceneManager.sceneCountInBuildSettings;
        while (_index < count && Skip.Contains(SceneName(_index))) _index++;
        if (_index >= count)
        {
            _step = Step.Done;
            Speech.Muted = false;
            Log.Info("Tour", "complete");
            Speech.Say(Strings.TourDone);
            return;
        }
        Log.Info("Tour", $"loading {_index} {SceneName(_index)}");
        _sceneLoaded = false;
        _skippedCutscene = false;
        _shots.Clear();
        KickStarter.sceneChanger.ChangeScene(new SceneInfo(_index), false, true);
        Begin(Step.Loading);
    }

    private static void Finish()
    {
        _index++;
        try { File.WriteAllText(ProgressFile, _index.ToString()); } catch { }
        NextScene();
    }

    private static string SceneName(int i) => Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));

    private static string SceneDir() => Path.Combine(Dir, SceneName(_index));

    // ---- screenshots ----------------------------------------------------------------------

    private static void QueueShots()
    {
        Directory.CreateDirectory(SceneDir());
        foreach (var cam in UnityEngine.Object.FindObjectsOfType<_Camera>())
        {
            if (cam == null) continue;
            var c = cam;
            var name = Safe(c.gameObject.name);
            var xs = PanPositions(c);
            for (var n = 0; n < xs.Count; n++)
            {
                var x = xs[n];
                var file = xs.Count == 1 ? $"{name}.png" : $"{name}_{n + 1}.png";
                _shots.Enqueue(() => Aim(c, x));
                _shots.Enqueue(() => Shoot(file));
            }
        }
    }

    /// <summary>Where to point a camera: once if it is fixed, panned across the room if it scrolls.</summary>
    private static List<float?> PanPositions(_Camera cam)
    {
        var c2 = cam.TryCast<GameCamera2D>();
        if (c2 == null || c2.lockHorizontal) return new List<float?> { null };

        var unity = KickStarter.CameraMain;
        var width = unity != null && unity.orthographic ? 2f * unity.orthographicSize * unity.aspect : 19.2f;
        float lo, hi;
        if (c2.limitHorizontal) { lo = c2.constrainHorizontal.x; hi = c2.constrainHorizontal.y; }
        else
        {
            // Unlimited: span the objects on this camera's level.
            var camY = cam.transform.position.y;
            var xs = UnityEngine.Object.FindObjectsOfType<Hotspot>()
                .Where(h => h != null && Math.Abs(h.transform.position.y - camY) < 8f)
                .Select(h => h.transform.position.x).ToList();
            if (xs.Count == 0) return new List<float?> { null };
            lo = xs.Min(); hi = xs.Max();
        }
        var steps = Math.Clamp((int)Math.Ceiling((hi - lo) / (width * 0.8f)) + 1, 1, MaxShotsPerCamera);
        return Enumerable.Range(0, steps)
            .Select(i => (float?)(steps == 1 ? (lo + hi) / 2 : lo + (hi - lo) * i / (steps - 1))).ToList();
    }

    private static void Aim(_Camera cam, float? x)
    {
        if (cam == null) return;
        if (x.HasValue)
        {
            // Stop the camera following anything and place it directly, keeping the height
            // the scene gives it - following a target adds offsets that frame the shot wrong.
            cam.enabled = false;
            var p = cam.transform.position;
            cam.transform.position = new Vector3(x.Value, p.y, p.z);
        }
        KickStarter.mainCamera.SetGameCamera(cam, 0f);
        KickStarter.mainCamera.SnapToAttached();
        KickStarter.mainCamera.FadeIn(0f);
        // Nothing drawn over the scene: splash, subtitle and prompt menus all off.
        var menus = PlayerMenus.GetMenus(true);
        for (var i = 0; menus != null && i < menus.Count; i++)
            if (menus[i] != null && menus[i].IsOn()) menus[i].ForceOff(true);
        _nextAt = Time.unscaledTime + 0.8f; // let it render (and any damping settle)
    }

    private static void Shoot(string file)
    {
        ScreenCapture.CaptureScreenshot(Path.Combine(SceneDir(), file));
        _nextAt = Time.unscaledTime + 0.3f;
    }

    private static string Safe(string s) => string.Join("_", s.Split(Path.GetInvalidFileNameChars()));

    // ---- text -----------------------------------------------------------------------------

    private static void WriteSceneText()
    {
        var sb = new StringBuilder();
        var lang = Options.GetLanguage();
        sb.AppendLine($"Scene {_index} {SceneName(_index)}  area='{WorldTables.Area(SceneName(_index))}'");

        sb.AppendLine().AppendLine("== Cameras");
        foreach (var cam in UnityEngine.Object.FindObjectsOfType<_Camera>())
        {
            if (cam == null) continue;
            var c2 = cam.TryCast<GameCamera2D>();
            var lim = c2 == null ? "" : $" lockH={c2.lockHorizontal} limitH={c2.limitHorizontal} constrainH={c2.constrainHorizontal}";
            sb.AppendLine($"  '{cam.gameObject.name}' at {cam.transform.position} {cam.GetIl2CppType().Name}{lim}");
        }

        sb.AppendLine().AppendLine("== Hotspots");
        foreach (var h in UnityEngine.Object.FindObjectsOfType<Hotspot>().OrderBy(h => h.transform.position.x))
        {
            if (h == null) continue;
            var p = h.transform.position;
            sb.AppendLine($"  ({p.x:0.0},{p.y:0.0}) on={h.IsOn()} active={h.gameObject.activeInHierarchy} " +
                          $"obj='{h.gameObject.name}' label='{h.GetName(lang)}' -> '{WorldHandler.Name(h)}'");
        }

        sb.AppendLine().AppendLine("== ActionLists");
        foreach (var list in UnityEngine.Object.FindObjectsOfType<ActionList>().OrderBy(l => l.gameObject.name))
        {
            if (list == null) continue;
            sb.AppendLine($"-- {list.GetIl2CppType().Name} '{list.gameObject.name}' source={list.source}" +
                          (list.assetFile != null ? $" asset='{list.assetFile.name}'" : ""));
            var actions = list.source == ActionListSource.AssetFile && list.assetFile != null
                ? list.assetFile.actions : list.actions;
            AppendActions(sb, actions);
        }

        Directory.CreateDirectory(SceneDir());
        File.WriteAllText(Path.Combine(SceneDir(), "scene.txt"), sb.ToString());
    }

    private static void AppendActions(StringBuilder sb, Il2CppSystem.Collections.Generic.List<Il2CppAC.Action> actions)
    {
        for (var i = 0; actions != null && i < actions.Count; i++)
        {
            var a = actions[i];
            if (a == null) continue;
            sb.Append($"   {i}. {a.GetIl2CppType().Name}");
            if (!a.isEnabled) sb.Append(" [disabled]");
            if (!string.IsNullOrEmpty(a.comment)) sb.Append($" // {a.comment}");
            sb.Append(' ').AppendLine(Fields(a));
        }
    }

    /// <summary>The action's own public fields, skipping empty ones, via IL2CPP reflection.</summary>
    private static string Fields(Il2CppAC.Action a)
    {
        var parts = new List<string>();
        foreach (var f in a.GetIl2CppType().GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            if (f.DeclaringType.Name == "Action") continue; // base fields: bookkeeping
            if (f.Name.EndsWith("ParameterID") || f.Name.EndsWith("ConstantID") || f.Name is "parameterID" or "constantID"
                || f.Name.EndsWith("Actual")) continue;          // wiring, not content
            string v;
            try
            {
                var o = f.GetValue(a);
                if (o == null) continue;
                // Boxed numbers must be unboxed explicitly; ToString on them reads garbage.
                v = f.FieldType.FullName switch
                {
                    "System.Int32" => o.Unbox<int>().ToString(),
                    "System.Single" => o.Unbox<float>().ToString("0.##"),
                    "System.Boolean" => o.Unbox<bool>().ToString(),
                    _ => o.TryCast<UnityEngine.Object>() is { } uo ? $"<{uo.name}>" : o.ToString(),
                };
            }
            catch { continue; }
            if (string.IsNullOrEmpty(v) || v is "False" or "0" or "-1" or "None" or "<(Clone)>") continue;
            if (v.StartsWith("Il2CppSystem.Collections") || v.StartsWith("System.Collections")) continue;
            if (v.Length > 160) v = v[..160] + "…";
            parts.Add($"{f.Name}={v}");
        }
        return string.Join(" ", parts);
    }
}
