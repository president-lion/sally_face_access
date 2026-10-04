using MelonLoader;

namespace SallyFaceAccess;

/// <summary>User settings, stored by MelonLoader in UserData\MelonPreferences.cfg.</summary>
internal static class Settings
{
    private static MelonPreferences_Entry<string> _backend;
    private static MelonPreferences_Entry<bool> _debugLog;
    private static MelonPreferences_Entry<bool> _speakUnhandled;
    private static MelonPreferences_Entry<bool> _runInBackground;
    private static MelonPreferences_Entry<bool> _speakerNames;
    private static MelonPreferences_Entry<float> _stepSize;
    private static MelonPreferences_Entry<bool> _captureCutscenes;
    private static MelonPreferences_Entry<bool> _tour;

    public static string Backend => _backend?.Value ?? "auto";
    public static bool DebugLog => _debugLog?.Value ?? false;
    public static bool RunInBackground => _runInBackground?.Value ?? false;
    public static bool SpeakerNames => _speakerNames?.Value ?? true;
    public static bool Tour => _tour?.Value ?? false;
    public static bool CaptureCutscenes => _captureCutscenes?.Value ?? false;
    public static float StepSize => Math.Max(0.01f, _stepSize?.Value ?? 1f);

    public static bool SpeakUnhandled
    {
        get => _speakUnhandled?.Value ?? false;
        set { if (_speakUnhandled != null) _speakUnhandled.Value = value; }
    }

    public static void Init()
    {
        var cat = MelonPreferences.CreateCategory("SallyFaceAccess", "Sally Face Access");
        _backend = cat.CreateEntry("Backend", "auto", "Speech backend",
            "Prism backend name (nvda, jaws, sapi, onecore, ...) or auto");
        _speakerNames = cat.CreateEntry("SpeakerNames", true, "Speak speaker names",
            "Say who is talking before each line of dialogue");
        _stepSize = cat.CreateEntry("StepSize", 1f, "Step size",
            "World units per 'step' when the room scan gives distances");
        _captureCutscenes = cat.CreateEntry("CaptureCutscenes", false, "Capture cutscenes",
            "Development aid: screenshot cutscenes so they can be described");
        _tour = cat.CreateEntry("Tour", false, "Tour the game",
            "Development tool: visit every scene, screenshot every camera, dump objects and cutscene scripts");
        _debugLog = cat.CreateEntry("DebugLog", false, "Verbose logging");
        _speakUnhandled = cat.CreateEntry("SpeakUnhandled", false, "Speak unhandled text",
            "Development aid: also speak on-screen text that no handler covers");
        _runInBackground = cat.CreateEntry("RunInBackground", false, "Keep running without focus",
            "Development aid: the game normally pauses when its window loses focus");
    }
}
