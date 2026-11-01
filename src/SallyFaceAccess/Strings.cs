namespace SallyFaceAccess;

/// <summary>
/// Every piece of wording the mod speaks on its own behalf. Game text is read in the
/// game's language; these are the English, translated through Loc into the game's language.
/// </summary>
internal static class Strings
{
    public static string Loaded => Loc.T("Sally Face Access loaded");
    public static string DebugSpeechOn => Loc.T("Unhandled text speech on");
    public static string DebugSpeechOff => Loc.T("Unhandled text speech off");
    public static string DumpWritten => Loc.T("Text dump written");
    public static string UiDumpWritten => Loc.T("Screen dump written");
    public static string Percent => Loc.T("{0} percent");
    public static string Back => Loc.T("back");
    public static string Close => Loc.T("close");
    public static string Continue => Loc.T("continue");
    public static string NextPage => Loc.T("next page");
    public static string PreviousPage => Loc.T("previous page");
    public static string Empty => Loc.T("empty");
    public static string Position => Loc.T("{0} of {1}");
    public static string Holding => Loc.T("holding {0}");
    public static string PutAway => Loc.T("put away {0}");
    public static string Got => Loc.T("got {0}");
    public static string Removed => Loc.T("{0} removed");
    public static string Here => Loc.T("here");
    public static string DoorTo => Loc.T("door to {0}");
    public static string TheOtherSide => Loc.T("{0}, the other side");
    public static string InventoryEmpty => Loc.T("inventory empty");
    public static string InventoryOne => Loc.T("inventory, 1 item");
    public static string InventoryMany => Loc.T("inventory, {0} items");
    public static string Goal => Loc.T("goal: {0}");
    public static string NewGoal => Loc.T("new goal: {0}");
    public static string NoGoal => Loc.T("no goal yet");
    public static string HintSubtle => Loc.T("something new to try here");
    public static string HintFull => Loc.T("try the {0} here");
    public static string And => Loc.T(" and ");
    public static string WayTo => Loc.T("way to {0}");
    public static string SomethingHappens => Loc.T("something happens here");
    public static string Edge => Loc.T("edge");
    public static string TourDone => Loc.T("tour complete");
    public static string NoDescription => Loc.T("no description for this place yet");
    public static string Left => Loc.T("left");
    public static string Right => Loc.T("right");
    public static string OneStep => Loc.T("1 step");
    public static string Steps => Loc.T("{0} steps");
    public static string NothingHere => Loc.T("nothing to interact with here");
    public static string RoomSummary => Loc.T("{0}, {1} things");
    public static string FloorHallway => Loc.T("floor {0} hallway");
    public static string Basement => Loc.T("basement");
    public static string BasementRoom => Loc.T("basement room {0}");
    public static string FloorRoom => Loc.T("floor {0} room {1}");
    public static string Apartment => Loc.T("apartment {0}");
    public static string Static => Loc.T("static");
    public static string ItemOfCount => Loc.T("{0}, {1} of {2}");
    public static string PreviousStation => Loc.T("previous station");
    public static string NextStation => Loc.T("next station");

    // The Super Gear Boy detector.
    public static string DetectorUp => Loc.T("Gear Boy up");
    public static string DetectorDown => Loc.T("Gear Boy down");
    public static string DetectorGlow => Loc.T("Gear Boy glowing, press R");
    public static string DetectorFade => Loc.T("glow fades");
    public static string HotSpot => Loc.T("supernatural hot spot");
    public static string DeadstoneSpot => Loc.T("Deadstone spot");
    public static string PonySpot => Loc.T("dark pony spot");

    // Top-down handheld games.
    public static string Up => Loc.T("up");
    public static string Down => Loc.T("down");
    public static string Open => Loc.T("open");
    public static string Walls => Loc.T("up {0}, down {1}, left {2}, right {3}");
    public static string HitSpikes => Loc.T("spikes! back to the start");
    public static string HitElectric => Loc.T("shocked! back to the start");
    public static string HitCultist => Loc.T("caught! back to the start");
    public static string TimesUp => Loc.T("time's up, back to the start");
    public static string Passage => Loc.T("passage");
    public static string AudioOn => Loc.T("positional audio on");
    public static string AudioOff => Loc.T("positional audio off");

