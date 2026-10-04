namespace SallyFaceAccess;

/// <summary>
/// Hand-written descriptions of what is only shown, never said - written from screenshots
/// of the game (CaptureCutscenes). They describe what a sighted player sees, clues included,
/// and nothing more. Mod wording, English for now.
///
///   Areas      "scene" or "scene/camera" - spoken on the first visit, and on F6
///   Cutscenes  "scene/ActionList name"   - spoken when that cutscene starts
///   Lines      speech line ID            - spoken just before that line
/// </summary>
internal static class Descriptions
{
    private static readonly Dictionary<string, string> Areas = new()
    {
        ["lvl00"] = "A hospital room, dark until the light is switched on. A door and a light switch on the left. " +
                    "On the wall, a framed painting of a white dog. A TV, a chest of drawers, and a phone on a small table. " +
                    "Further right, an empty hospital bed with a bloodstained pillow, an IV stand and a radiator.",
        ["lvl01"] = "A long, filthy hospital corridor lined with green doors. Each door has a large letter on a plate.",
        ["lvl02"] = "A graveyard at night, in pouring rain. Mourners in black huddle under an umbrella on the left. " +
                    "On the right stands a big bare tree with the number 6 scratched into its trunk, " +
                    "and a dark dog with glowing yellow eyes lies beneath it.",
        ["lvl03"] = "A crumbling corridor. A green door with a battery compartment, labelled: Batteries NOT included. " +
                    "A keypad is mounted on the wall to its right.",
        ["lvl04"] = "A bare, decaying room with holes in the plaster and papers scattered over the floor. " +
                    "Sal's dad stands with his head bowed.",
        ["t01"] = "Dr. Enon's office. Sal, older now, with long blue hair, sits on a couch facing Dr. Enon, " +
                  "a man with a moustache and round glasses who takes notes on a clipboard. " +
                  "On the table between them, two mugs and Sal's white prosthetic mask.",
        ["F4r2/NavCam 1"] = "The living room of the new apartment: yellow wallpaper, green carpet, " +
                            "and moving boxes stacked everywhere.",
        ["F4r2/salsRoomCam"] = "Sal's room: a bare mattress on the floor beside a bed frame not yet put together, " +
                               "a dresser, and more boxes. Gizmo the cat is curled up asleep on the mattress.",
        ["F4r2/bathroomCam"] = "A small bathroom with green tiles: a bathtub with a shower curtain, a sink and mirror, " +
                               "a toilet with a bottle of sleep aid on the tank.",
    };

    private static readonly Dictionary<string, string> Cutscenes = new()
    {
        ["lvl00/turnOnLights"] = "The lights flicker on.",
        ["lvl02/dogSpeakTo"] = "The camera closes in on Sal and the dog. The dog stares up at Sal with glowing yellow eyes.",
        ["lvl02/graveClimb"] = "An open grave lies before a headstone that reads: Diane Fisher, beloved mother and wife.",
        ["lvl04/momHS: Use"] = "A morgue: a wall of metal body drawers, and a body under a sheet on a gurney.",
    };

    private static readonly Dictionary<int, string> Lines = new()
    {
        [84] = "Darkness.",
        [137] = "Close up: under the sheet lies a pale, lifeless woman with long blonde hair, eyes closed.",
        [148] = "A rainy evening. A tall brick apartment building surrounded by pine trees, " +
                "a sign out front reading Addison Apartments.",
        [174] = "Sal, a small kid with blue pigtails and a white prosthetic mask, stands among moving boxes " +
                "next to Dad, a tall man with a blue beard.",
    };

    public static string Area(string scene, string camera) =>
        Areas.TryGetValue($"{scene}/{camera}", out var d) ? d
        : Areas.TryGetValue(scene, out d) ? d : null;

    public static string Cutscene(string scene, string list) =>
        Cutscenes.TryGetValue($"{scene}/{list}", out var d) ? d : null;

    public static string Line(int lineID) => Lines.TryGetValue(lineID, out var d) ? d : null;
}
