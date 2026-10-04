namespace SallyFaceAccess;

/// <summary>
/// Hand-written names for things the game only labels with a verb, and for areas whose
/// scene name does not generate a good one. Mod wording, English for now.
/// Keys: hotspots "scene/GameObject name"; areas "scene".
/// </summary>
internal static class WorldTables
{
    private static readonly Dictionary<string, string> Hotspots = new()
    {
    };

    private static readonly Dictionary<string, string> Areas = new()
    {
        // Episode 1: the dream Sal tells Dr. Enon about, then the apartment.
        ["lvl00"] = "dream, hospital room",
        ["lvl01"] = "dream, hospital hallway",
        ["lvl02"] = "dream, graveyard",
        ["lvl03"] = "dream, locked door",
        ["lvl04"] = "dream, mom and dad",
        ["t01"] = "Dr. Enon's office",
        ["F4r2"] = "apartment 402",
    };

    public static string Hotspot(string scene, string objectName) =>
        Hotspots.TryGetValue($"{scene}/{objectName}", out var n) ? n : null;

    public static string Area(string scene) =>
        Areas.TryGetValue(scene, out var n) ? n : WorldText.SceneName(scene);
}
