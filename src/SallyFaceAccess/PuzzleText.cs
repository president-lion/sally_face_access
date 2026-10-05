namespace SallyFaceAccess;

/// <summary>
/// Wording for puzzles whose state the game only shows as pictures, read from the game's
/// own variables. Pure logic - no Unity types - so it is unit tested.
/// </summary>
internal static class PuzzleText
{
    /// <summary>
    /// The cult temple's floor seal (Episode 3): two rings turned by the levers, four
    /// positions each, solved when both are at 1. The right lever turns its ring; the left
    /// lever turns both.
    /// </summary>
    public static string Seal(int right, int left) =>
        right == 1 && left == 1 ? Strings.SealSolved : string.Format(Strings.SealState, right, left);

    /// <summary>
    /// The temple's floor switch rooms (Episode 3): five switches, each up or down; stepping
    /// on one flips others too, and the gate opens when all are down.
    /// </summary>
    public static string Switches(IReadOnlyList<bool> down)
    {
        var d = Enumerable.Range(0, down.Count).Where(i => down[i]).Select(i => (i + 1).ToString()).ToList();
        if (d.Count == down.Count) return Strings.SwitchesAll;
        if (d.Count == 0) return Strings.SwitchesNone;
        return string.Format(Strings.SwitchesDown, string.Join(", ", d));
    }

    // Todd's guitar (Episode 4): a direction picks a group of three notes, J/K/L strums one.
    private static readonly string[] GuitarGroups = { Strings.Up, Strings.Right, Strings.Down, Strings.Left };
    private static readonly string[] GuitarKeys = { "J", "K", "L" };

    /// <summary>How to play guitar note 1-12: "right, K" for note 5.</summary>
    public static string GuitarNote(int note) => $"{GuitarGroups[(note - 1) / 3]}, {GuitarKeys[(note - 1) % 3]}";

    private static readonly System.Text.RegularExpressions.Regex Rune = new(@"^anim:rune_(\d+)$");

    /// <summary>
    /// The note a barrier rune asks for, from its animation ("anim:rune_7" -> 7), or 0 for
    /// anything else (a rune fading, the timer).
    /// </summary>
    public static int RuneNote(string key)
    {
        var m = Rune.Match(key ?? "");
        return m.Success && int.TryParse(m.Groups[1].Value, out var n) && n is >= 1 and <= 12 ? n : 0;
    }
}
