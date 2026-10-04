namespace SallyFaceAccess;

/// <summary>Episode 2: The Wretched - the dream, then Addison Apartments in winter.</summary>
internal static partial class Descriptions
{
    static partial void Episode2()
    {
        Areas["02_sgb"] = "The Super Gear Boy, Todd's upgraded handheld, fills the view, a red Super label on the case. On its " +
                          "green screen: In Seeking Providence, two glowing eyes in a tangle of roots. The game is played on the " +
                          "screen, chapter by chapter.";

        // ---- The opening dream
        Areas["dream1"] = "A dream. A golden field under a hazy yellow sky, dotted with bare, twisted trees that grow " +
                          "thicker to the right into a dark wood. Further on, a woman with long blonde hair sits on a " +
                          "picnic blanket beside a basket.";
        Areas["dream1/navCam (1)"] = "An empty parking lot under a blood-red sky, the silhouettes of ruined buildings in the " +
                                     "distance and a chain-link fence behind. Along the guard rail lie scattered remains and " +
                                     "skulls. At the far end, an ambulance stands with its back doors open, blood pouring out " +
                                     "across the ground.";
        Cutscenes["dream1/birdsFly"] = "A flock of birds bursts up from the grass and flies off.";
        Cutscenes["dream1/momChat"] = "Sal's mom looks up from the picnic blanket.";
        Areas["dream2"] = "Apartment 403, before anything happened to it: yellow wallpaper, a beige couch with a yellow " +
                          "throw, a framed picture of a couple, and pony figurines on a dresser. Mrs. Sanderson, a woman " +
                          "with curly green hair in a purple dress, stands by the kitchen.";
        Areas["dream2/BathroomCam"] = "The bathroom: green tiles, a bathtub and sink, and at the far end a toilet " +
                                      "overflowing onto the floor, a plunger lying in the puddle.";
        Cutscenes["dream2/toiletAction"] = "Sal plunges the toilet. Something gurgles and roars up from below.";
        Areas["dream3"] = "A sewer. Brick walls and walkways with green railings above channels of glowing green sludge. " +
                          "Ladders lead up to big rusted pipes overhead, where switches control the lights. " +
                          "In an archway sits a hideous rotting pony creature with a horn and stringy yellow mane.";
        Areas["dream3/BaseCam 2"] = "A lower channel of bright blue water. A bloated pink pony carcass floats in it. " +
                                    "At the end, a doorway opens onto a filthy bathroom.";
        Areas["dream4"] = "Apartment 403 again, now a murder scene: the door broken, furniture smashed, blood spattered over " +
                          "the walls and couch. Mrs. Sanderson lies dead on the carpet in a pool of blood.";
        Areas["dream4/bathroomCam"] = "The bathroom, in dim green light. Sal stands by the toilet.";
        Cutscenes["dream4/bathroomDoor"] = "Sal peers through the crack of the bathroom door into the dark.";
        Areas["dream5"] = "A dark hallway that crumbles away into nothing at its edges.";
        AfterLines[1310] = "Charley takes hold of a zipper that runs down the middle of his face, and slowly unzips it.";
        // ---- Addison Apartments, winter
        Areas["02_402/NavCam 1"] = "Apartment 402, unpacked now. The kitchen with its checkered floor and cat bowls on the left. " +
                                   "Then the living room: a brown couch, where Gizmo lounges, a cat scratching post, an armchair " +
                                   "and side tables. Along the wall: the bathroom door, the green front door, Sal's room and dad's room.";
        Areas["02_402/salsRoomCam"] = "Sal's room, settled in: posters on the walls, Vampire Hunter B, Twine Parks, Edward " +
                                      "Shovelhands and a band poster, a small TV, an amp and an electric guitar, a cat bed, " +
                                      "a dresser, and a bed with a green blanket beside a nightstand.";
        Areas["02_402/dadsRoomCam"] = "Dad's room: a made bed, a small table with a radio, and dad hunched at his desk, " +
                                      "typing at an old computer under a lamp.";
        Areas["02_402/bathroomCam"] = "The green tiled bathroom, with Gizmo's litter box on the floor.";
        Areas["02_504/NavCam 1"] = "Apartment 504: dark and filthy, stained wallpaper, a boarded-up window, old newspapers " +
                                   "piled on the floor. Dark handprints mark the bathroom door.";
        Areas["02_504/bathroomCam"] = "The mouldy green bathroom: a stained bathtub, a sink, a filthy toilet with a plunger " +
                                      "and a bottle on the floor.";
        Areas["02_Fbr1/NavCam 1"] = "Larry and Lisa's basement apartment: the dark kitchen on the left, then the living room with " +
                                    "its sunset painting, family photos, a wooden chest, a blue couch and a bookshelf. Doors: " +
                                    "the bathroom, the front door, Larry's room with its Keep Out sign, and Lisa's room.";
        Areas["02_Fbr1/larrysRoomCam"] = "Larry's room: a staircase up on the left, a coat stand, walls covered in horror and metal " +
                                         "posters, the stereo with a stack of tapes, a bean bag, a heap of clothes, a dresser with " +
                                         "a mirror and the police scanner, paints on a shelf, and an easel holding Larry's painting " +
                                         "of a ghastly face with glowing green eyes. An old TV with a game console in the corner.";
        Areas["02_Fbr1/lisasRoomCam"] = "Lisa's room: Lisa lies in bed under a blue blanket, looking pale and tired, used tissues " +
                                        "scattered on the floor by the nightstand.";
        Areas["02_outBack"] = "Behind the building, in deep snow. A grey back door with trash cans beside it, then a wide snowy " +
                              "field stretching left under a pale winter sun, to a big tree with wooden planks nailed up its trunk " +
                              "and a treehouse in its branches.";
        Areas["02_treeHouse"] = "Inside the treehouse: wooden plank walls, the tree's trunk growing up through the floor, a radio and " +
                                "a chair by the window, a shelf of tools, a tall grey cabinet, a small chest, and a framed picture " +
                                "on the wall. Larry's old hideout.";
        Cutscenes["02_treeHouse/picture: Use"] = "A family photo: a bald man with a big moustache, a young Larry in a backwards cap, " +
                                                 "and Lisa, all smiling.";
        Cutscenes["02_treeHouse/chest: Use"] = "The chest is full of old things: Nature Life magazines, comics, clothes, a rubik's cube, " +
                                               "a toy, and a little metal box.";
        Areas["02_102"] = "Apartment 102, Miss Rosenberg's: an old lady with glasses and grey hair sits in a rocking chair. " +
                          "A tall wooden cabinet full of goblets and china plates, round plaques with cat silhouettes on the " +
                          "wall, an ornate golden urn on a round table, and flowery armchairs.";
        Areas["02_202/NavCam 1"] = "Apartment 202, Todd's family's place: the kitchen on the left, then a tidy living room with a " +
                                   "hookah on a side table, a tan couch, a colourful round rug, a record player, potted plants, " +
                                   "a green couch and lots of family pictures. Doors: the bathroom, the front door, Todd's parents' " +
                                   "room, and Todd's room with a name plate reading Todd.";
        Areas["02_202/ToddsRoomCam"] = "Todd's room: a fish tank labelled Bob, a printer, filing cabinets, and a desk with an old " +
                                       "computer, more computer parts, books and an alien poster. A neatly made bed.";
        Areas["02_202/bathroomCam"] = "A clean blue-green bathroom with a pink towel and a bath mat.";
        Cutscenes["02_202/ashAndToddIntro"] = "A girl comes in: long reddish-brown hair, green eyes, pink lipstick and a black choker. Ash.";
        Lines[1858] = "Todd's screen shows a news article headlined Local Murderer Receives Life Sentence, with two mugshots " +
                      "captioned Charley Mansfield, age 41. The man in them is thin and blond, nothing like the Charley Sal knows.";
        Lines[1873] = "A photo of a little girl with messy purple hair and a gold cross necklace, smiling. The caption reads Megan Holmes.";
        Areas["02_204"] = "Apartment 204, Charley's old place, now empty and dim: bare walls and carpet, the green front door, " +
                          "and the kitchen at the far end.";
        Cutscenes["02_204/sealFound"] = "On the bare wall, a strange symbol glows into being, then fades.";
        Areas["02_301/NavCam 1"] = "Apartment 301, the college students' place: the kitchen, where a girl with curly dark hair and " +
                                   "a green scarf stands by the fridge. A box of junk by the door, a banner reading IGG, stereo " +
                                   "speakers, a guy with a beanie in a wheelchair, a folding table and stacks of folding chairs, " +
                                   "and a girl in a red hoodie reading a book.";
        Areas["02_301/bathroomCam"] = "A tidy pale blue bathroom.";
        Areas["02_302"] = "Apartment 302, David and Sara's: grubby and cluttered. A corkboard covered in newspaper clippings, a mini " +
                          "fridge, a sagging wall shelf with a photo and a box, a worn red couch with a mannequin bust sitting " +
                          "on it, and a picture of a cloud by the door. David, a skinny man in a vest and underwear, stands around.";
        Areas["02_403"] = "Apartment 403, cleaned out and empty now: bare green walls and carpet in dim light, the door hanging " +
                          "slightly open.";
        Areas["02_502"] = "Apartment 502: dark and empty, a boarded-up window, and a broken birdcage lying on its side on the floor.";
        Cutscenes["02_502/sealSummon"] = "By the broken birdcage, a strange symbol glows into being, then fades.";
        Areas["02_503"] = "Apartment 503, dark and wrecked: peeling wallpaper, a big hole broken through the wall from 504, a broken " +
                          "shelf, a torn mattress against the wall, and in the darkness to the left, a noose hanging from the ceiling.";
        Areas["02_therapy1"] = "Dr. Enon's office.";
        Cutscenes["02_therapy1/sessionStart"] = "Dr. Enon's office. Later, the door opens: two prison guards lead Sal out, in his " +
                                                "orange jumpsuit, hands cuffed.";
        Cutscenes["02_drive/disablePlayerUpDown"] = "A long drive through the night.";
        Areas["02_outBack 2"] = "Behind Addison Apartments, years later, at dusk. The field is dead and dark, strewn with trash. " +
                                "The building's back wall is covered in graffiti, an SF symbol among it, and the back door is " +
                                "boarded shut. Far to the left, the old tree with planks nailed up its trunk.";
        Areas["02_treeHouse 2"] = "The treehouse, abandoned for years: dust and cobwebs, the old radio and chair, boxes, a broken poster, " +
                                  "shelves of junk, a faded photograph on the wall, paintings scattered on the floor, and a dark stain " +
                                  "on the rug. A faint symbol glows on the grey cabinet. Dr. Enon, in shirt and tie, looks around.";
        Visuals["anim:larryGhostUp"] = "The ghost of Larry rises up in front of Dr. Enon, pale and flickering.";
        Visuals["anim:larryGhostDown"] = "Larry's ghost fades away.";
        Visuals["anim:enonFall"] = "Dr. Enon staggers back, through the trapdoor, and falls.";
        AfterLines[2137] = "Moments later: Dr. Enon lies sprawled on the ground at the foot of the tree, glasses knocked off, " +
                           "blood pooling under his head.";
        Cutscenes["02_Fbr1/fragFound"] ="In the dark kitchen, a hooded shadow flickers into view.";
        Cutscenes["02_F5/fragFoundF5"] ="The view shifts to the far end of the hallway. Something stirs by the door of 501.";
        Cutscenes["dream5/End"] ="Sal wakes up in bed in the dark, his mask on the nightstand. Outside, snow falls on " +
                                  "Addison Apartments.";
    }
}
