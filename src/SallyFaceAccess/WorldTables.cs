namespace SallyFaceAccess;

/// <summary>
/// Hand-written names for things the game only labels with a verb, and for areas whose
/// scene name does not generate a good one. Mod wording, English for now.
/// Keys: hotspots "scene/GameObject name" or "scene/@x"; areas "scene"; rooms "scene/camera".
/// One file per episode.
/// </summary>
internal static partial class WorldTables
{
    static WorldTables()
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

    private static readonly Dictionary<string, string> Hotspots = new();

    private static readonly Dictionary<string, string> Areas = new();

    // Rooms within an area, keyed "scene/camera name".
    private static readonly Dictionary<string, string> Rooms = new();

    public static string Room(string scene, string camera) =>
        Rooms.TryGetValue($"{scene}/{camera}", out var n) ? n : null;

    /// <summary>
    /// A hand-written name, keyed "scene/object name", or for objects with no name of their
    /// own "scene/@x" with x rounded to a whole unit.
    /// </summary>
    public static string Hotspot(string scene, string objectName, float x) =>
        Hotspots.TryGetValue($"{scene}/{objectName}", out var n) ? n
        : Hotspots.TryGetValue($"{scene}/@{Math.Round(x):0}", out n) ? n
        : WorldText.HallwayDoor(scene, objectName, x);

    public static string Area(string scene)
    {
        var n = Areas.TryGetValue(scene, out var a) ? a : WorldText.SceneName(scene);
        return scene.StartsWith("04_os_") ? $"{n}, the other side" : n;
    }
}
