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
}
