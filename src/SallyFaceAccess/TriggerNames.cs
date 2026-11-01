namespace SallyFaceAccess;

/// <summary>
/// Names for Adventure Creator triggers whose purpose is known (see Triggers). Written in
/// English and translated through Loc. Pure data - no Unity types - so the catalogue sees it.
/// </summary>
internal static class TriggerNames
{
    // Triggers whose purpose is known, by scene/name; "" hides scaffolding the generic rules let through.
    public static readonly Dictionary<string, string> Known = new()
    {
        ["03_cult_Main/switchStep"] = "floor switch",
        ["03_cult_Main/doorFalls"] = "gate",
        ["03_cult_Main/doorFalls (1)"] = "gate",
        ["03_cult_Center/AshTrigger"] = "Ash",
        ["04_darkPlace/eyeTriggerR"] = "the eyes",
        ["04_darkPlace/faceTrigger"] = "the faces",
        ["04_darkPlace/herTrigger"] = "her",
        ["04_house/tvOff"] = "",          // turns the TV off when you walk away from it
        ["04_house/tvOff (1)"] = "",
        // The barrier spells (Episode 4): standing here and playing the guitar starts the runes.
        ["04_2_F1/guitarIN"] = "barrier spell",
        ["04_2_F2/guitarIN"] = "barrier spell",
        ["04_2_F3/guitarIN"] = "barrier spell",
        ["04_2_F4/guitarIN"] = "barrier spell",
        ["04_2_roof/guitarIN"] = "barrier spell",
        ["05_void2/f1_trigger"] = "memory of Larry",
        ["05_void2/f2_trigger"] = "memory of Todd",
        ["05_void2/f3_trigger"] = "memory of Ash",
        ["05_cemetery/enemyTest (1)"] = "walk here to go on",  // continues the story once Ash has her power
        ["05_cemetery/salAgain"] = "Sal's grave",
        ["05_32_field/Trigger2D"] = "",        // the arena's edges: stepping off resets it
        ["05_32_field/Trigger2D (1)"] = "",
        ["05_32_field/Trigger2D (2)"] = "",
        ["05_32_field/Trigger2D (3)"] = "",
        ["05_cave/Trigger2D"] = "someone up ahead",   // meeting Tala (once)
        ["05_forest/exitHex"] = "",       // walking out of these steps back from the hex dials
        ["05_forest/exitHex (1)"] = "",
        ["05_forest/exitHex (2)"] = "",
        ["05_forest/exitHex (3)"] = "",
        ["04_2_403/Trigger2D"] = "",      // only plays a stinger as Sal sees Mr. Sanderson; described instead
        ["05_temple1/Trigger W"] = "cultist, don't touch",  // each cultist's capture box, moving with him
        ["04_201/safeTrig"] = "",         // only flags "standing at the safe"; the safe is its own hotspot
        ["04_201/safeTrigOut"] = "",
        ["04_201/safeTrigOut (1)"] = "",
        ["03_cult_L2/switch1"] = "floor switch 1",
        ["03_cult_L2/switch2"] = "floor switch 2",
        ["03_cult_L2/switch3"] = "floor switch 3",
        ["03_cult_L2/switch4"] = "floor switch 4",
        ["03_cult_L2/switch5"] = "floor switch 5",
        ["03_cult_S2/switch1"] = "floor switch 1",
        ["03_cult_S2/switch2"] = "floor switch 2",
        ["03_cult_S2/switch3"] = "floor switch 3",
        ["03_cult_S2/switch4"] = "floor switch 4",
        ["03_cult_S2/switch5"] = "floor switch 5",
    };
}
