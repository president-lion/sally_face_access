namespace SallyFaceAccess;

/// <summary>
/// Hand-written descriptions of what is only shown, never said - written from screenshots
/// of the game (the tour) and its cutscene scripts. They describe what a sighted player sees,
/// clues included, and nothing more. Mod wording, English for now. One file per episode.
///
///   Areas       "scene" or "scene/camera" - spoken on the first visit, and on F6
///   Cutscenes   "scene/ActionList name"   - spoken when that cutscene starts
///   Lines       speech line ID            - spoken just before that line
///   AfterLines  speech line ID            - spoken just after that line
/// </summary>
internal static partial class Descriptions
{
    private static readonly Dictionary<string, string> Areas = new();
    private static readonly Dictionary<string, string> Cutscenes = new();
    private static readonly Dictionary<int, string> Lines = new();
    private static readonly Dictionary<int, string> AfterLines = new();
    // What a menu text's picture shows that the text itself doesn't (notes, close-ups), by line ID.
    private static readonly Dictionary<int, string> MenuPictures = new();

    static Descriptions()
    {
        Episode1();
        Episode2();
        Episode3();
        Episode4();
        Episode5();
    }

    static partial void Episode1();
    static partial void Episode2();
    static partial void Episode3();
    static partial void Episode4();
    static partial void Episode5();

    public static string Area(string scene, string camera) =>
        Areas.TryGetValue($"{scene}/{camera}", out var d) ? d
        : Areas.TryGetValue(scene, out d) ? d
        : Hallways.TryGetValue(WorldText.HallwayFloor(scene), out d) ? Version(scene, d)
        : Version(scene, null);

    // Episode 4's two versions of Addison Apartments: every room exists in both, so each
    // version is described once and prefixed to whatever the room itself has.
    private const string NowVersion = "Addison Apartments under the cult's spell: black ooze drips from the ceiling and pools on the floor. ";
    private const string OtherSide = "The other side: a grey, colourless mirror of the building, walls cracked and peeling, " +
                                     "the edges of the room swallowed in darkness. ";

    private static string Version(string scene, string described)
    {
        if (scene.StartsWith("04_os_")) return OtherSide + (described ?? "");
        if (scene.StartsWith("04_2_") && described != null) return NowVersion + described;
        return described;
    }

    // Every episode's version of a hallway looks the same unless it has its own entry.
    private static readonly Dictionary<int, string> Hallways = new()
    {
        [1] = "The first floor hallway. Doors to 101 and 102 on the left, the elevator, then 103, Mr. Addison's door " +
              "with his nameplate, and 104. Further right, the way out of the building.",
        [2] = "The second floor hallway: green doors to 201 and 202 on the left, the elevator in the middle, " +
              "203 and 204 on the right.",
        [3] = "The third floor hallway: doors to 301 and 302 on the left, the elevator, 303 and 304 on the right.",
        [4] = "The fourth floor hallway: striped wallpaper over wood panelling. Doors to 401 and 402 on the left, " +
              "the elevator, 403 and 404 on the right.",
        [5] = "The fifth floor hallway, half torn apart for renovation: peeling wallpaper, loose planks propped against " +
              "the walls and scattered on the floor, and notices saying the floor is being renovated. " +
              "Grey doors to 501 and 502 on the left, the elevator, 503 and 504 on the right.",
    };

    // "scene/camera" -> what a close-up shows, spoken whenever the view cuts to that camera.
    private static readonly Dictionary<string, string> Shots = new();

    public static string Shot(string scene, string camera) => Shots.TryGetValue($"{scene}/{camera}", out var d) ? d : null;

    public static string Cutscene(string scene, string list) =>
        Cutscenes.TryGetValue($"{scene}/{list}", out var d) || Cutscenes.TryGetValue($"*/{list}", out d) ? d : null;

    public static string Line(int lineID) => Lines.TryGetValue(lineID, out var d) ? d : null;

    public static string MenuPicture(int lineID) => MenuPictures.TryGetValue(lineID, out var d) ? d : null;

    // Menus that are only a picture and a close button, described by menu title when they open.
    private static readonly Dictionary<string, string> MenuPages = new();

    public static string MenuPage(string menuTitle) => MenuPages.TryGetValue(menuTitle, out var d) ? d : null;

    public static string AfterLine(int lineID) => AfterLines.TryGetValue(lineID, out var d) ? d : null;

    // Game text read out after a speech line, by line ID: a note the scene shows Sal reading,
    // kept in the game's language by naming the menu label that holds it.
    private static readonly Dictionary<int, (string Menu, string Element)> AfterLineTexts = new();

    public static (string Menu, string Element)? AfterLineText(int lineID) =>
        AfterLineTexts.TryGetValue(lineID, out var t) ? t : null;

    // Visual events by name, in any scene (see ActionHooks): "anim:clip", "show:object", "hide:object".
    private static readonly Dictionary<string, string> Visuals = new()
    {
        ["anim:ghostGirlUp"] = "The ghost girl rises into view.",
        ["anim:ghostGirlDown"] = "The ghost girl sinks away and is gone.",
        ["anim:dogDeath"] = "The dog slumps over, dead, and fades away.",
        ["show:demon"] = "In the dark, for a split second, a demonic figure.",
        ["hide:demon"] = "It is gone.",
        ["show:bathroomDoorHandPrints"] = "Handprints have appeared on the bathroom door.",
        ["show:endBody"] = "Through the van's back window, a body lies inside.",
        ["anim:charleySleep"] = "Charley slumps in his armchair, fast asleep.",
        // Episode 2: the fragments.
        ["anim:cultAnimAppear"] = "A hooded shadow figure flickers into view, then fades.",
        ["anim:cultAnimAppear1"] = "A hooded shadow figure flickers into view, then fades.",
        ["anim:cultAnimAppear2"] = "A hooded shadow figure flickers into view, then fades.",
        ["anim:cultAnimAppear3"] = "A hooded shadow figure flickers into view, then fades.",
        ["anim:phantomUp"] = "A translucent, flickering figure of a man rises up in front of Sal.",
        ["anim:phantomOut"] = "The figure breaks apart and vanishes with a scream.",
        ["anim:bathGhostUp"] = "A ghostly figure rises out of the bathtub.",
        ["anim:bathGhostDown"] = "The ghost sinks back down and is gone.",
        ["show:b_mirrorRedEyes"] = "In the mirror, a pair of glowing red eyes.",
        // Played on every room load to reset the hidden seal, and the appear lines already say it fades.
        ["anim:cultAnimGone"] = "",
        ["anim:sandersonGhostUp"] = "The ghost of Mrs. Sanderson rises up, pale and translucent.",
        ["anim:sandersonGhostDown"] = "Mrs. Sanderson's ghost sinks away.",
        ["anim:lukeHanged"] = "The ghost of a man hangs from the noose, slowly swaying.",
        ["anim:lukeDown"] = "He drops from the noose and stands, head hanging.",
        ["anim:lukeDemon"] = "His face splits into something demonic, and he lunges at Sal with a shriek.",
        ["anim:stacyUp"] = "The ghost of a woman rises into view.",
        ["anim:stacyDown"] = "She fades away.",
        ["anim:hugAnim"] = "The ghosts hold each other in a long embrace.",
        ["anim:hugGone"] = "They fade from view together.",
    };

    public static string Visual(string key) => Visuals.TryGetValue(key, out var d) ? d : null;
}
