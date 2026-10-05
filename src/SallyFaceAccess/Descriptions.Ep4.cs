namespace SallyFaceAccess;

/// <summary>Episode 4: The Trial - the Dark Place, Sal and Todd's house, the courtroom, Addison Apartments in three times.</summary>
internal static partial class Descriptions
{
    static partial void Episode4()
    {
        // ---- The nightmare and the Dark Place
        Cutscenes["04_intro/disablePlayerUpDown"] = "Night. A small figure with a guitar stands in the lit doorway of Addison " +
            "Apartments. Flashes of a nightmare, all in red scratches: a corridor, a face, bloodied hands. Then Sal's house, " +
            "dark and tilted, furniture floating in the air, and an alarm clock reading 3:33.";
        // The nightmare's walkable room, from the tour screenshots.
        Areas["04_intro/houseCam"] = "A nightmare mash-up of memories in a dim, reddish light. Sal's bedroom: red posters of staring " +
            "eyes, Twine Parks and Edward Shovelhands, a dresser with pill bottles and cups scattered across the floor around it, the " +
            "green bed, a TV hanging upside down from the ceiling. To the left, a dark stone archway, and beyond it Mrs. Packerton's " +
            "iron furnace with black bags beneath it and a blood-spattered freezer.";
        Shots["04_intro/alarmClock"] = "An alarm clock in the dark, its green display glowing 3:33 AM.";
        Areas["04_darkPlace"] = "The Dark Place: a black void drawn in white chalk lines. A long row of numbered doors, left to " +
            "right: door 5, boarded shut, then 4, 3, 2 and 1, and further along door 6, beside a goat-headed demon with a " +
            "serpent's tail, Beelzebub.";
        // Behind each door, a room of its own (camera = room), from the tour screenshots.
        Areas["04_darkPlace/cam1"] = "Behind door 1: a pair of huge chalk-drawn eyes hang in the dark on the left, following you. " +
            "On the far right wall a rune is scratched: an N with a doubled slash, beside a line with a triangle on its right side.";
        Areas["04_darkPlace/cam2"] = "Behind door 2: on the far left wall a rune is scratched: a line with a triangle on its left " +
            "side, like a flag, beside an F. Far to the right, a cluster of square, weeping faces on dripping stalks, above a puddle.";
        Areas["04_darkPlace/cam3"] = "Behind door 3: to the left stands Trapanrot, a figure made of jumbled arms and legs. On the " +
            "far right wall a rune is scratched: a zigzag hook, beside a line crossed by an X, like a starburst.";
        Areas["04_darkPlace/cam4"] = "Behind door 4: numbers glow on the wall at the left, 342 above 697. Further on, pale smoke " +
            "rises from flames around a dark kneeling figure, and a goat's skull hangs on a chain above.";
        Areas["04_darkPlace/cam6"] = "Behind door 6: an old TV on a little stand, alone in the dark.";
        Areas["04_tv"] = "An old television on a metal table, a tall stack of VCRs beside it, tangled cables pooling on a " +
                         "black and white zigzag floor.";

        // ---- Sal and Todd's house
        Areas["04_house"] = "Sal and Todd's house, years later: a kitchen with floral wallpaper, wooden cabinets, a stove, a " +
            "microwave and a fridge, and a round table with blue chairs. Then a hallway: stairs up, a plant on a small table, " +
            "doors to the basement and the bathroom, and a living room with a big TV, a game console and an orange couch.";
        Areas["04_house/salsCam"] = "Sal's room: an SF poster, a Wednesday poster, a bookcase, an amp and an electric guitar, " +
            "a dresser covered in pill bottles, a wall of masks, green, black, white, a clown, a bed with a green blanket, " +
            "and framed pictures.";
        Areas["04_house/bathCam"] = "A pink tiled bathroom with towels hanging and a laundry basket.";
        Areas["04_house_toddsRoom"] = "Todd's attic room: slanted ceiling with skylights, a solar system poster, a round table, " +
            "pink chairs, filing cabinets, a computer desk, a map pinned to the wall, and a bed. Neil, Todd's boyfriend, " +
            "stands holding a mug.";
        Areas["04_house_Shed"] = "The shed out back, now a workshop: a corkboard covered in clippings and pentagram drawings " +
            "linked with red string, old computers, a safe, shelves of books and jars, a trident, tall metal lockers, a tool " +
            "chest and a workbench.";
        Areas["04_house_Basement"] = "The basement, with stairs up and a door out to the backyard. Gizmo is down here.";
        Areas["04_house_Backyard"] = "The backyard, with the house on one side and the shed on the other.";

        // ---- The courtroom and Nockfell
        Cutscenes["04_courtroom/1"] = "A courtroom, In God We Trust carved above the judge's bench. A grey-haired judge in black " +
            "robes. Sal on the stand in his orange jumpsuit, a guard beside him. A prosecutor with purple hair in a grey suit. " +
            "The jury and a full gallery watch.";
        Shots["04_courtroom/camBench_Ash"] = "Ash on the witness stand, short brown hair now, a choker at her neck.";
        Shots["04_courtroom/camBench_Enon"] = "Dr. Enon on the witness stand, in shirt and green tie.";
        Shots["04_house/trailCam"] = "A gavel strikes the block.";
        Areas["04_outside1"] = "Outside a blue clapboard house with a porch, on a sunny day: a gravel road, a chain-link fence, " +
                               "green hills beyond.";
        Areas["04_outside2"] = "A Nockfell street.";
        Areas["04_outside3"] = "A Nockfell street, by a sign reading Phelps Ministry. Up the hill, a white church with a steeple, " +
                               "and beside it a fenced graveyard.";
        Areas["04_outside4"] = "The street outside Addison Apartments.";

        // ---- Addison Apartments, adult years (Larry's ghost at Sal's side)
        Areas["04_102"] = "Apartment 102, Miss Rosenberg's old place, emptied out: her furniture draped in white dust sheets, " +
                          "a cat plaque leaning against them, pale shapes on the wallpaper where pictures once hung.";
        Areas["04_104"] = "Apartment 104: dusty, cobwebs in the corner, a wardrobe, cardboard boxes stacked everywhere, " +
                          "and filing cabinets at the far end.";
        Areas["04_201/NavCam 1"] = "Apartment 201, the Packertons' old place, now bare: an old blood stain on the kitchen floor, " +
                                   "an orange couch, a pale patch on the wall where the barn painting hung, and closed doors.";
        Areas["04_201/bbr1_Cam"] = "Bedroom 1, empty now but for the rack of life support machines against the wall.";
        Areas["04_201/br2_Cam"] = "Bedroom 2, stripped: the hooks where the tools hung, empty shelves, old stains on the carpet.";
        Areas["04_201/bathroomCam"] = "The grimy green bathroom, the stain still in the tub.";
        Areas["04_202/NavCam 1"] = "Apartment 202, Todd's parents' place: the hookah, the tan couch where Janis sits, colourful " +
                                   "rug and record player, family pictures, and the green couch.";
        Areas["04_202/ToddsRoomCam"] = "Todd's old room, now a grow room: rows of potted cannabis plants under hanging lamps, " +
                                       "a wardrobe, and Ray, Todd's dad, with red hair and beard, admiring them.";
        Areas["04_203"] = "Apartment 203: dim and empty. In the kitchen at the far end, something small and red lies on the counter.";
        Areas["04_204"] = "Apartment 204, empty and dim, with a filthy bathroom.";
        Areas["04_301"] = "Apartment 301, CJ's place: the IGG banner, a stereo, a cabinet, a box of junk, a folding chair.";
        Areas["04_303"] = "Apartment 303, Chug and Maple's: a beige couch where Chug, grown up with green hair and a cap, sits " +
                          "with Maple, grey hair, beside him. Toys scattered on the floor, a toy chest, and their little girl " +
                          "playing nearby.";
        Areas["04_304"] = "Apartment 304: dim, green, and empty.";
        Areas["04_401"] = "Apartment 401, Robert's: the punk and horror posters, the mattress and the boombox.";
        Areas["04_402/NavCam 1"] = "Apartment 402, now dad and Lisa's home: brown armchairs, a side table with a phone, and a " +
                                   "framed photo of the family on the wall, Sal, Larry, Lisa and dad.";
        Areas["04_402/salsRoomCam"] = "Sal's old room, now dad's office: a desk with a computer and an office chair, a filing " +
                                      "cabinet, a briefcase on the floor, a dresser, and storage boxes on a shelf.";
        Areas["04_402/dadsRoomCam"] = "Dad and Lisa's bedroom: the brass bed with an orange blanket.";
        Areas["04_402/bathroomCam"] = "The pale green bathroom.";
        Areas["04_403"] = "Apartment 403: dim and empty, green walls and carpet.";
        Shots["04_403/ghostCam"] = "The ghost of a thin old man with wisps of red hair, in a lilac sweater, stands with his head bowed. " +
                                   "Mr. Sanderson.";
        Areas["04_404"] = "Apartment 404: dark, bare, and empty, a broken jar on the floor in the corner.";
        Areas["04_502"] = "Apartment 502: dark and empty, the old birdcage on the floor.";
        Shots["04_502/coveredCam"] = "A round metal plate, scratched and screwed down at four points.";
        Visuals["anim:cageGlowing"] = "The birdcage begins to glow and hum.";
        Visuals["anim:cageGlowStar"] = "A star of light blazes inside the cage.";
        Areas["04_501"] = "Apartment 501: dark, the old newspapers on the wall.";
        Areas["04_503"] = "Apartment 503: dark, the hole broken through to 504.";
        Areas["04_504/NavCam 1"] = "Apartment 504: dark and filthy, newspapers on the floor.";
        Areas["04_504/bathroomCam"] = "The mouldy bathroom where Megan stays.";
        Areas["04_504_2"] = "504's bedroom: the torn mattress against the wall, the hole through to 503, and a noose hanging in " +
                            "the darkness.";
        Areas["04_Fbr1/NavCam 1"] = "Larry and Lisa's old basement apartment.";
        Areas["04_Fbr1/larrysRoomCam"] = "Larry's room, as he left it: the posters, the stereo and the easel.";
        Areas["04_Fbr1/lisasRoomCam"] = "Lisa's old room.";
        Cutscenes["04_cycleDate/disablePlayerUpDown"] = "Sal and Ash ride a motorcycle down a misty forest road, both in helmets, " +
                                                        "Ash driving, Sal holding on behind.";
        Cutscenes["04_date/disablePlayerUpDown"] = "A misty lake surrounded by forested hills. Sal and Ash sit together on the " +
                                                   "grassy bank beneath a twisted, leaning tree.";
        AfterLines[5335] = "Ash pulls Sal to his feet, and together they scream out across the lake.";
        Areas["04_outBack_Dark"] = "Behind the building, at night, in pouring rain. The back door and the trash cans, and far to " +
                                   "the left in the darkness, the treehouse tree.";
        Cutscenes["04_outBack_Dark/getNote"] = "Sal finds a note left at the foot of the tree, and reads it in the rain.";
        Areas["04_Fbr1_dark"] = "Larry and Lisa's apartment, dark and silent.";
        Areas["04_treeHouseDark"] = "The treehouse at night, rain drumming on the roof. Todd is here with Sal.";
        Shots["04_treeHouseDark/cultCam1"] = "A vision: the cult's inner sanctum bathed in purple light. Hooded figures with glowing " +
            "eyes stand in a ring. At their centre, a figure in a horned goat mask, a green gem blazing on his chest.";
        Shots["04_treeHouseDark/cultCam3"] = "A tall crystal shard carved with rings, rising through the dark.";
        // ---- The building in two versions, and the rifts
        Visuals["anim:pillarHole"] = "A huge stone pillar carved with a circle of symbols has burst up through the floor, black ooze " +
                                     "dripping down its sides.";
        Visuals["anim:pillarHoleOS"] = "A huge stone pillar carved with a circle of symbols stands here, black ooze dripping down its sides.";
        Visuals["anim:pillarBreak"] = "The stone pillar cracks apart and crumbles.";
        Visuals["anim:pillarBreakFinal"] = "The last pillar shatters.";
        Visuals["anim:rift_On"] = "A rift tears open in the air, crackling with white lightning.";
        Visuals["anim:rift_Off"] = "The rift dims.";
        Visuals["anim:rift_Gone"] = "The rift seals shut and vanishes.";
        Visuals["anim:larryOS_fadeIN"] = "Larry's ghost fades into view.";
        Visuals["anim:larryOS_fadeOUT"] = "Larry's ghost fades away.";
        Visuals["anim:rabbitLit"] = "A rabbit-like creature lights up in the gloom.";
        Visuals["anim:rabbitKilled"] = "The creature is struck down.";
        Visuals["anim:lightningStrike"] = "Lightning strikes.";
        Visuals["anim:404_vhsBlast"] = "The tape bursts apart in a flash.";
        Areas["04_2_F4"] = "The fourth floor hallway, doors 401 to 404, the door of 403 splintered. Halfway along, a huge stone " +
                           "pillar carved with a circle of symbols has burst up through the floor, black ooze dripping from it.";
        Areas["04_os_F4"] = "The fourth floor hallway: the doors grey and peeling, the doorway of 404 a black void, and the huge " +
                            "carved stone pillar dripping black ooze in the middle.";
        Areas["04_os_402/NavCam 1"] = "Apartment 402: grey armchairs and side tables, cracked frames on the walls. A towering shadow " +
                                      "creature with glowing white eyes and tangled tendrils looms in the room, and white lightning " +
                                      "crackles from a rift on the floor.";
        Areas["04_os_402/bathroomCam"] = "The bathroom, rotting, blood streaked down the sink and tub.";
        Areas["04_os_roof"] = "The roof, in black rain: crumbling stone ledges, and a tall carved crystal monolith.";
        Areas["04_2_roof"] = "The roof of Addison Apartments.";
        // ---- The Endless One, the murders, the execution
        Areas["04_2_103"] = "Apartment 103, Mr. Addison's: dark, black ooze and tangled black tendrils spreading over the walls, a " +
                            "heap of black sludge full of bones in the middle of the room.";
        Cutscenes["04_2_103/onSceneStart101"] = "Mr. Addison stands at the end of the room, but wrong: his body hangs limp like a " +
                                                "puppet, green slime oozing over it.";
        Shots["04_2_103/cam2"] = "His true form rises behind him: a colossal mass of green slime, human hands and arms sticking out " +
                                 "of it, a small pale head at its tip.";
        Areas["04_2_battle"] = "A purple void between walls of green flesh. Sal floats in it, guitar in hand, his hair blazing like " +
                               "blue fire. In the flesh wall, a giant eye opens, its pupil a glowing circle of symbols.";
        Shots["04_2_battle/gameCamWhite"] = "Endless white. Far off, a small figure: Terrence Addison, as he was.";
        Visuals["anim:terrenceKnife"] = "Terrence holds out a knife to Sal.";
        Visuals["anim:terrenceFade"] = "Terrence fades into the white.";
        Cutscenes["04_3_end/onSceneStart"] = "The entrance hall of Addison Apartments at night, the Addison Tea sign on the floor, " +
                                             "black tendrils at the edges of everything.";
        Shots["04_3_end/NavCam 1 (1)"] = "Todd slumps against the wall by the front door, pale, eyes red-rimmed, blood soaking his " +
                                         "shirt from a wound in his side.";
        Cutscenes["04_END/disablePlayerUpDown"] = "Ash races down a road on her motorcycle under a red sky.";
        Shots["04_END/hallCam1"] = "Two guards lead Sal, in his orange jumpsuit, down a long grey corridor.";
        Shots["04_END/chairCam"] = "Sal is strapped into an electric chair, a metal cap on his head.";
        Shots["04_END/window1Cam"] = "Ash pounds on the viewing window with one fist, a paper pressed to the glass in her other hand.";
        Shots["04_END/exoCam"] = "The executioner checks his watch, hand on the lever.";
        Shots["04_END/window2Cam"] = "Ash's face against the glass, crying, as the chair behind it goes still.";
        Visuals["anim:ashCam_snap"] = "Ash raises her camera. A blinding flash.";
        Areas["04_treeHouseEnd"] = "The treehouse. Ash is here, alone, a stereo playing.";
        Areas["04_Fbr2/NavCam 1"] = "The hidden apartment behind the basement shelf, abandoned. A hatch in the floor at the far end.";
    }
}
