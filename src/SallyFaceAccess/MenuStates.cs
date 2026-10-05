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

    // Episode 5's hexagon tiles, from their art (hex_black ... hex_white): the colour and the dots on it.
    internal static readonly (string Colour, string Text)[] Hexagons =
    {
        ("black", "a black hexagon with five green dots"),
        ("blue", "a blue hexagon with one white dot"),
        ("white", "a white hexagon with three blue dots"),
        ("purple", "a purple hexagon with two black dots"),
        ("green", "a green hexagon with four red dots"),
        ("red", "a red hexagon with six purple dots"),
    };

    // The hex keypad (Episode 5) appends each colour pressed as a digit to variable 17
    // (haxPad_black ... haxPad_white: black 1, blue 2, purple 3, green 4, red 5, white 6).
    private static readonly string[] PadColours = { "", "black", "blue", "purple", "green", "red", "white" };

    private static readonly string[] Corners = { "", "top", "bottom left", "bottom right" };
    private static readonly string[] PyramidNames = { "", "north pyramid", "west pyramid", "east pyramid" };

    /// <summary>The pyramid puzzle's state: "north pyramid: top, bottom right" or a bare name.</summary>
    public static string PyramidEntry(int pyramid, int code)
    {
        if (pyramid is < 1 or > 3) return Strings.ChoosePyramid;
        if (pyramid == 3) return PyramidNames[3];
        var corners = new List<string>();
        for (var c = code; c > 0; c /= 10) corners.Insert(0, c % 10 is var d && d is >= 1 and <= 3 ? Corners[d] : "?");
        return corners.Count == 0 ? $"{PyramidNames[pyramid]}, {Strings.NothingEntered}" : $"{PyramidNames[pyramid]}: {string.Join(", ", corners)}";
    }

    /// <summary>A number keypad's entry so far: 502 -> "entered: 5 0 2".</summary>
    public static string DigitsEntry(int code) =>
        code <= 0 ? Strings.NothingEntered : string.Format(Strings.Entered, string.Join(" ", code.ToString().ToCharArray()));

    /// <summary>The colours entered on the hex keypad so far: 612 -> "entered: white, black, blue".</summary>
    public static string HexPadEntry(int code)
    {
        var colours = new List<string>();
        for (var c = code; c > 0; c /= 10)
        {
            var d = c % 10;
            colours.Insert(0, d < PadColours.Length ? PadColours[d] : "?");
        }
        return colours.Count == 0 ? Strings.NothingEntered : string.Format(Strings.Entered, string.Join(", ", colours));
    }

    // The bird cage's compass slots, by the number in their pictures' names, read clockwise from north.
    private static readonly (int Slot, string Name)[] CompassSlots =
        { (3, "north"), (4, "east"), (1, "south"), (2, "west"), (5, "centre") };
    private static readonly string[] ToothLetters = { "n", "e", "x", "s", "w" };

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

            // The bird cage compass (Episode 4): five slots, each showing the tooth put in it as a
            // picture "tooth <letter> <slot>".
            case "bird cage":
                if (!visible.ContainsKey("compass")) return null;
                var slots = new List<string>();
                foreach (var (slot, name) in CompassSlots)
                {
                    var tooth = ToothLetters.FirstOrDefault(t => visible.ContainsKey($"tooth {t} {slot}"));
                    slots.Add($"{name}: {(tooth == null ? Strings.Empty : string.Format(Strings.Tooth, tooth.ToUpperInvariant()))}");
                }
                return string.Join(". ", slots);

            // The hexagons hidden through Episode 5: a close-up of the one just found.
            case "HexCU":
                foreach (var (colour, text) in Hexagons)
                    if (visible.ContainsKey(colour)) return text;
                return null;

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
