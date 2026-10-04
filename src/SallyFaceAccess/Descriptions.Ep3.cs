namespace SallyFaceAccess;

/// <summary>Episode 3: The Bologna Incident - the White Room, Nockfell High, the apartments in spring, the cult.</summary>
internal static partial class Descriptions
{
    static partial void Episode3()
    {
        Areas["03_clumpy"] = "Clumpy, a game on Larry's TV: a pudgy winged creature with a grin, carrying a spear, flies through " +
                             "a green cavern over a river of slime, dodging flying bugs.";

        // ---- The White Room and the interview
        Areas["03_intro"] = "An endless white emptiness, then a pale, misty space with a tiled floor broken by jagged holes. " +
                            "Lever machines with up and down buttons stand along the way, and far ahead, a lone white door.";
        AfterLines[2517] = "A huge clawed hand bursts up out of a hole in the floor.";
        Areas["03_intro2"] = "The pale tiled space again, now lined with panels of numbered buttons.";
        Cutscenes["03_intro3/disablePlayerUpDown"] = "A press interview. A reporter in a suit, with long hair, sits at a table, a " +
            "cameraman filming behind. Across the table: adult Sal, in an orange jumpsuit marked Inmate 081486, guards standing " +
            "behind him, a Faux News microphone in front of him.";
        Areas["03_doodles"] = "A world drawn on lined notebook paper. Sal is a stick figure. A line of handwriting floats above: " +
                              "What if I were more than just a doodle? Other doodles stand about: triangle-headed stick men, " +
                              "sad square-headed figures, a scribbled monster with a mouth full of teeth. Ladders drawn in pencil " +
                              "connect the lines. Further down, crumpled paper, a giant pink eraser, and a folded paper fortune teller.";

        // ---- Nockfell High School
        Cutscenes["03_schoolPackertonIntro/End"] = "Nockfell High School: a red brick building with a flagpole out front, " +
                                                   "under a grey spring sky.";
        Cutscenes["03_schoolPackertonIntro/HallwayScene"] = "A school hallway lined with brown lockers. Travis, a blond kid in a " +
                                                            "pink shirt, blocks the way. Ash stands nearby.";
        Lines[2671] = "Travis punches Sal in the face, knocking him back.";
        Lines[2681] = "Ash gently lifts the edge of Sal's mask and dabs the cut with her red bandanna.";
        Areas["03_schoolHall"] = "The main hallway of Nockfell High: cream walls, speckled floor, fire alarm bells and trash cans. " +
            "Left to right: the front entrance doors under an Exit sign, room 101, a bulletin board crowded with flyers, " +
            "a pep rally banner, missing posters, talent show and football tryout notices, room 102, double doors to the " +
            "courtyard, the cafeteria doors, the boys' and girls' bathrooms with a water fountain between them, rows of " +
            "brown lockers, the cafeteria again, rooms 103 and 104, Mrs. Packerton's classroom. Round the corner, a second " +
            "hall: the science lab, 201, room 202, more courtyard doors, and a closet.";
        Shots["03_schoolHall/lockerCam_Sal"] = "Sal's locker: a Physics 101 textbook and another book, a blue backpack, and a photo " +
                                               "of Sal with friends taped inside the door.";
        Shots["03_schoolHall/lockerCam_Larry"] = "Larry's locker: a red backpack covered in band patches, a can of pencils, papers, " +
                                                 "and the door plastered with stickers: Immure, biohazard signs, I love metal, and a " +
                                                 "photo of the gang.";
        Shots["03_schoolHall/lockerCam_Todd"] = "Todd's locker, neatly organised: a little lamp, a small set of drawers, jars, " +
                                                "engineering and computer science books, a green thermos, and a photo of Todd and Neil.";
        Shots["03_schoolHall/lockerCam_Ash"] = "Ash's locker: star stickers, a Clumpy poster, a mirror, headphones, a polaroid camera, " +
                                               "stacked books, and polaroids of her friends.";
        Areas["03_schoolCafeteria"] = "The cafeteria: long tables full of students under a Nockfell Warriors football banner. Maple, " +
            "a girl with grey hair, sits with Chug. Travis sits further along. At the far end, the serving counter with trays and " +
            "a register, and Kim the lunch lady behind it, one side of her face heavily swollen.";
        Areas["03_schoolBathroomBoys"] = "The boys' bathroom: cream tiles, a row of sinks under mirrors, a row of urinals, and three " +
                                         "stalls at the far end.";
        Areas["03_schoolPackerton"] = "Mrs. Packerton's classroom: rows of blue desks, the teacher's desk at the front, a chalkboard " +
            "with an algebra problem worked through, 4 brackets x plus 2, plus 5x, equals 6 brackets x plus 4, ending x equals " +
            "16 over 3, and a note: Final exam, 5/23, 12:45 pm. A banner of the digits of pi above the board, and a multiplication chart.";
        Areas["03_schoolScienceLab"] = "The science lab: lab benches with sinks and microscopes, a periodic table, a portrait of " +
                                       "Einstein, and a poster of the evolution of man. Todd works at a bench.";
        Areas["03_schoolCourtyard"] = "The school courtyard: grass, benches along the red brick walls, small trees, a big tree near " +
                                      "the doors, a trash can, and a window on the right. Birds chirp in the leaves.";
        Shots["03_schoolCourtyard/birdCam"] = "Up in the tree: thick green leaves against a blue sky.";
        // ---- Apartment 201, the Packertons'
        Areas["03_201/NavCam 1"] = "Apartment 201, the Packertons': the kitchen with its checkered floor, then a living room with a " +
            "worn orange couch, a framed portrait, a painting of a lightning-struck tree, a big painting of a red barn on a hill, " +
            "an old radio on a cabinet, stacks of newspapers, and packs of HugGems adult diapers. A grandfather clock hangs " +
            "between the two bedroom doors.";
        Areas["03_201/bathroomCam"] = "A grimy green bathroom, a green stain in the bathtub.";
        Areas["03_201/bbr1_Cam"] = "Bedroom 1: an emaciated old man, Mr. Packerton, is strapped by his wrists and ankles to a stained " +
            "hospital bed, wearing only a diaper and a breathing mask. An IV drip, an oxygen tank, a tall rack of machines with a " +
            "heart monitor, an overflowing trash can, and newspaper articles pinned to the wall.";
        Areas["03_201/br2_Cam"] = "Bedroom 2: blood-spattered chest freezers, a blood-stained workbench, saws, cleavers, knives and " +
            "pliers hanging on the wall, an iron furnace with a grate and black bags piled beneath it, melted candles, jars, and " +
            "a wall covered in MISSING posters, scribbled notes and a symbol.";
        Shots["03_201/goatHeadCam"] = "Inside the fridge: a severed goat's head with curling horns and a staring yellow eye, a " +
                                      "numbered tag in its ear, lying in blood. Pentagrams are drawn on the fridge's inner walls.";
        Shots["03_201/freezerCam (1)"] = "Inside the freezer: human remains. Severed faces, hands and organs, heaped and soaked in blood.";
        Shots["03_201/freezerCam"] = "Larry, Sal and Ash stare down into the freezer in horror, Larry clutching his mouth.";
        Shots["03_201/hidieCam1"] = "Larry and Sal crouch hidden in a dark corner.";
        Shots["03_201/hidieCam2"] = "Larry throws an arm out across Sal, shielding him, as Sal hugs his knees.";
        Shots["03_201/trashCam"] = "A dark, stained chute, dropping away into blackness.";
        Visuals["anim:br1_machineBeating"] = "The heart monitor beeps steadily.";
        Visuals["anim:br1_machineFlatline"] = "The heart monitor flatlines with a long, steady tone.";
        Areas["03_102"] = "Apartment 102, Miss Rosenberg's: the rocking chair, the cabinet of goblets and china, cat plaques on the walls.";
        Areas["03_204"] = "Apartment 204, empty and dim.";
        Areas["03_401"] = "Apartment 401, Robert's: walls covered in punk and horror posters, a skull with a green mohawk, Evil " +
                          "Bread, an astronaut, a monster. A lamp on a chair, a battered dresser with clothes hanging out.";
        Shots["03_401/robCam"] = "Robert, a punk with a tall red mohawk, sits hunched on a bare mattress beside a boombox and " +
                                 "scattered CDs.";
        Areas["03_402/NavCam 1"] = "Apartment 402: the kitchen with a note on the fridge, the living room, and the doors to the " +
                                   "bathroom, the hallway, Sal's room and dad's room.";
        Areas["03_402/salsRoomCam"] = "Sal's room: the posters, the guitar and amp, the cat bed, the dresser, and the green bed. " +
                                      "A tea cup and the Super Gear Boy lie nearby.";
        Areas["03_402/dadsRoomCam"] = "Dad's room: the bed, and dad's desk with his computer, its screen on. A photo of mom stands " +
                                      "on the desk.";
        Areas["03_501"] = "Apartment 501: dark, with stained wallpaper. Old newspaper pages are pinned to the wall.";
        Areas["03_502"] = "Apartment 502: dark and empty but for the old broken birdcage on the floor.";
        Areas["03_504/NavCam 1"] = "Apartment 504: dark and filthy. A Ouija board lies on the floor.";
        Areas["03_504/bathroomCam"] = "The mouldy bathroom. A necklace hangs from the towel rail.";
        Areas["03_Fbr2/NavCam 1"] = "A hidden apartment behind the basement shelf, long abandoned: stained wallpaper, mouldy carpet, " +
                                    "doors to a bathroom and two bedrooms.";
        Areas["03_Fbr2/bathroomCam"] = "A filthy, abandoned bathroom, the toilet leaking onto the floor.";
        Areas["03_Fbr2/salsRoomCam"] = "An empty bedroom with bare walls.";
        Areas["03_Fbr2/dadsRoomCam"] = "An empty bedroom. Along the floor, a jagged tear in the carpet shows wooden boards beneath.";
        Shots["03_Fbr2/doorFoundCam"] = "A jagged tear in the carpet, floorboards showing through.";
        Shots["03_Fbr2/stairCam1"] = "Under the carpet, a hatch opens onto steep wooden stairs, descending into darkness.";
        // ---- The cult tunnels
        Areas["03_cult_Main"] = "An underground temple of old stone: arched walls, pillars, iron spikes along the edges of the floor, " +
            "trident-topped torch stands, and spiked chandeliers. In the centre, a huge iron portcullis between carved demonic " +
            "statues wrapped in serpents, beneath an inscription: May our sight pierce the heavens and our reach be infinite and " +
            "everlasting. A great pentagram is set into the floor before it, flanked by triangular stone markers. Stone pedestals " +
            "carved with red symbols hold books, and wooden doors lead off to the halls on either side.";
        Shots["03_cult_Main/book1Cam"] = "A stone pedestal carved with red symbols, an old book lying open on top.";
        Shots["03_cult_Main/book2Cam"] = "A stone pedestal carved with red symbols, an old book lying open on top.";
        Areas["03_cult_L1"] = "A maze of stone halls, three rows high, each lined with arched wooden doors.";
        Areas["03_cult_S1"] = "A maze of stone halls, three rows high, each lined with arched wooden doors.";
        Areas["03_cult_L2"] = "A stone chamber with a barred iron gate, a field of floor spikes before it, and pressure plates in the floor.";
        Areas["03_cult_S2"] = "A stone chamber: chandeliers, a pedestal with a book, and a barred gate. In a side room, a skeleton " +
                              "lies in a heap on the floor.";
        Visuals["anim:cultUp"] = "The ghost of a hooded cultist rises before Sal.";
        Visuals["anim:cultistGhost_Down"] = "The cultist's ghost sinks away.";
        Areas["03_cult_Final"] = "The temple's main hall again, the pentagram in the floor and the great portcullis.";
        Areas["03_cult_Center"] = "The inner sanctum: an archway carved with serpents and demon faces, inscribed For we are the " +
            "devourers of god. Huge mounds of human bones and skulls are heaped against the walls, blood running down from a " +
            "window above them.";
        Shots["03_cult_Center/ashCam1"] = "On top of a mound of bones and skulls lies Ash, limp and unconscious.";
        Cutscenes["03_mediaEnd/endingCutScene"] = "The interview again. Then: Sal's cuffed hands hold a copy of the Nockfell News, " +
            "headlined The Sally Face Killer, Trial Today for Mass Murder, Including Entire Family, with a photo of Ash and the " +
            "words: the prosecutor will call Ashley Campbell to testify. Sal crumples the paper and tears it apart. " +
            "Then a hospital corridor, a padded cell, and a man with red hair and beard in a straitjacket, sitting slumped. " +
            "He looks up: his eyes are glowing red.";
        Cutscenes["03_mediaBreak/disablePlayerUpDown"] = "Back at the press interview: the reporter, and adult Sal in his orange jumpsuit.";
        Areas["03_Fbr1/NavCam 1"] = "Larry and Lisa's apartment in spring: the kitchen, where Lisa is, the living room with a vase " +
                                    "of flowers, family photos and the couch, and the doors to the bathroom, the hallway, Larry's " +
                                    "room and Lisa's room.";
        Areas["03_Fbr1/larrysRoomCam"] = "Larry's room: the posters, the stereo, the easel, and the old TV in the corner.";
        Areas["03_Fb"] = "The basement: the door to Larry's apartment, the vending machine, the elevator, and a tall supply shelf.";
        Areas["03_302"] = "Apartment 302, David's: the cluttered room with the red couch and the mannequin bust.";
        Areas["03_304"] = "Apartment 304: dim and empty.";
        Areas["03_403"] = "Apartment 403, empty and dim. A letter lies on the floor.";
        Cutscenes["03_schoolCafeteriaIntro/disablePlayerUpDown"] = "The school cafeteria at lunch, noisy with chatter. Lunch ladies " +
            "serve from behind the counter, slapping slices of grey bologna onto trays.";
    }
}
