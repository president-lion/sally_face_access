using System.Text.RegularExpressions;

namespace SallyFaceAccess;

/// <summary>
/// Turns display text into something worth speaking: strips rich-text tags and
/// Adventure Creator inline tokens, and tidies whitespace.
/// Pure logic - no Unity types - so it is unit tested.
/// </summary>
internal static class TextClean
{
    // Unity rich text / TextMeshPro tags: <b>, </color>, <size=20>, <sprite name="x">.
    private static readonly Regex Tags = new(@"</?[a-zA-Z][a-zA-Z0-9\-]*(\s*=\s*[^>]*|\s+[^>]*)?>", RegexOptions.Compiled);

    // Adventure Creator speech tokens: [wait], [wait:2], [hold], [expression:Happy], [continue].
    private static readonly Regex AcTokens = new(@"\[(wait|hold|expression|continue|token|var|localvar|param|speaker|line)(:[^\]]*)?\]",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex Spaces = new(@"[ \t ]+", RegexOptions.Compiled);
    private static readonly Regex Newlines = new(@"\s*\n\s*", RegexOptions.Compiled);

    public static string Clean(string text)
    {
        if (string.IsNullOrEmpty(text)) return "";
        var s = text.Replace("\r", "");
        s = Tags.Replace(s, "");
        s = AcTokens.Replace(s, "");
        s = Spaces.Replace(s, " ");
        s = Newlines.Replace(s, " ");
        return s.Trim();
    }
}
