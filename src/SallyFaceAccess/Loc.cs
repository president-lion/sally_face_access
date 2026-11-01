using System.Text.Json;

namespace SallyFaceAccess;

/// <summary>
/// Translations of the mod's own wording into the game's languages. Every English text is
/// looked up by a short hash of itself, so a text the code changes simply falls back to
/// English until its translation is updated. Translation files are { "hash": "text" }:
/// lang/&lt;code&gt;.json, built into the DLL, with UserData\SallyFaceAccess\lang\&lt;code&gt;.json
/// on top for players' own fixes. lang/en.json lists every hash with its English.
/// Pure logic - no Unity types - so it is unit tested.
/// </summary>
internal static class Loc
{
    // The game's language names (SpeechManager.languages) -> translation file codes.
    private static readonly Dictionary<string, string> Codes = new()
    {
        ["English"] = "en",
        ["Русский"] = "ru",
        ["Português (BR)"] = "pt-BR",
        ["Deutsch"] = "de",
        ["Italiano"] = "it",
        ["繁體中文"] = "zh-Hant",
        ["Français"] = "fr",
        ["Español"] = "es",
        ["Simplified Chinese"] = "zh-Hans",
        ["Japanese"] = "ja",
        ["Korean"] = "ko",
        ["Polish"] = "pl",
    };

    public static IEnumerable<string> AllCodes => Codes.Values;

    /// <summary>The file code for one of the game's language names, or null for an unknown language.</summary>
    public static string Code(string gameLanguage) =>
        Codes.TryGetValue((gameLanguage ?? "").Trim(), out var c) ? c : null;

    private static Dictionary<string, string> _table = new();
    private static readonly Dictionary<string, string> _cache = new();

    public static string Current { get; private set; } = "en";

    /// <summary>Switches to a language's table; an empty one means English.</summary>
    public static void Use(string code, IReadOnlyDictionary<string, string> table)
    {
        Current = code ?? "en";
        _table = table == null ? new() : new Dictionary<string, string>(table);
        _cache.Clear();
    }

    /// <summary>The current language's wording for an English text, or the English itself.</summary>
    public static string T(string english)
    {
        if (string.IsNullOrEmpty(english) || _table.Count == 0) return english;
        if (_cache.TryGetValue(english, out var t)) return t;
        t = _table.TryGetValue(Hash(english), out var found) && found.Length > 0 ? found : english;
        _cache[english] = t;
        return t;
    }

    /// <summary>A text's key: FNV-1a over its UTF-16 code units, 40 bits as 10 hex digits.</summary>
    public static string Hash(string english)
    {
        var h = 14695981039346656037UL;
        foreach (var c in english)
        {
            h ^= c;
            h *= 1099511628211UL;
        }
        return (h & 0xFFFFFFFFFFUL).ToString("x10");
    }

    /// <summary>Reads a { "hash": "text" } file; entries that are not strings are skipped.</summary>
    public static Dictionary<string, string> Parse(string json)
    {
        var table = new Dictionary<string, string>();
        using var doc = JsonDocument.Parse(json, new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true });
        foreach (var p in doc.RootElement.EnumerateObject())
            if (p.Value.ValueKind == JsonValueKind.String) table[p.Name] = p.Value.GetString();
        return table;
    }
}
