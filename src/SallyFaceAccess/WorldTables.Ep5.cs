namespace SallyFaceAccess;

/// <summary>Episode 5 names.</summary>
internal static partial class WorldTables
{
    static partial void Episode5()
    {
        // ---- Areas
        Areas["05_cemetery"] = "Neveroak Cemetery";
        Areas["05_house1"] = "Sal and Todd's house";
        Areas["05_house1_neilsRoom"] = "Neil's room";
        Areas["05_house1_basement"] = "the basement";
        Areas["05_house1_Backyard"] = "the backyard";
        Areas["05_house1_Shed"] = "the shed";
        Areas["05_treeHouse"] = "the treehouse";
        Areas["05_temple1"] = "the cult temple";
        Areas["05_temple2"] = "the cult temple";
        Areas["05_templeOS"] = "the cult temple, the other side";
        Areas["05_temple2_center"] = "the temple's heart";
        Areas["05_templeOS_center"] = "the temple's heart, the other side";
        Areas["05_void"] = "the void";
        Areas["05_void2"] = "the void";
        Areas["05_void_House"] = "a house in the void";
        Areas["05_alt_House"] = "the house, but wrong";
        Areas["05_pyramids"] = "the pyramids";
        Areas["05_forest"] = "the forest";
        Areas["05_caveEnt"] = "the cave mouth";
        Areas["05_cave"] = "the cave";
        Areas["05_lab"] = "the office in the temple";
        Hotspots["05_lab/@-17"] = "bookcase";        // nameless; its script is "voidDoor Use"
        Hotspots["05_lab/@-5"] = "tank of liquid";   // nameless; its script is "tube"
        Areas["05_32_field"] = "the arena";
        Areas["05_32_boss"] = "the arena";
        Areas["05_endless_2_clay"] = "the clay world";
        Areas["05_endless_3_B"] = "the Endless One";
        Areas["05_endless_4_3D"] = "the Endless One";
        Areas["05_endless_5_Toon"] = "the cartoon world";
        Areas["05_endless_6_void"] = "the void";
        Areas["05_END"] = "the ruins of the church";

        // ---- Rooms
        Rooms["05_house1/houseCam"] = "the downstairs";
        Rooms["05_house1/bathCam"] = "bathroom";
        Rooms["05_alt_House/houseCam"] = "the downstairs";
        Rooms["05_alt_House/salsCam"] = "Sal's room";
        Rooms["05_alt_House/bathCam"] = "bathroom";

        // ---- Hotspots
        Hotspots["05_alt_House/gear 1 hs"] = "right hex dial";
        Hotspots["05_alt_House/gear 2 hs"] = "middle hex dial, turns the right one too";
        Hotspots["05_alt_House/gear 3 hs"] = "left hex dial, turns the middle one too";
        // The cave's two halls share one scene with free-following cameras; the tunnels between
        // them sit at about x -14 (first hall's left end) and -45 (back hall's right end).
        Rooms["05_cave/gameCam1"] = "the first hall";
        Rooms["05_cave/gameCam2"] = "the back hall";
        RoomBounds["05_cave/gameCam1"] = (-30f, 100f);
        RoomBounds["05_cave/gameCam2"] = (-160f, -30f);
        Hotspots["05_cave/tunnel1"] = "tunnel to the back hall";
        Hotspots["05_cave/tunnel1 (1)"] = "tunnel to the first hall";
        Hotspots["05_cave/turn (2)"] = "left hex dial, turns the middle one too";
        Hotspots["05_cave/turn (1)"] = "middle hex dial, turns the right one too";
        Hotspots["05_cave/turn"] = "right hex dial, turns the left one too";
        Hotspots["05_cave/buttonHS"] = "button in the rock";
        Hotspots["05_forest/hexHS 1"] = "middle terminal";
        Hotspots["05_forest/hexHS 2_1"] = "left hex dial, turns the right one too";
        Hotspots["05_forest/hexHS 2_2"] = "middle hex dial, turns the left one too";
        Hotspots["05_forest/hexHS 2_3"] = "right hex dial, turns the middle one too";
        Hotspots["05_forest/blankHS 1"] = "tile terminal";
        Hotspots["05_forest/blankHS 2"] = "tile terminal";
        Hotspots["05_cemetery/@-34"] = "black hexagon, on a freshly dug grave";
        Hotspots["05_cemetery/SAL"] = "Sal Fisher's grave";
        Hotspots["05_house1/photoHS"] = "photo";
        Hotspots["05_house1/letter"] = "letter";
        Hotspots["05_house1_neilsRoom/neilHS"] = "Neil";
        Hotspots["05_house1_neilsRoom/toddsNotes"] = "Todd's notes";
        Hotspots["05_house1_basement/Maple"] = "Maple";
        Hotspots["05_house1_Shed/box"] = "package";
        Hotspots["05_house1_Shed/notes"] = "Todd's notes";
        Hotspots["05_cave/talaHS"] = "Tala";
        Hotspots["05_void_House/JimHS"] = "Jim";
        Hotspots["05_temple1/jarHS_w"] = "place for the C-4";
    }
}
