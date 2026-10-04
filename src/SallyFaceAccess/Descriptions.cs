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
        : Areas.TryGetValue(scene, out d) ? d : null;

    public static string Cutscene(string scene, string list) =>
        Cutscenes.TryGetValue($"{scene}/{list}", out var d) ? d : null;

    public static string Line(int lineID) => Lines.TryGetValue(lineID, out var d) ? d : null;

    public static string AfterLine(int lineID) => AfterLines.TryGetValue(lineID, out var d) ? d : null;
}
