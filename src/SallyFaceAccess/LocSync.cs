using Il2CppAC;

namespace SallyFaceAccess;

/// <summary>
/// Keeps Loc on the language picked in the game's own menu: checked every frame, and the
/// language's table is loaded when it changes - the built-in file first, then the player's
/// UserData\SallyFaceAccess\lang\&lt;code&gt;.json over it.
/// </summary>
internal static class LocSync
{
    private static int _index = -1;

    public static void Tick()
    {
        var sm = KickStarter.speechManager;
        if (sm == null || sm.languages == null) return;
        var index = Options.GetLanguage();
        if (index == _index || index < 0 || index >= sm.languages.Count) return;
        _index = index;

        var code = Loc.Code(sm.languages[index]);
        if (code == null) Log.Warn("Loc", $"unknown game language '{sm.languages[index]}', using English");
        Loc.Use(code ?? "en", code == null || code == "en" ? null : Load(code));
        Log.Info("Loc", $"language {code ?? "en"}");
    }

    private static Dictionary<string, string> Load(string code)
    {
        var table = new Dictionary<string, string>();
        try
        {
            using var s = typeof(LocSync).Assembly.GetManifestResourceStream($"lang.{code}.json");
            if (s != null)
                using (var r = new StreamReader(s))
                    foreach (var kv in Loc.Parse(r.ReadToEnd())) table[kv.Key] = kv.Value;
        }
        catch (Exception ex) { Log.Once("Loc.builtin", ex); }

        var own = Path.Combine(Log.Dir, "lang", code + ".json");
        try
        {
            if (File.Exists(own))
                foreach (var kv in Loc.Parse(File.ReadAllText(own))) table[kv.Key] = kv.Value;
        }
        catch (Exception ex) { Log.Warn("Loc", $"could not read {own}: {ex.Message}"); }
        return table;
    }
}
