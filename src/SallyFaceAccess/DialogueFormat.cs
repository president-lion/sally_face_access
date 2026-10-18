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

    // Lines the game writes backwards (the voice in the white room), in every language.
    private static readonly HashSet<int> Mirrored = new() { 1820, 1822, 1824, 2509, 2510 };

    public static bool IsMirrored(int lineID) => Mirrored.Contains(lineID);

    /// <summary>Mirror-written text the right way round, a space after each full stop.</summary>
    public static string Unmirror(string text)
    {
        var chars = (text ?? "").ToCharArray();
        Array.Reverse(chars);
        return System.Text.RegularExpressions.Regex.Replace(new string(chars), @"\.(?=\S)", ". ").Trim();
    }
}
