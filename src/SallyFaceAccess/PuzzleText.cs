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
}
