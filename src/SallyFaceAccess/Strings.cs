namespace SallyFaceAccess;

/// <summary>
/// Every piece of wording the mod speaks on its own behalf. Game text is read in the
/// game's language; these are English for now and kept here so they can be translated.
/// </summary>
internal static class Strings
{
    public const string Loaded = "Sally Face Access loaded";
    public const string DebugSpeechOn = "Unhandled text speech on";
    public const string DebugSpeechOff = "Unhandled text speech off";
    public const string DumpWritten = "Text dump written";
    public const string UiDumpWritten = "Screen dump written";
    public const string Percent = "{0} percent";
    public const string Back = "back";
    public const string Close = "close";
    public const string Continue = "continue";
    public const string NextPage = "next page";
    public const string PreviousPage = "previous page";
    public const string Empty = "empty";
    public const string Position = "{0} of {1}";
    public const string Holding = "holding {0}";
    public const string PutAway = "put away {0}";
    public const string Got = "got {0}";
    public const string Removed = "{0} removed";
    public const string Here = "here";
    public const string DoorTo = "door to {0}";
    public const string InventoryEmpty = "inventory empty";
    public const string InventoryOne = "inventory, 1 item";
    public const string InventoryMany = "inventory, {0} items";
    public const string Goal = "goal: {0}";
    public const string NewGoal = "new goal: {0}";
    public const string NoGoal = "no goal yet";
    public const string HintSubtle = "something new to try here";
    public const string HintFull = "try the {0} here";
    public const string And = " and ";
    public const string WayTo = "way to {0}";
    public const string SomethingHappens = "something happens here";
    public const string Edge = "edge";
    public const string TourDone = "tour complete";
    public const string NoDescription = "no description for this place yet";
    public const string Left = "left";
    public const string Right = "right";
    public const string OneStep = "1 step";
    public const string Steps = "{0} steps";
    public const string NothingHere = "nothing to interact with here";
    public const string RoomSummary = "{0}, {1} things";
    public const string FloorHallway = "floor {0} hallway";
    public const string Basement = "basement";
    public const string BasementRoom = "basement room {0}";
    public const string FloorRoom = "floor {0} room {1}";
    public const string Apartment = "apartment {0}";
    public const string Static = "static";
    public const string ItemOfCount = "{0}, {1} of {2}";
    public const string PreviousStation = "previous station";
    public const string NextStation = "next station";

    // The Super Gear Boy detector.
    public const string DetectorUp = "Gear Boy up";
    public const string DetectorDown = "Gear Boy down";
    public const string DetectorGlow = "Gear Boy glowing, press R";
    public const string DetectorFade = "glow fades";
    public const string HotSpot = "supernatural hot spot";
    public const string DeadstoneSpot = "Deadstone spot";
    public const string PonySpot = "dark pony spot";

    // Top-down handheld games.
    public const string Up = "up";
    public const string Down = "down";
    public const string Open = "open";
    public const string Walls = "up {0}, down {1}, left {2}, right {3}";
    public const string HitSpikes = "spikes! back to the start";
    public const string HitElectric = "shocked! back to the start";
    public const string HitCultist = "caught! back to the start";
    public const string TimesUp = "time's up, back to the start";
    public const string Passage = "passage";
    public const string AudioOn = "positional audio on";
    public const string AudioOff = "positional audio off";

    // Puzzles read from game variables.
    public const string SealState = "seal: right lever ring {0}, left lever ring {1}, of 4. Line both up at 1";
    public const string SealSolved = "seal lined up";
    public const string SwitchesDown = "switches down: {0}";
    public const string SwitchesNone = "no switches down";
    public const string SwitchesAll = "all switches down";
    public const string FloorSwitch = "floor switch {0}";
    public const string SkipAvailable = "skip puzzle available, press Delete";
    public const string PuzzleSkipped = "puzzle skipped";
    public const string GateAlreadyOpen = "the gate is already open";
    public const string Dial = "dial {0}: {1}";
    public const string Tooth = "tooth {0}";
    public const string Hexagon = "{0} hexagon";
    public const string NothingEntered = "nothing entered";
    public const string ChoosePyramid = "choose a pyramid";
    public const string ThreeDArea = "a 3D area: up and down arrows walk away from and toward the camera. Press End to use the object you last scanned";
    public const string ScanFirst = "scan to an object first, then press End";
    public const string Upright = "upright";
    public const string TurnToUpright = "{0} turn to upright";
    public const string TurnsToUpright = "{0} turns to upright";
    public const string AllDialsUpright = "all three dials upright";
    public const string Middle = "middle";
    public const string Entered = "entered: {0}";
    public const string Blocked = "blocked";
    public const string WayBlocked = "{0} blocked";
    public const string WayClear = "{0} clear";
    public const string GetOut = "get out, {0} seconds";
    public const string Autoplaying = "autoplaying: planting the C-4";
}
