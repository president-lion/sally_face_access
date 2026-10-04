namespace SallyFaceAccess;

/// <summary>
/// Menus that show their current choice as a picture or a lone label rather than a selected
/// button: the stereo's station (one frequency label visible at a time) and Azaria's game
/// book (a two-page spread of game pictures plus a marker on the chosen side). The state is
/// worked out from which elements are visible, and spoken whenever it changes.
/// Pure logic - no Unity types - so it is unit tested.
/// </summary>
internal static class MenuStates
{
    // Station label element -> genre; the label's own text is the frequency.
    private static readonly (string Element, string Genre)[] Stations =
    {
        ("classical", "classical"), ("hip", "hip hop"), ("pop", "pop"),
        ("classic rock", "classic rock"), ("metal", "metal"), ("country", "country"),
    };

    // Each spread shows two games; the names are only in the pictures (English artwork).
    private static readonly (string Left, string Right, string LeftName, string RightName)[] Spreads =
    {
        ("MC", "C", "Musical Chairs", "Charades"),
        ("DDM", "DB", "Duck, Duck, Moose!", "Dodge Ball"),
        ("T", "PP", "Telephone", "Ping Pong"),
    };

    /// <summary>
    /// The menu's current choice, or null for menus without one.
    /// <paramref name="visible"/> maps each visible element's title to its text ("" for pictures).
    /// </summary>
    public static string State(string menu, IReadOnlyDictionary<string, string> visible)
    {
        switch (menu)
        {
            case "Stereo":
            case "Stereo 2":
            case "Stereo OS":
                foreach (var (element, genre) in Stations)
                    if (visible.TryGetValue(element, out var freq))
                    {
                        var s = $"{freq}, {genre}";
                        return visible.ContainsKey("static") ? $"{s}, {Strings.Static}" : s;
                    }
                return null;

            case "Game Book":
                for (var i = 0; i < Spreads.Length; i++)
                {
                    var sp = Spreads[i];
                    if (!visible.ContainsKey(sp.Left)) continue;
                    var right = visible.ContainsKey("SelectR");
                    var n = i * 2 + (right ? 2 : 1);
                    return string.Format(Strings.ItemOfCount, right ? sp.RightName : sp.LeftName, n, Spreads.Length * 2);
                }
                return null;
        }
        return null;
    }
}
