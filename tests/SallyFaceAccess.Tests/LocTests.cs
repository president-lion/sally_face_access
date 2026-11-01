using SallyFaceAccess;
using Xunit;

// Loc holds one language for the whole process, so tests must not switch it under each other.
[assembly: CollectionBehavior(DisableTestParallelization = true)]

public class LocTests
{
    [Fact]
    public void TranslatesByHashOfTheEnglish()
    {
        Loc.Use("de", new Dictionary<string, string> { [Loc.Hash("back")] = "zurück" });
        try
        {
            Assert.Equal("zurück", Loc.T("back"));
            Assert.Equal("zurück", Strings.Back);
        }
        finally { Loc.Use("en", null); }
    }

    [Fact]
    public void FallsBackToEnglish()
    {
        Loc.Use("de", new Dictionary<string, string> { [Loc.Hash("back")] = "" });
        try
        {
            Assert.Equal("back", Loc.T("back"));
            Assert.Equal("close", Loc.T("close"));
        }
        finally { Loc.Use("en", null); }
    }

    [Fact]
    public void FormatStringsAreTranslatedBeforeFormatting()
    {
        Loc.Use("fr", new Dictionary<string, string> { [Loc.Hash("{0} of {1}")] = "{0} sur {1}" });
        try { Assert.Equal("2 sur 5", string.Format(Strings.Position, 2, 5)); }
        finally { Loc.Use("en", null); }
    }

    [Fact]
    public void HashIsTenHexDigits() => Assert.Matches("^[0-9a-f]{10}$", Loc.Hash("door to {0}"));

    [Fact]
    public void GameLanguageNamesMapToCodes()
    {
        Assert.Equal("ru", Loc.Code("Русский"));
        Assert.Equal("zh-Hant", Loc.Code("繁體中文 "));
        Assert.Equal("pt-BR", Loc.Code("Português (BR)"));
        Assert.Null(Loc.Code("Klingon"));
    }

    [Fact]
    public void ParsesTranslationFiles() =>
        Assert.Equal("weiter", Loc.Parse("{ \"abc\": \"weiter\", \"n\": 3 }")["abc"]);
}
