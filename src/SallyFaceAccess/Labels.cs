using System.Text.RegularExpressions;

namespace SallyFaceAccess;

/// <summary>
/// Names for menu buttons that have no text of their own - image buttons, whose only
/// name is an internal one like "b_ep1_x".
///
/// The table is keyed "Menu title/Element title". A value is either plain wording (the
/// mod's own, English for now) or a template whose {references} pull in the text of other
/// elements in the same menu, so those parts stay in the game's language.
/// Anything not in the table falls back to <see cref="Humanize"/>.
/// Pure logic - no Unity types - so it is unit tested.
/// </summary>
internal static class Labels
{
    private static readonly Dictionary<string, string> Table = new()
    {
        // Episode select: each card is a button beside its title, subtitle and progress labels.
        ["Episode Select 2/ep1"] = "{l_ep1}, {l_ep1_sn}, {l_ep1%}",
        ["Episode Select 2/ep2"] = "{l_ep2}, {l_ep2_sn}, {l_ep2%}",
        ["Episode Select 2/ep3"] = "{l_ep3}, {l_ep3_sn}, {l_ep3%}",
        ["Episode Select 2/ep4"] = "{l_ep4}, {l_ep4_sn}, {l_ep4%}",
        ["Episode Select 2/ep5"] = "{l_ep5}, {l_ep5_sn}, {l_ep5%}",
        ["Episode Select 2/b_ep1_x"] = Strings.Back,
        ["Episode Select 2/b_ep2_x"] = Strings.Back,
        ["Episode Select 2/b_ep3_x"] = Strings.Back,
        ["Episode Select 2/b_ep4_x"] = Strings.Back,
        ["Episode Select 2/b_ep5_x"] = Strings.Back,
        ["Episode Select 2/back"] = Strings.Back,
        ["Episodes DLC/Button"] = Strings.Back,

        ["Language/Back"] = Strings.Back,
        ["Controller Option/Back"] = Strings.Back,
        ["Controls/Button"] = Strings.Back,
        ["Controls2/Button"] = Strings.Back,
        ["Controls3/Button"] = Strings.Back,
        ["Controls - Start/Button"] = Strings.Back,
        ["Controls - Start 2/Button"] = Strings.Back,
        ["Controls - Start 3/Button"] = Strings.Back,

        ["Stereo/Left Arrow"] = Strings.PreviousStation,
        ["Stereo/Right Arrow"] = Strings.NextStation,
        ["Stereo 2/Left Arrow"] = Strings.PreviousStation,
        ["Stereo 2/Right Arrow"] = Strings.NextStation,
        ["Stereo OS/Left Arrow"] = Strings.PreviousStation,
        ["Stereo OS/Right Arrow"] = Strings.NextStation,

        // The treehouse chest (Episode 2) and the school lockers (Episode 3): picture buttons.
        ["chestMenu/1"] = "magazines",
        ["chestMenu/2"] = "leather jacket",
        ["chestMenu/3"] = "small box",
        ["chestMenu/X"] = Strings.Close,
        ["Larry's Locker/sal"] = "Sal's painting",
        ["Larry's Locker/ash"] = "Ash's drawing",
        ["Larry's Locker/art"] = "art supplies",
        ["Ash's Locker/dudes"] = "stuffed dolls",
        ["Ash's Locker/art"] = "art supplies",
        ["Ash's Locker/sal"] = "photo of Sal",
        ["Todd's Locker/stuff"] = "invention supplies",
        ["Todd's Locker/potato"] = "potato light",
        ["locker message/Button"] = Strings.Continue,

        // Mrs. Packerton's clock (Episode 3): each button turns the digit it sits under.
        ["Packerton's Clock/button 1"] = "digit 1, {1}",
        ["Packerton's Clock/button 2"] = "digit 2, {2}",
        ["Packerton's Clock/button 3"] = "digit 3, {3}",
        ["Packerton's Clock/button 4"] = "digit 4, {4}",
        ["Packerton's Clock/close"] = Strings.Close,

        // Sal's cell phone: the buttons' words are labels beside them, changing with the conversation.
        ["Cell Phone/Button 1"] = "{Reply}",
        ["Cell Phone/Button 2"] = "{Exit}",
        // Touch-screen twin of button 1; button 2's twin only appears on PC after Larry's last
        // text, as the phone icon Sal uses to call him.
        ["Cell Phone/Button 1 (Touch)"] = "{Reply}",
        ["Cell Phone/Button 2 (Touch)"] = "call Larry",

        // The newspaper the journalist shows Sal (Episode 3 ending).
        ["Newspaper/Button"] = Strings.Close,
        ["Newspaper/Button1"] = Strings.Continue,

        // Door 4's lock in the Dark Place: one button turns each dial.
        ["Dark Place Lock/Button 1"] = "turn dial 1",
        ["Dark Place Lock/Button 2"] = "turn dial 2",
        ["Dark Place Lock/Button 3"] = "turn dial 3",
        ["Dark Place Lock/Close"] = Strings.Close,

        // The bird cage compass (Episode 4): pick a tooth from the tray, then the slot to put it in.
        ["bird cage/N"] = "north slot",
        ["bird cage/E"] = "east slot",
        ["bird cage/S"] = "south slot",
        ["bird cage/W"] = "west slot",
        ["bird cage/X"] = "centre slot",
        ["bird cage/tooth n"] = "tooth N",
        ["bird cage/tooth e"] = "tooth E",
        ["bird cage/tooth x"] = "tooth X, burned",
        ["bird cage/tooth s"] = "tooth S",
        ["bird cage/tooth w"] = "tooth W",

        // Episode 5's hexagons: the close-up, and the keypad their colours unlock.
        ["HexCU/close"] = Strings.Close,
        ["HexPad/O"] = "enter",
        ["HexPad/X"] = Strings.Close,
        ["HexPad/black"] = "black hexagon",
        ["HexPad/blue"] = "blue hexagon",
        ["HexPad/white"] = "white hexagon",
        ["HexPad/purple"] = "purple hexagon",
        ["HexPad/green"] = "green hexagon",
        ["HexPad/red"] = "red hexagon",

        ["Toon Code/E"] = "enter",
        ["Clay Console/Slide 1"] = "slider 1",
        ["Clay Console/Slide 2"] = "slider 2",
        ["Clay Console/Slide 3"] = "slider 3",
        ["Clay Console/X"] = Strings.Close,

        // The three pyramids (Episode 5): pick one, press its corners, then place.
        ["Pyramids/top"] = "north pyramid",
        ["Pyramids/left"] = "west pyramid",
        ["Pyramids/right"] = "east pyramid",
        ["Pyramids/b_top"] = "top corner",
        ["Pyramids/b_left"] = "bottom left corner",
        ["Pyramids/b_right"] = "bottom right corner",

        ["Achievements Nav/close button"] = Strings.Close,
        ["Achievements Nav/close button (MM)"] = Strings.Close,
        ["Achievements Nav/r 1"] = Strings.NextPage,
        ["Achievements Nav/r 2"] = Strings.NextPage,
        ["Achievements Nav/r 3"] = Strings.NextPage,
        ["Achievements Nav/r 4"] = Strings.NextPage,
        ["Achievements Nav/l 2"] = Strings.PreviousPage,
        ["Achievements Nav/l 3"] = Strings.PreviousPage,
        ["Achievements Nav/l 4"] = Strings.PreviousPage,
        ["Achievements Nav/l 5"] = Strings.PreviousPage,

        ["Item Close Up/Close Button"] = Strings.Close,
    };

