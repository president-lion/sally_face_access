using MelonLoader;

namespace SallyFaceAccess;

/// <summary>User settings, stored by MelonLoader in UserData\MelonPreferences.cfg.</summary>
internal static class Settings
{
    private static MelonPreferences_Entry<string> _backend;
    private static MelonPreferences_Entry<bool> _debugLog;
    private static MelonPreferences_Entry<bool> _speakUnhandled;
    private static MelonPreferences_Entry<bool> _runInBackground;

    public static string Backend => _backend?.Value ?? "auto";
    public static bool DebugLog => _debugLog?.Value ?? false;
    public static bool RunInBackground => _runInBackground?.Value ?? false;

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
        _debugLog = cat.CreateEntry("DebugLog", false, "Verbose logging");
        _speakUnhandled = cat.CreateEntry("SpeakUnhandled", false, "Speak unhandled text",
            "Development aid: also speak on-screen text that no handler covers");
        _runInBackground = cat.CreateEntry("RunInBackground", false, "Keep running without focus",
            "Development aid: the game normally pauses when its window loses focus");
    }
}
