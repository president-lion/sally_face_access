using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using SallyFaceAccess;
using Xunit;

/// <summary>
/// lang/en.json is the list of every English text the mod speaks, by its Loc hash, read out
/// of the wording tables. Set SFA_WRITE_CATALOGUE=1 to rewrite it after changing wording.
/// The translation files are checked against it.
/// </summary>
public class CatalogueTests
{
    // Tables that hold wording; fields of theirs that are not spoken are skipped by name.
    private static readonly Type[] Tables =
        { typeof(Strings), typeof(Descriptions), typeof(WorldTables), typeof(Labels), typeof(MenuStates), typeof(PuzzleText), typeof(TriggerNames) };
    private static readonly HashSet<string> NotSpoken = new() { "AfterLineTexts", "ToothLetters", "GuitarKeys", "RoomBounds" };

    private static string LangDir
    {
        get
        {
            var d = new DirectoryInfo(AppContext.BaseDirectory);
            while (d != null && !Directory.Exists(Path.Combine(d.FullName, "lang"))) d = d.Parent;
            return d == null ? null : Path.Combine(d.FullName, "lang");
        }
    }

    private static List<string> Texts()
    {
        Loc.Use("en", null);
        var found = new List<string>();
        var seen = new HashSet<string>();
        void Add(object o)
        {
            switch (o)
            {
                case string s:
                    if (Spoken(s) && seen.Add(s)) found.Add(s);
                    break;
                case IDictionary d:
                    foreach (var v in d.Values) Add(v);
                    break;
                case ITuple t:
                    for (var i = 0; i < t.Length; i++) Add(t[i]);
                    break;
                case IEnumerable e:
                    foreach (var v in e) Add(v);
                    break;
            }
        }
        foreach (var type in Tables)
        {
            const BindingFlags all = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            foreach (var p in type.GetProperties(all).Where(p => p.PropertyType == typeof(string) && p.GetIndexParameters().Length == 0))
                Add(p.GetValue(null));
            foreach (var f in type.GetFields(all).Where(f => !NotSpoken.Contains(f.Name)))
                Add(f.IsLiteral ? f.GetRawConstantValue() : f.GetValue(null));
        }
        return found;
    }

    // Codes ("MC", "n") and pure {reference} templates have nothing to translate.
    private static bool Spoken(string s) =>
        s.Trim().Length > 2 && Regex.IsMatch(Regex.Replace(s, @"\{[^{}]*\}", ""), @"\p{L}{2}");

    private static string Render(IEnumerable<string> texts)
    {
        var opts = new JsonWriterOptions { Indented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        using var ms = new MemoryStream();
        using (var w = new Utf8JsonWriter(ms, opts))
        {
            w.WriteStartObject();
            foreach (var t in texts) w.WriteString(Loc.Hash(t), t);
            w.WriteEndObject();
        }
        return System.Text.Encoding.UTF8.GetString(ms.ToArray()).Replace("\r\n", "\n") + "\n";
    }

    [Fact]
    public void CatalogueIsCurrent()
    {
        var path = Path.Combine(LangDir, "en.json");
        var now = Render(Texts());
        if (Environment.GetEnvironmentVariable("SFA_WRITE_CATALOGUE") == "1") File.WriteAllText(path, now);
        Assert.Equal(now, File.ReadAllText(path).Replace("\r\n", "\n"));
    }

    [Fact]
    public void NoTwoTextsShareAHash()
    {
        var texts = Texts();
        Assert.Equal(texts.Count, texts.Select(Loc.Hash).Distinct().Count());
    }

    [Fact]
    public void CatalogueHasTheDescriptions()
    {
        var texts = Texts();
        Assert.Contains(Strings.Backwards, texts);
        Assert.Contains(texts, t => t.StartsWith("The first floor hallway."));
        Assert.Contains("north pyramid", texts);
        Assert.Contains("barrier spell", texts);
    }

    private static readonly Regex Placeholder = new(@"\{[^{}]*\}");

    private static string Placeholders(string s) =>
        string.Join(" ", Placeholder.Matches(s).Select(m => m.Value).OrderBy(x => x, StringComparer.Ordinal));

    [Fact]
    public void TranslationsKeepTheirPlaceholders()
    {
        var english = Loc.Parse(File.ReadAllText(Path.Combine(LangDir, "en.json")));
        var problems = new List<string>();
        foreach (var file in Directory.GetFiles(LangDir, "*.json").Where(f => Path.GetFileName(f) != "en.json"))
            foreach (var (hash, text) in Loc.Parse(File.ReadAllText(file)))
            {
                if (!english.TryGetValue(hash, out var en)) problems.Add($"{Path.GetFileName(file)} {hash}: not in en.json");
                else if (Placeholders(en) != Placeholders(text)) problems.Add($"{Path.GetFileName(file)} {hash}: '{text}' vs '{en}'");
                // A leading "!" makes a visual interrupt speech (ActionHooks), so it must survive translation.
                else if (en.StartsWith('!') != text.StartsWith('!')) problems.Add($"{Path.GetFileName(file)} {hash}: interrupt marker");
            }
        Assert.True(problems.Count == 0, string.Join("\n", problems));
    }

    [Fact]
    public void ARealTranslationReachesTheTables()
    {
        Loc.Use("ru", Loc.Parse(File.ReadAllText(Path.Combine(LangDir, "ru.json"))));
        try
        {
            Assert.StartsWith("Внутри лифта", Descriptions.Area("elevator", "NavCam 1"));
            Assert.Equal("пакеты с продуктами", WorldTables.Hotspot("F4r2", "groceryBagsHS", 0));
            Assert.Equal("назад", Strings.Back);
        }
        finally { Loc.Use("en", null); }
    }

    [Fact]
    public void EveryGameLanguageHasAFile()
    {
        foreach (var code in Loc.AllCodes.Where(c => c != "en"))
            Assert.True(File.Exists(Path.Combine(LangDir, code + ".json")), code);
    }
}
