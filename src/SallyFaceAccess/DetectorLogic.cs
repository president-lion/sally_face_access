namespace SallyFaceAccess;

/// <summary>
/// The Super Gear Boy detector's state lives in global variable 6: 555 while it is held up,
/// 777 while its screen glows (the player is in a hot spot's zone), anything else when it is
/// put away. Hot spot zones are triggers whose script starts by checking for 555.
/// Pure logic - no Unity types - so it is unit tested.
/// </summary>
internal static class DetectorLogic
{
    public const int VariableId = 6;
    public const int Held = 555;
    public const int Glowing = 777;

    /// <summary>What to say when the variable changes from <paramref name="from"/> to <paramref name="to"/>, or null.</summary>
    public static string Transition(int from, int to)
    {
        if (from == to) return null;
        var wasOut = from is Held or Glowing;
        return to switch
        {
            Glowing => Strings.DetectorGlow,
            Held when from == Glowing => Strings.DetectorFade,
            Held => Strings.DetectorUp,
            _ when wasOut => Strings.DetectorDown,
            _ => null,
        };
    }
}
