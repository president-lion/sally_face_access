using MelonLoader;
using MelonLoader.Utils;

[assembly: MelonInfo(typeof(SallyFaceAccess.Mod), "Sally Face Access", "0.1.0", "president-lion")]
[assembly: MelonGame("Steve Gabry", "Sally Face")]

namespace SallyFaceAccess;

/// <summary>Entry point: starts speech, applies the hooks, and runs the per-frame tick.</summary>
public sealed class Mod : MelonMod
{
    private bool _dumpedThisSession;

    public override void OnInitializeMelon()
    {
        Log.Init(LoggerInstance);
        Settings.Init();
        Speech.Preload(MelonEnvironment.UserLibsDirectory);
        Speech.Init(Settings.Backend);
        Speech.Say(Strings.Loaded);
    }

    public override void OnUpdate()
    {
        try { Hotkeys.Tick(); }
        catch (Exception ex) { Log.Once("Hotkeys", ex); }

        // Write the text dump once per session as soon as AC's managers exist, if there
        // is not one already. F11 rewrites it on demand.
        if (!_dumpedThisSession && TextDump.Ready)
        {
            _dumpedThisSession = true;
            try { if (!File.Exists(Path.Combine(TextDump.DumpDir, "lines.json"))) TextDump.WriteAll(); }
            catch (Exception ex) { Log.Once("Dump.auto", ex); }
        }
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        => Log.Debug("Scene", $"loaded {buildIndex} {sceneName}");

    public override void OnDeinitializeMelon() => Speech.Shutdown();
}
