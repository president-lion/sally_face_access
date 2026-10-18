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
        // Todd's note (Item Close Up, line 5787): the code is only in the picture. From the shed keypad's check.
        MenuPictures[5787] = "The note reads: the new shed code is 4 0 3 5 3 6 4.";
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
        Areas["04_2_403"] = "Apartment 403, almost black: bare green walls and carpet, nothing else in the room.";
        // The zone a few steps in: a stinger as Sal sees what is at the far end.
        Cutscenes["04_2_403/Interaction"] = "Slumped against the far right wall: Mr. Sanderson's body, in his lilac sweater. " +
                                            "The top of his head is blown open, blood sprayed up the wall above him and " +
                                            "pooled around him, a pistol fallen by his hand.";
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
        // Before Sal's "..." so the note's own text, read after that line, comes last.
        Lines[5364] = "Sal finds a note left at the foot of the tree, and reads it in the rain.";
        AfterLineTexts[5364] = ("Item Close Up", "larrysNote Text");

        // The barrier spells: the camera moves to the pillar when Sal steps up to it.
        const string pillar = "A barrier pillar: a towering block of blue marble veined with red, black ooze dripping " +
                              "down its sides and spreading across the floor. Carved on its face, a ring of eight circles " +
                              "around a diamond inside a square.";
        foreach (var f in new[] { "04_2_F1", "04_2_F2", "04_2_F3" }) Shots[$"{f}/guitarCam"] = pillar;
        Shots["04_2_F4/pillarCam"] = pillar;
        Shots["04_2_roof/pillarCam"] = pillar;

        // Todd's guitar, in any scene: choosing a group of notes with a direction.
        Cutscenes["*/trigger_gUP"] = "up";
        Cutscenes["*/trigger_gR"] = "right";
        Cutscenes["*/trigger_gD"] = "down";
        Cutscenes["*/trigger_gL"] = "left";
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
        // ---- The other side's apartments, as Larry sees them: grey and crumbling, the tenants' true shapes inside.
        Areas["04_os_201/NavCam 1"] = "Apartment 201 on the other side: grey, cracked plaster walls, a boxy grey sofa, " +
                                      "empty and silent.";
        Areas["04_os_202/NavCam 1"] = "Apartment 202 on the other side, Todd's home. A grey kitchen, black ooze dribbling from the " +
                                      "sink. The door to Todd's room is webbed shut with thick black strands. In the living room, " +
                                      "Todd's parents: a single hunched mass of shadow draped over the sofa, two pale faces with " +
                                      "glowing white eyes sunk into it, black tendrils hanging from the ceiling into its back.";
        Areas["04_os_204/NavCam 1"] = "Apartment 204 on the other side: bare cracked walls, and a dark ritual circle scrawled on " +
                                      "the floor.";
        Areas["04_os_301"] = "Apartment 301 on the other side: cracked walls under a peeling IGG banner, a stereo and speakers, a " +
                             "cabinet, an overflowing bin. In the corner, the children who live here: a towering knot of black " +
                             "ooze with three small glowing faces caught in it, tendrils rising from it into the ceiling.";
        Areas["04_os_302"] = "Apartment 302 on the other side: a sofa, a crate, shelves on the wall, a tiny picture of a grey " +
                             "cloud. Sara sits on the sofa, her pale body split by black veins of ooze, long black fingers " +
                             "hanging from her hands. By the door, Dave: a dripping column of black sludge with two glowing " +
                             "eyes, strands of it stretching up to the ceiling.";
        Areas["04_os_303"] = "Apartment 303 on the other side: a worn sofa and a toy chest, toys scattered across the floor. " +
                             "Faint, see-through echoes of a heavy man in a cap slumped on the sofa and a little girl " +
                             "sitting on the floor. By the toy chest, a little dark pony toy with white eyes and a crooked " +
                             "grin.";
        Areas["04_os_401"] = "Apartment 401 on the other side, Robert's: cracked walls covered in horror and band posters, a " +
                             "skull, a monster. A stereo crackles with white lightning, CDs scattered around it. On a " +
                             "mattress in the corner sits Robert's shadow form, hunched and black, one pale eye glowing in " +
                             "a gaunt face, black branches growing up out of him into the ceiling.";
        Areas["04_os_403"] = "Apartment 403 on the other side: bare, black and grey, a grey door hanging open.";
        Shots["04_os_403/ghostCam"] = "Mr. Sanderson's ghost stands against the wall, head bowed, wild red hair matted with " +
                                      "blood, his lilac sweater soaked red down the front.";
        Areas["04_os_404"] = "Apartment 404 on the other side: nothing but blackness, specks drifting in it like dust. On the " +
                             "floor in a scatter of debris, a broken video player.";
        Areas["04_os_501"] = "Apartment 501 on the other side: bare cracked walls. In the corner, on a bed of scattered " +
                             "newspapers, the homeless man sits hugging his knees, long hair over his face, barefoot.";
        Areas["04_os_504/NavCam 1"] = "Apartment 504 on the other side: grey, cracked and empty.";
        Areas["04_os_504/bathroomCam"] = "The bathroom: grimy tiles, a cracked mirror, black ooze hanging from the sink.";
        Areas["04_os_504_2/NavCam 1"] = "A dark room on the other side: a crate against the cracked wall, a ruined board of " +
                                        "tiles beside it.";
        Shots["04_os_504_2/holeCam1"] = "A jagged hole broken through the wall, pitch black inside.";
        Areas["04_2_roof"] = "The roof of Addison Apartments.";
        // ---- The rooms while the stone pillars stand in the halls
        Areas["04_2_104"] = "Apartment 104: dusty, a cobweb in the corner, a wardrobe, cardboard boxes stacked everywhere, and " +
                            "filing cabinets at the far end.";
        Areas["04_2_201/NavCam 1"] = "Apartment 201, bare: a kitchen with a checkered floor and an old blood stain on it, an " +
                                     "orange couch, a pale patch on the wall where a painting hung, the green front door, and " +
                                     "closed doors to the other rooms.";
        Areas["04_2_201/bathroomCam"] = "A green-tiled bathroom, grubby: a stain in the tub, a dirty toilet with a plunger.";
        Areas["04_2_201/bbr1_Cam"] = "Bedroom 1, empty but for the rack of life support machines against the wall.";
        Areas["04_2_201/br2_Cam"] = "Bedroom 2, stripped bare: benches along stained walls, rows of hooks, pipes, and an old " +
                                    "iron furnace in a puddle.";
        Areas["04_2_202/NavCam 1"] = "Apartment 202: a kitchen, then a living room with plants, a hookah on a side table, framed " +
                                     "pictures and a colourful round rug. On the tan couch sits Janis, a red-haired man asleep " +
                                     "with his head in her lap. A green couch by the front door.";
        Areas["04_2_202/bathroomCam"] = "A clean mint-tiled bathroom: a tub with an orange curtain, a pink towel, a blue mat.";
        Areas["04_2_204/NavCam 1"] = "Apartment 204, empty and dim: bare walls, a kitchen at the far end.";
        Areas["04_2_204/bathCam"] = "A filthy bathroom: a stained toilet and tub.";
        Areas["04_2_301"] = "Apartment 301, CJ's place: the IGG banner, a stereo with speakers, a cabinet, a box of junk, a " +
                            "folding chair. On the left, three friends: a blond man in a beanie in a wheelchair, a woman with " +
                            "curly black hair and a green scarf standing, and a man in a red hoodie reading a book on a chair.";
        Areas["04_2_302"] = "Apartment 302: a red couch with a mannequin torso propped on it, a shelf with a framed photo, a " +
                            "board pinned with newspaper clippings, a small picture of a cloud. David stands by the cabinet, " +
                            "thin as a rake, in a vest and underpants, staring.";
        Areas["04_2_303"] = "Apartment 303: Chug, green hair and a cap, sits on the grey couch, his little girl on the floor " +
                            "among scattered toys, a toy chest beside them.";
        Areas["04_2_401"] = "Apartment 401, Robert's: punk and horror posters, a safe, CDs scattered by a boombox, a lamp on a " +
                            "chair. Robert, red mohawk, sits hunched on the mattress.";
        Areas["04_2_402/NavCam 1"] = "Apartment 402, dad and Lisa's home: a kitchen, then the living room, family photos on the " +
                                     "wall. Dad and Lisa sit close together on the brown couch, facing the TV.";
        Areas["04_2_402/bathroomCam"] = "The clean pale green bathroom: an orange towel, a plunger by the toilet.";
        Areas["04_2_404"] = "Apartment 404: dark, bare, and empty, something broken on the floor in the corner.";
        Areas["04_2_501"] = "Apartment 501, empty: a boarded window, holes in the plaster, and heaps of old newspapers on the floor.";
        Areas["04_2_504/NavCam 1"] = "Apartment 504: dark and bare, handprints on a door, a boarded window, loose planks.";
        Areas["04_2_504/bathroomCam"] = "The mouldy bathroom: a grimy tub, a stained toilet with a plunger, a pill bottle on the floor.";
        Areas["04_2_504_2/NavCam 1"] = "504's bedroom: a broken shelf, the torn mattress against the wall, and the hole through to 503.";
        Areas["04_2_504_2/503Cam"] = "Apartment 503: dark, the hole broken through from 504, and a noose hanging in the darkness.";
        // ---- The Endless One, the murders, the execution
        Areas["04_2_103"] = "Apartment 103, Mr. Addison's: dark, black ooze and tangled black tendrils spreading over the walls, a " +
                            "heap of black sludge full of bones in the middle of the room.";
        Cutscenes["04_2_103/onSceneStart101"] = "Mr. Addison stands at the end of the room, but wrong: his body hangs limp like a " +
                                                "puppet, green slime oozing over it.";
        Shots["04_2_103/cam2"] = "His true form rises behind him: a colossal mass of green slime, human hands and arms sticking out " +
                                 "of it, a small pale head at its tip.";
        Shots["04_2_battle/gameCamWhite"] = "Endless white. Far off, a small figure: Terrence Addison, as he was.";
        // The fight with the Endless One: six rounds of runes. Kept short - the next runes cut in within seconds.
        Areas["04_2_battle"] = "A purple void between towering walls of green flesh, dripping slime. Sal falls through it, " +
                               "guitar in hand, his hair blazing like blue fire. The Endless One: a giant slit eye in the flesh " +
                               "wall, smaller black eyes clustered around it. Each round a glowing circle of symbols lights " +
                               "its pupil, and the rune to play burns at the centre.";
        Visuals["anim:endless_intro"] = "The wall of flesh heaves. The giant eye rolls open and stares at Sal.";
        for (var i = 0; i < 5; i++)
            Cutscenes[$"04_2_battle/song_{i}_WIN"] = $"Hit! Lightning blasts from the guitar into the eye. It shrieks, squeezes " +
                                                     $"shut, and the flesh drags Sal deeper. {i + 1} of 6.";
        Cutscenes["04_2_battle/song_5_WIN_KILLED"] = "The last blast! Lightning tears through the eye; it bursts, the walls of flesh " +
                                                     "rip apart and shake, and everything floods with blinding white.";
        for (var i = 1; i < 6; i++)
            Cutscenes[$"04_2_battle/song_{i}_FAIL"] = $"Too slow. The symbol fades, the eye snaps shut, and the flesh hauls Sal back " +
                                                      $"up. Back to {i - 1} of 6.";
        Cutscenes["04_2_battle/song_0_FAIL_END"] = "Too slow. The eye snaps shut and the walls of flesh crush in around Sal.";
        Shots["04_2_battle/gameCamBlack"] = "The Black Room: nothing but darkness, and Rosenberg's voice.";
        Visuals["anim:terrenceKnife"] = "Terrence holds out a knife to Sal.";
        Visuals["anim:terrenceFade"] = "Terrence fades into the white.";
        // ---- The murders: Sal goes door to door with the knife. Everyone is grey-faced and hollow-eyed.
        const string stab = "The screen floods red, black scratches tearing across it. The knife drives down, " +
                            "slick with blood.";
        foreach (var s in new[] { "04_3_F1", "04_3_202", "04_3_301", "04_3_302", "04_3_303", "04_3_401", "04_3_402" })
            Shots[$"{s}/stabCam"] = stab;
        Areas["04_3_F1"] = "The first floor hallway, a security camera's red light blinking near the ceiling. Outside her open " +
                           "door, 101, Mrs. Gibson lies curled on the carpet: her skin sickly green and blotched, arms " +
                           "wrapped around herself.";
        Shots["04_3_F1/endCam"] = "Close on Mrs. Gibson, curled on the floor, looking up at Sal.";
        Areas["04_3_202"] = "Apartment 202, Todd's home: bright flowered paintings, a hookah on a side table. Janis sits on the " +
                            "sofa, pale and dazed, her eyes red. Ray lies across her lap, staring blankly at nothing.";
        Areas["04_3_301"] = "Apartment 301 under the IGG banner. Sal's friends wait together, quiet and staring: one in a " +
                            "wheelchair, purple hoodie and beanie; one standing in a long brown coat and green scarf; one " +
                            "on a wooden chair in a red hoodie, a little green book in her hands.";
        Areas["04_3_302"] = "Apartment 302: a corkboard of newspaper clippings, a wedding photo on the shelf. David stands in " +
                            "the middle of the room, skeletal, in a vest and underpants, hand on his stomach. On the red sofa " +
                            "behind him, a mannequin's torso: Sara.";
        Shots["04_3_303/introCam"] = "Apartment 303: Chug sits on the sofa, green hair under his cap, staring. Little Soda " +
                                     "sits on the floor at his feet among scattered toys, wide-eyed.";
        Areas["04_3_401"] = "Apartment 401, Robert's: posters everywhere, the boombox and CDs on the floor. Robert sits hunched " +
                            "on his mattress, red mohawk drooping, staring at the floor.";
        Areas["04_3_402"] = "Home, 402. Sal's dad and Lisa sit close together on the brown sofa, his arm around her, both " +
                            "tired and red-eyed. On the wall above them, their wedding photo, and a family picture: Sal, " +
                            "Dad, Lisa and Larry, smiling.";
        Cutscenes["04_3_end/onSceneStart"] = "The entrance hall of Addison Apartments at night, the Addison Tea sign on the floor, " +
                                             "black tendrils at the edges of everything.";
        Shots["04_3_end/NavCam 1 (1)"] = "Todd slumps against the wall by the front door, pale, eyes red-rimmed, blood soaking his " +
                                         "shirt from a wound in his side.";
        Cutscenes["04_END/disablePlayerUpDown"] = "Ash races down a road on her motorcycle under a red sky.";
        // Flashes between stretches of Ash's ride: each cuts to one corridor shot and pans straight on to the
        // next, so the camera change alone would miss the first; they are described by the flash itself.
        Cutscenes["04_END/flash1"] = "Flash: a long prison corridor under flickering ceiling lights. Sal, in an orange jumpsuit, " +
                                     "hands cuffed behind his back and ankles chained, is marched away between two guards.";
        Cutscenes["04_END/flash2"] = "Flash: further down the corridor now, Sal and the guards nearly at the door at its end.";
        Visuals["show:executioner (2)"] = "The executioner steps up to the lever.";
        Visuals["anim:chairON"] = "He pulls it. The chair jolts, current crackling through Sal's body.";
        Shots["04_END/chairCam"] = "Sal is strapped into an electric chair, a metal cap on his head.";
        Shots["04_END/window1Cam"] = "Ash pounds on the viewing window with one fist, a paper pressed to the glass in her other hand.";
        Shots["04_END/exoCam"] = "The executioner checks his watch, hand on the lever.";
        Shots["04_END/window2Cam"] = "Ash's face against the glass, crying, as the chair behind it goes still.";
        Visuals["anim:ashCam_snap"] = "Ash raises her camera. A blinding flash.";
        Areas["04_treeHouseEnd"] = "The treehouse. Ash is here, alone, a stereo playing.";
        Areas["04_Fbr2/NavCam 1"] = "The hidden apartment behind the basement shelf, abandoned. A hatch in the floor at the far end.";
    }
}