    public static bool TryGet(string menuTitle, string elementTitle, out string template) =>
        Table.TryGetValue($"{menuTitle}/{elementTitle}", out template);

    private static readonly Regex Reference = new(@"\{([^{}]+)\}", RegexOptions.Compiled);

    /// <summary>
    /// Fills {references} via <paramref name="lookup"/> (element title -> its text). Parts
    /// whose reference is missing or empty are dropped along with their ", " separator.
    /// </summary>
    public static string Expand(string template, Func<string, string> lookup)
    {
        if (!template.Contains('{')) return template;
        var parts = template.Split(", ")
            .Select(p => Reference.Replace(p, m => lookup(m.Groups[1].Value) ?? "").Trim())
            .Where(p => p.Length > 0);
        return string.Join(", ", parts);
    }

    private static readonly Regex Parens = new(@"\([^)]*\)", RegexOptions.Compiled);
    private static readonly Regex Separators = new(@"[_\-\s]+", RegexOptions.Compiled);

    /// <summary>A readable guess from an internal element name, for buttons not in the table.</summary>
    private static readonly Regex Placeholder = new(@"^button\s*\d*$", RegexOptions.IgnoreCase);

    /// <summary>A button title that says nothing ("Button", "Button1"): name it from what it does instead.</summary>
    public static bool IsPlaceholder(string title) => title != null && Placeholder.IsMatch(title.Trim());

    public static string Humanize(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) return "";
        var s = Parens.Replace(title, " ");
        s = Regex.Replace(s, @"^b[_\-]", "", RegexOptions.IgnoreCase);
        var words = Separators.Split(s).Where(w => w.Length > 0).ToList();
        if (words.Any(w => w.Equals("x", StringComparison.OrdinalIgnoreCase)
                        || w.Equals("close", StringComparison.OrdinalIgnoreCase)))
            return Strings.Close;
        return string.Join(" ", words);
    }
}
