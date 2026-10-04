namespace SallyFaceAccess;

/// <summary>
/// Builds the spoken form of a speech line: "Speaker: text", or just the text for
/// narration or when speaker names are turned off.
/// Pure logic - no Unity types - so it is unit tested.
/// </summary>
internal static class DialogueFormat
{
    public static string Line(string speaker, string text, bool includeSpeaker)
    {
        var t = TextClean.Clean(text);
        if (t.Length == 0) return "";
        var s = TextClean.Clean(speaker);
        return includeSpeaker && s.Length > 0 ? $"{s}: {t}" : t;
    }
}
