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
        // Episode 1
        ["lvl00/lightsOn"] = "light switch",
        ["F4r2/@3"] = "quarter on the floor",
        ["F4r2/groceryBagsHS"] = "grocery bags",
        ["F4r2/sleepyDad"] = "dad, asleep",
        ["F4/copSpot"] = "police officer",
        ["F5/@-24"] = "hole in the wall",
        ["F5r4/hoboHS"] = "ragged man",
        ["F5r4/chugHs"] = "Chug",
        ["F5r4/laxativeHS"] = "old laxative",
        ["F1/exitDoor"] = "building entrance",
        ["F1/detectiveHS"] = "detective",
        ["Fb/door"] = "apartment 1B",
        ["Fb/@13"] = "lost and found box",
        ["Fbr1/scannerHS"] = "police scanner",
        ["F4r3/outlineHS"] = "chalk outline",
        ["F4r3/brokenStuffHS"] = "broken furniture",
        ["F4r3/statueHS"] = "statue",
        ["F4r3/blockageHS"] = "blocked doorway",
        ["End/vanHS"] = "coroner's van",
        ["End/charleyHS"] = "Charley",
        ["F2r4/charleyHS"] = "Charley",
        ["F2r4/Collection 1"] = "pony collection",
        ["F2r4/Collection 1 (1)"] = "pony collection",
        ["F3/lisaHS"] = "Lisa",
        ["F3/chugHS"] = "Chug",
        ["F2/door 204 - locked"] = "apartment 204",
        ["F2/door 204 - open"] = "apartment 204",
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

    // Rooms within an area, keyed "scene/camera name".
    private static readonly Dictionary<string, string> Rooms = new()
    {
        ["F4r2/salsRoomCam"] = "Sal's room",
        ["F4r2/dadsRoomCam"] = "dad's room",
        ["F4r2/bathroomCam"] = "bathroom",
        ["F4r2/NavCam 1"] = "living room",
    };

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

    public static string Area(string scene) =>
        Areas.TryGetValue(scene, out var n) ? n : WorldText.SceneName(scene);
}
