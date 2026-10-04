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
    };

    public static string Hotspot(string scene, string objectName) =>
        Hotspots.TryGetValue($"{scene}/{objectName}", out var n) ? n : null;

    public static string Area(string scene) =>
        Areas.TryGetValue(scene, out var n) ? n : WorldText.SceneName(scene);
}
