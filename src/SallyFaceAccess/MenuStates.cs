namespace SallyFaceAccess;

/// <summary>
/// Menus that show their current choice as a picture or a lone label rather than a selected
/// button: the stereo's station (one frequency label visible at a time) and Azaria's game
/// book (a two-page spread of game pictures plus a marker on the chosen side), Mrs. Packerton's
/// clock (four digit labels). The state is
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

    // The lock's symbols in dial order, named from the art (dp_lock_0 .. dp_lock_8).
    private static readonly string[] LockSymbols =
        { "dot", "line", "peak", "flag", "double N", "zigzag", "hourglass", "forked diamond", "boxed diamond" };

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

            // Mrs. Packerton's clock: four digit labels, each changed by its own button.
            case "Packerton's Clock":
                if (!visible.TryGetValue("1", out var h1) || !visible.TryGetValue("2", out var h2)
                    || !visible.TryGetValue("3", out var m1) || !visible.TryGetValue("4", out var m2)) return null;
                return $"{h1}{h2}:{m1}{m2}";

            // Door 4's lock in the Dark Place (Episode 4): three dials, each showing one of nine
            // symbols as a picture "dial-symbol" (1-0 .. 3-8).
            case "Dark Place Lock":
                var dials = new List<string>();
                for (var d = 1; d <= 3; d++)
                    for (var k = 0; k < LockSymbols.Length; k++)
                        if (visible.ContainsKey($"{d}-{k}")) { dials.Add(string.Format(Strings.Dial, d, LockSymbols[k])); break; }
                return dials.Count == 0 ? null : string.Join(". ", dials);

            // Sal's cell phone (Episode 4): the sender and message change in place as texts arrive.
            case "Cell Phone":
                if (!visible.TryGetValue("Text", out var msg) || msg.Length == 0) return null;
                return visible.TryGetValue("inbox name", out var from) && from.Length > 0 ? $"{from}: {msg}" : msg;

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
