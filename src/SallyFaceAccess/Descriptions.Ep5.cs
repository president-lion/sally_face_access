namespace SallyFaceAccess;

/// <summary>Episode 5: Memories and Dreams - Neveroak Cemetery, the house, the temple, the void, the end.</summary>
internal static partial class Descriptions
{
    static partial void Episode5()
    {
        // ---- Neveroak Cemetery
        Areas["05_cemetery"] = "Neveroak Cemetery at night, under a full moon: an iron arch over the gates spelling Neveroak " +
            "Cemetery, rows of headstones and crosses on dark hills, bare trees against drifting clouds. Among the graves lie " +
            "the Holmes family, Luke, Stacy and Megan; the Sandersons; Robert Silva; Morrison; Chug and Soda Cohen; Larry " +
            "Johnson; Lisa Johnson-Fisher and Henry Fisher; and at the far end, Sal Fisher. At the far left, past the " +
            "Holmes family, a freshly dug grave: a raw heap of loose dark earth, no headstone yet, a plain cross " +
            "standing behind it.";
        Shots["05_cemetery/salCam 1"] = "A headstone: Sal Fisher, 1976 to 2004, son of Henry and Diane Fisher.";
        Shots["05_cemetery/cutCam"] = "Ash looks down at her own gloved hands: one held open, the other flicking out a " +
                                      "small pocket knife.";
        // The pyramids lighting up at Sal's grave: each time his ghost comes back a little more.
        Cutscenes["05_cemetery/P1_active"] = "The pyramid by the grave blazes white, a shaft of light shooting up into " +
            "the sky. Sal's ghost turns towards Ash: a floating skeleton.";
        Cutscenes["05_cemetery/P2_active"] = "The second pyramid blazes white and the ground shakes. Sal's ghost turns: no " +
            "longer bare bones, but a body of raw red muscle, with no skin.";
        Cutscenes["05_cemetery/P3_active"] = "The third pyramid blazes white. Crackling lightning arcs between the three. Sal's " +
            "ghost hangs over the grave with skin now, but pale and patchy, rotting green in places, like a corpse.";
        Visuals["05_cemetery/anim:cut_cut"] = "She draws the blade across her palm. Blood wells up.";
        Visuals["05_cemetery/anim:lightning_strike"] = "Lightning from the three pyramids strikes Ash, lifting her off her feet.";
        Visuals["05_cemetery/anim:ash2_down1"] = "Ash crumples to the ground.";
        Visuals["05_cemetery/show:Ash_blood"] = "Blood spreads out from under her across the grass.";
        Shots["05_cemetery/ashDownCam1"] = "The view sinks slowly down to the grass at the foot of the grave, where Sal's " +
                                           "mask lies, its straps spread out around it.";
        Visuals["05_cemetery/anim:zomb_end"] = "Sal's ghost fades away.";
        Shots["05_cemetery/ashAwakenCam"] = "Ash rises from the grave, her hair matted, her arm burned and scarred down to the " +
                                            "fingers, a pale glow in her eyes.";
        Visuals["anim:ash_awaken"] = "Ash's eyes snap open.";
        Visuals["anim:ash_powerUp"] = "Light crackles down Ash's scarred arm.";
        // Sal's power in Ash: R strikes with it (R-ON runs ashAttack for this player).
        MenuPages["R Icon"] = "Press R to use Sal's power.";
        Visuals["anim:ashAttack"] = "Ash throws out her scarred arm and Sal's power bursts from her hand.";
        Shots["05_cemetery/blankCam"] = "Darkness.";

        // ---- The house, and its other versions
        Areas["05_house1"] = "Sal and Todd's house, in daylight: the kitchen with floral wallpaper, the hallway and stairs, the " +
                             "living room with the TV and the orange couch, a wilting plant on the hall table.";
        Shots["05_house1/photoCam"] = "A photo, held in Sal's hand: a young man with messy brown hair hugging an old lady with " +
                                      "curly grey hair, both smiling.";
        Shots["05_house1/churchCam1"] = "Up the hill past the Phelps Ministry sign, the white church stands against a blue sky.";
        Areas["05_house1_neilsRoom"] = "Todd's attic room, now Neil's: the solar system poster, the round table heaped with " +
                                       "papers, filing cabinets, the computer desk, and Neil standing by them.";
        Areas["05_house1_basement"] = "The basement, grey brick: a water heater, a washer and dryer, wardrobes, an old green " +
            "couch, boxes, a broken cage under the stairs, and dozens of photos and notes pinned to the wall.";
        Areas["05_house1_Shed"] = "The shed out back, the workshop. A package sits on the bench.";
        Areas["05_alt_House"] = "The house again, but flat and wrong, as if drawn without shadows: the kitchen, its microwave clock " +
            "glowing green, reading 1:45; a metal door with " +
            "a keypad, a scratched and splintered door, the living room, Sal's room with its masks, and hexagon plates mounted " +
            "on the walls with green lights above them.";
        Visuals["anim:black_on"] = "The lights go out.";

        // The pyramid puzzle's notes page, transcribed symbol by symbol (each is half a circle,
        // filled in or empty); checked against the three answers in pyramidPress_Place.
        MenuPages["Pyramids"] = "A crumpled page of handwritten notes. \"The three pyramids of Asintmah.\" A little map: " +
            "three pyramids marked N, W and E around a circle with a question mark, \"to be placed around a corpse?\" " +
            "A triangle split into three parts, each holding a circle: the top part's circle has its upper half filled in, " +
            "the bottom left part's circle its left half, the bottom right part's circle its right half. " +
            "\"These symbols are some kind of code... but where are they??\" Then three rows of half circles. " +
            "North: filled upper half, empty left half, filled left half, empty lower half, filled left half, empty left half. " +
            "West: empty left half, filled upper half, empty left half, filled left half, empty right half, filled upper half, " +
            "empty left half, filled upper half, empty lower half. " +
            "East: filled left half, empty left half, filled upper half, empty left half, filled right half, empty right half, " +
            "filled upper half, empty right half, filled right half, empty right half, filled upper half, empty left half.";
        // The forest terminals (Episode 5). The middle one first shows two pictures - the clues for the
        // two tile terminals, each drawn in the other terminal's symbols (checked against G1/G2_checkSolved).
        MenuPages["3D Grids"] = "Two pictures. On the left, a 3 by 3 grid of moons and suns. Top row: moon, moon, sun. " +
            "Middle row: sun, sun, moon. Bottom row: moon, sun, moon. On the right, a grid two wide and three tall of light " +
            "and dark squares. Top row: light, light. Middle row: dark, light. Bottom row: dark, dark.";
        // Solving a tile terminal (G1/G2_solved): the view cuts to the middle terminal as part of it rises.
        Visuals["05_forest/anim:terminal_up"] = "The tiles lock into place. With a deep hum, a hexagon block rises up out of " +
                                                "the ground at the middle terminal.";
        MenuPages["3D Code1"] = "A terminal with a 3 by 3 grid of tiles, each black or white. Pressing a tile flips it.";
        MenuPages["3D Code2"] = "A terminal with a grid of tiles two wide and three tall, each a sun or a moon. Pressing a " +
            "tile flips it.";
        MenuPages["Toon Code"] = "The keypad on Todd's door: a grey screen that shows what you type, and across the top " +
                                 "of its frame, three plus signs.";
        MenuPages["Toon Photo"] = "A framed photo, drawn like a cartoon: the four friends together. Larry on the left, " +
            "long dark hair, in his SF shirt, throwing the horns with one hand, his other arm around little Sal, who grins " +
            "in his mask and blue pigtails. Ash beside them in her purple top, one arm around Sal too, and Todd on the " +
            "right, wild orange hair and big round glasses, arms folded, smiling.";

        // The TV in the cartoon house: a number shows faintly through its static.
        Visuals["05_alt_House/anim:sink_num"] = "Hot water runs and the mirror over the sink fogs up. Written in the steam: " +
                                                "7, 8, 4, 2.";
        Visuals["05_alt_House/anim:tv_on"] = "The TV flickers on to snowy static. Faint dark digits show through it: 5, 0, 2, 3.";

        // The Center Chamber fight: cultists charge one at a time (c6 right, c1 left, c5 right, c2 left);
        // a touch is capture, Ash's R strike kills.
        Cutscenes["05_temple2_center/c6_attack"] = "A cultist charges in from the right!";
        Cutscenes["05_temple2_center/c1_attack"] = "A cultist charges in from the left!";
        Cutscenes["05_temple2_center/c5_attack"] = "A cultist charges in from the right!";
        Cutscenes["05_temple2_center/c2_attack"] = "A cultist charges in from the left!";
        foreach (var c in new[] { "c1", "c2", "c5", "c6" })
            Cutscenes[$"05_temple2_center/{c}_killed"] = "Sal's power blasts him; the cultist drops dead.";

        // ---- The treehouse fire, and the void
        Cutscenes["05_treeHouse/burning"] = "The treehouse catches fire. Flames climb the walls and roar up into the dark.";
        Areas["05_void"] = "The void: an endless dark grey space. Overhead, a tangle of black roots spreads like a web, " +
            "glowing orbs of drifting specks caught in it like stars. Ahead, a house drawn in thin white lines stands " +
            "alone in the dark, its windows cracked.";
        Shots["05_void/startCam"] = "Blackness, and in it a red tree inside a circle, its roots spreading below as wide as " +
                                    "its branches above.";
        // The opening talk is one long cutscene, so the area is described before its first line too.
        Lines[6350] = Areas["05_void"];
        Areas["05_void_House"] = "Inside the house in the void: everything drawn in faint white lines on black, a floor of hexagons. " +
            "A glowing white door, a staircase leading up into nothing, a black doorway, and a long hall of numbered doors.";
        Shots["05_void_House/cam_memory1"] = "A memory, scratched in white lines: two tall figures looming over a small child with " +
                                             "pigtails.";
        Areas["05_void_House/cam_hall"] = "A long dark hall: plain doors outlined in white, each with a number, faint streaks " +
            "of light rising up the walls from a floor of dark hexagon tiles.";
        Areas["05_void_House/cam_Room"] = "A dark room. A small door on the left, and in the middle a tall, narrow black slit " +
            "of a doorway reaching up into the dark, pale light glowing around its edges.";
        Areas["05_void_House/cam_workshop"] = "A workshop in the dark: a great heap of broken things, toppled buildings, crystals " +
            "and machine parts. On top of it a figure sits hunched at a little table, working, and from his head a river of " +
            "glowing blue and violet threads pours up into the blackness. A door far to the right.";
        Areas["05_void_House/cam_workshop (1)"] = Areas["05_void_House/cam_workshop"];
        Shots["05_void_House/cam_memory3"] = "A memory, scratched in white: a tangled mass hanging from above, strands dripping " +
                                             "down from it.";
        // Door 5's memory (after the hexagon keypad): one shot while the whole scene plays out (m4_play).
        Shots["05_void_House/cam_memory4"] = "A memory, scratched in white lines on black. Sal's mother, long blonde hair, " +
            "waves, and little Sal with spiky blue hair runs to her. A shadow rises behind them: a robed figure in a horned, " +
            "fanged mask like a beast's skull, a third eye carved on its brow. It levels a shotgun. The blast tears through " +
            "Sal's mother as she clutches him to her, blood spraying through them both, her mouth open in a scream, " +
            "Sal's face caught in it.";
        Shots["05_void_House/cam_memory2"] = "A memory: a car at night, headlights glaring, a bearded man at the wheel.";
        Areas["05_void2"] = "A dark space under a web of black roots, glowing orbs hanging in it like stars. Each holds a memory.";
        Shots["05_void2/cam_F1"] = "A memory, scribbled in white lines on black: Larry, long-haired, on a couch beside Sal, " +
                                   "both watching a film, a couple holding each other on the screen.";
        Shots["05_void2/cam_F2"] = "A memory: Sal, seen from behind, his pigtails and mask strap, facing Todd, whose hair " +
                                   "is the only colour here, orange, above big round glasses.";
        Shots["05_void2/cam_F3"] = "A memory: from either side, two pairs of hands reaching toward each other across a table " +
                                   "scribbled with splashes of paint.";
        Shots["05_void2/endCam"] = "Under the hanging orbs, a heap of lifeless bodies, the people of Addison Apartments, " +
                                   "piled on one another. Above them floats Ash's limp body, and rising straight up through " +
                                   "her, a jagged shard of blue light.";
        Visuals["05_void2/anim:bolt_burn"] = "The blue shard flares and burns, crackling with light.";

        // ---- The cult temple and the caves
        Areas["05_temple1"] = "The cult temple: long stone halls of arches and pillars, red banners bearing the cult's trident " +
            "symbol, spiked iron chandeliers and trident torch stands. Barred cells line one hall, someone held inside. Hooded " +
            "cultists with glowing eyes stand guard in the archways.";
        Areas["05_temple2"] = "The temple again: arched halls, red banners, chandeliers and tridents. Ash, her scarred arm glowing, " +
                              "makes her way through.";
        Areas["05_templeOS"] = "The temple's other side, grey and colourless.";
        Areas["05_temple2_center"] = "The temple's heart: hooded cultists stand around a dark pit in the floor. Bloody bodies hang " +
            "upside down on chains over it, blood dripping in. Behind it, on a dais under a carved hexagon door, a figure in a " +
            "goat skull mask stands over Todd, whose arms are spread wide.";
        Shots["05_temple2_center/travCam1"] = "Travis, older, blond and red-eyed, in a green cultist's robe, drives a glowing green " +
                                              "dagger into a shadowy figure in a horned skull mask.";
        Shots["05_temple2_center/detonateCam"] = "A bloodied thumb presses down on a button marked Detonate.";
        Areas["05_templeOS_center"] = "The heart of the temple's other side, grey: towering shadow creatures with glowing white " +
                                      "eyes rise from the pit, black tendrils reaching out.";
        Shots["05_templeOS_center/laryCUcam"] = "Larry, old now, with a thick black beard, his eyes blank and white, in a red hood.";
        Areas["05_pyramids"] = "Darkness, and three pyramids outlined in white: one glowing at the top, two below.";
        Shots["05_pyramids/ashCam"] = "Ash sits on Sal's old bed in the moonlight, stroking a sleeping ginger cat.";
        Shots["05_pyramids/ashCam (1)"] = "Ash's green eyes, wide.";
        Areas["05_forest"] = "A misty forest at dusk under a violet sky, drawn in simple flat shapes: bare trees, pines and stumps. " +
                             "Three stone plates lie in the grass: one with nine moons, full and crescent, one blank, one a grid " +
                             "of light and dark squares.";
        Areas["05_caveEnt"] = "A cave mouth.";
        Areas["05_cave"] = "Inside a sandy cave: walls painted with red stick figures and handprints, a figure with a spiked crown, " +
            "a mouth of light where the cave opens onto water, a tunnel spiralling back into darkness, and a wall carved with " +
            "three squares beneath a downward triangle.";
        Areas["05_lab"] = "An office hidden in the temple, a red-lit cellar: anatomy charts and diagrams pinned up, a bookcase " +
                          "of old books and specimen jars, the center chamber's control console, a tall tank of glowing green " +
                          "liquid plastered with biohazard signs, a heavy ornate safe, a nightstand, a rug, and a big wooden door.";
        Shots["05_lab/safeCam"] = "An ornate metal safe with a numbered dial.";
        Shots["05_lab/consoleCam"] = "Three wall panels, each with a switch and a slider marked 1, 2, 3.";

        // ---- The final battles and the end
        Areas["05_32_field"] = "A pixel art arena of grass and stone. Sal, with his guitar, faces creeping black blobs with red " +
                               "eyes, and green tentacles reach in from the edges.";
        Areas["05_32_boss"] = "The pixel arena. Enormous arms rise to attack.";
        Areas["05_endless_2_clay"] = "A world of clay: three empty chairs in a red glow. A huge red clay eye in a burst of green.";
        Shots["05_endless_2_clay/salCam"] = "Sal made of clay, wide-eyed, half his face orange.";
        Areas["05_endless_5_Toon"] = "A flat green cartoon world, a giant yellow eye with a purple pupil watching Sal.";
        Cutscenes["05_END/dialog"] = "Dawn over the ruins of the church: shattered walls and splintered beams on the hilltop, the " +
                                     "sun rising. Ash, glowing, kneels beside Todd, lying in the rubble.";
        Shots["05_END/finalBlowCam2"] = "The final blow: Ash's beam of light, a burst of green lightning from Sal in the void, and " +
                                        "a wave of purple from Larry, all striking together.";
        Visuals["anim:oldLarry_appear"] = "Larry appears, old and bearded.";
        Visuals["anim:toddCough"] = "Todd coughs and stirs.";
        Visuals["anim:ashArmOut"] = "Ash holds out her glowing arm over Todd.";
    }
}