    // Puzzles read from game variables.
    public static string SealState => Loc.T("seal: right lever ring {0}, left lever ring {1}, of 4. Line both up at 1");
    public static string SealSolved => Loc.T("seal lined up");
    public static string SwitchesDown => Loc.T("switches down: {0}");
    public static string SwitchesNone => Loc.T("no switches down");
    public static string SwitchesAll => Loc.T("all switches down");
    public static string FloorSwitch => Loc.T("floor switch {0}");
    public static string SkipAvailable => Loc.T("skip puzzle available, press Delete");
    public static string PuzzleSkipped => Loc.T("puzzle skipped");
    public static string GateAlreadyOpen => Loc.T("the gate is already open");
    public static string Dial => Loc.T("dial {0}: {1}");
    public static string Tooth => Loc.T("tooth {0}");
    public static string Hexagon => Loc.T("{0} hexagon");
    public static string NothingEntered => Loc.T("nothing entered");
    public static string ChoosePyramid => Loc.T("choose a pyramid");
    public static string LightsOn => Loc.T("{0} of 3 lights on");
    public static string ArenaKills => Loc.T("{0} of {1}");
    public static string OnYou => Loc.T("on you");
    public static string Middle => Loc.T("middle");
    public static string FightAuto => Loc.T("autoplaying the fight: nothing can hurt Sal now");
    public static string EndlessAuto => Loc.T("autoplaying the fight: the spit can't hurt Sal now");
    public static string UnderMouth => Loc.T("under the {0} mouth");
    public static string UnderDeadMouth => Loc.T("under the dead {0} mouth");
    public static string BetweenMouths => Loc.T("between the {0} and {1} mouths");
    public static string FarLeft => Loc.T("far left");
    public static string FarRight => Loc.T("far right");
    public static string MouthSpits => Loc.T("{0} mouth spits");
    public static string MouthSpitsMove => Loc.T("{0} mouth spits, move!");
    public static string MouthHit => Loc.T("{0} mouth hit");
    public static string MouthDead => Loc.T("{0} mouth dead, {1} left");
    public static string AutoStrikeOn => Loc.T("auto strike on: Sal blasts whatever comes in reach");
    public static string AutoStrikeOff => Loc.T("auto strike off");
    public static string StepsBeside => Loc.T("{0} steps {1} of you");
    public static string ArmRising => Loc.T("{0} arm rising, strikes {1}");
    public static string ArmMove => Loc.T("{0} arm rising, strikes on you, move!");
    public static string ArmDown => Loc.T("{0} arm down {1}, hit it!");
    public static string ArmBack => Loc.T("{0} arm pulls back");
    public static string ArmHit => Loc.T("arm hit, {0}");
    public static string SkipConsole => Loc.T("the control console can be skipped: press Delete");
    public static string PyramidsClose => Loc.T("Backspace closes this.");
    public static string Closed => Loc.T("closed");
    public static string PyramidNotFound => Loc.T("{0}: not found yet, so it has no corners to press");
    public static string ThreeDArea => Loc.T("a 3D area: up and down arrows walk away from and toward the camera. Press End to use the object you last scanned");
    public static string ScanFirst => Loc.T("scan to an object first, then press End");
    public static string Upright => Loc.T("upright");
    public static string TurnToUpright => Loc.T("{0} turn to upright");
    public static string TurnsToUpright => Loc.T("{0} turns to upright");
    public static string AllDialsUpright => Loc.T("all three dials upright");
    public static string Entered => Loc.T("entered: {0}");
    public static string Blocked => Loc.T("blocked");
    public static string WayBlocked => Loc.T("{0} blocked");
    public static string WayClear => Loc.T("{0} clear");
    public static string GetOut => Loc.T("get out, {0} seconds");
    public static string Backwards => Loc.T("written backwards: {0}");
    public static string ConsoleSwitch => Loc.T("switch {0} {1}");
    public static string ConsoleSlider => Loc.T("slider {0} at {1}");
    public static string Autoplaying => Loc.T("autoplaying: planting the C-4");
}
