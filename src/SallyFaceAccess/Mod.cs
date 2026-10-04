using MelonLoader;
using MelonLoader.Utils;

[assembly: MelonInfo(typeof(SallyFaceAccess.Mod), "Sally Face Access", "0.1.0", "president-lion")]
[assembly: MelonGame("Steve Gabry", "Sally Face")]

namespace SallyFaceAccess;

/// <summary>Entry point: starts speech, applies the hooks, and runs the per-frame tick.</summary>
public sealed class Mod : MelonMod
{
    private bool _dumpedThisSession;
    private float _nextHeartbeat;

    public override void OnInitializeMelon()
    {
        Log.Init(LoggerInstance);
        Settings.Init();
        Speech.Preload(MelonEnvironment.UserLibsDirectory);
        Speech.Init(Settings.Backend);
        Speech.Say(Strings.Loaded);
    }

    public override void OnLateInitializeMelon()
    {
        if (Settings.RunInBackground) UnityEngine.Application.runInBackground = true;
    }

    public override void OnUpdate()
    {
        if (Settings.DebugLog && UnityEngine.Time.unscaledTime >= _nextHeartbeat)
        {
            _nextHeartbeat = UnityEngine.Time.unscaledTime + 3f;
            try { Heartbeat(); } catch (Exception ex) { Log.Once("Heartbeat", ex); }
        }

        try { Hotkeys.Tick(); }
        catch (Exception ex) { Log.Once("Hotkeys", ex); }

        try { MenuHandler.Tick(); }
        catch (Exception ex) { Log.Once("MenuHandler", ex); }

        try { DialogueHandler.Tick(); }
        catch (Exception ex) { Log.Once("DialogueHandler", ex); }

        try { InventoryHandler.Tick(); }
        catch (Exception ex) { Log.Once("InventoryHandler", ex); }

        try { WorldHandler.Tick(); }
        catch (Exception ex) { Log.Once("WorldHandler", ex); }

        try { ItemHints.Tick(); }
        catch (Exception ex) { Log.Once("ItemHints", ex); }

        try { GoalHandler.Tick(); }
        catch (Exception ex) { Log.Once("GoalHandler", ex); }
        try { Detector.Tick(); }
        catch (Exception ex) { Log.Once("Detector", ex); }

        try { CutsceneHandler.Tick(); }
        catch (Exception ex) { Log.Once("CutsceneHandler", ex); }

        try { TourMode.Tick(); }
        catch (Exception ex) { Log.Once("TourMode", ex); }

        // Write the text dump once per session as soon as AC's managers exist, if there
        // is not one already. F11 rewrites it on demand.
        if (!_dumpedThisSession && TextDump.Ready)
        {
            _dumpedThisSession = true;
            try { if (!File.Exists(Path.Combine(TextDump.DumpDir, "lines.json"))) TextDump.WriteAll(); }
            catch (Exception ex) { Log.Once("Dump.auto", ex); }
        }
    }

    /// <summary>Diagnostic: proves the tick runs and shows what AC thinks is on screen.</summary>
    private static void Heartbeat()
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        var menus = Il2CppAC.KickStarter.playerMenus != null ? Il2CppAC.PlayerMenus.GetMenus(true) : null;
        var on = new List<string>();
        for (var i = 0; menus != null && i < menus.Count; i++)
            if (menus[i] != null && menus[i].IsOn()) on.Add(menus[i].title);
        Log.Info("Beat", $"t={UnityEngine.Time.unscaledTime:0} scene={scene} state={Il2CppAC.KickStarter.stateHandler?.gameState} " +
                         $"menus={(menus == null ? "null" : menus.Count.ToString())} on=[{string.Join(", ", on)}]");
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        Log.Debug("Scene", $"loaded {buildIndex} {sceneName}");
        try { InventoryHandler.OnSceneLoaded(); GoalHandler.OnSceneLoaded(); Detector.OnSceneLoaded(); CutsceneHandler.OnSceneLoaded(sceneName); TourMode.OnSceneLoaded(sceneName); ItemHints.OnSceneLoaded(sceneName); WorldHandler.OnSceneLoaded(sceneName); }
        catch (Exception ex) { Log.Once("SceneLoaded", ex); }
    }

    public override void OnDeinitializeMelon() => Speech.Shutdown();
}
