using SallyFaceAccess;
using Xunit;

public class HotspotNameTests
{
    // Real object name / label pairs from Episode 1's opening scenes.
    [Theory]
    [InlineData("dadHS", "talk", "dad, talk")]
    [InlineData("phoneSpot", "answer phone", "answer phone")]
    [InlineData("groceryBagsHS", "bags", "bags")]
    [InlineData("gearBoyHS", "Gear Boy", "Gear Boy")]
    [InlineData("Open Door (2)", "living room", "door to living room")]
    [InlineData("Open Door", "leave", "door, leave")]
    [InlineData("openDoor", "open", "door, open")]
    [InlineData("pickUpKey01", "???", "key")]
    [InlineData("momHS", "...", "mom")]
    [InlineData("codeHS", "passcode", "passcode")]
    [InlineData("sleepyDad", "check", "sleepy dad, check")]
    [InlineData("lightsOn", "light switch", "light switch")]
    [InlineData("graveStone", "climb down", "grave stone, climb down")]
    [InlineData("dogHotspot", "Speak", "dog, Speak")]
    [InlineData("pushDrawer", "push", "drawer, push")]
    [InlineData("doorD", "open", "door D, open")]
    public void NamesFromObjectAndLabel(string objectName, string label, string expected) =>
        Assert.Equal(expected, WorldText.HotspotName(null, objectName, label));

    [Theory]
    [InlineData("Hotspot")]
    [InlineData("Hotspot (3)")]
    [InlineData("GameObject")]
    public void GenericObjectNamesAreIgnored(string objectName) =>
        Assert.Equal("open", WorldText.HotspotName(null, objectName, "open"));

    [Fact]
    public void TableNameKeepsAVerbPrompt() =>
        Assert.Equal("Apartment 401 door, open", WorldText.HotspotName("Apartment 401 door", "doorA", "open"));

    [Fact]
    public void TableNameReplacesANounLabel() =>
        Assert.Equal("Sal's bed", WorldText.HotspotName("Sal's bed", "bedHS", "bed"));

    [Fact]
    public void NothingKnownGivesEmpty() =>
        Assert.Equal("", WorldText.HotspotName(null, "", ""));
}

public class HallwayDoorTests
{
    [Theory]
    [InlineData("F4", "Knock Door", -19.8f, "apartment 401")]
    [InlineData("F4", "Open 402", -12.2f, "apartment 402")]
    [InlineData("F4", "Open 403", 3.1f, "apartment 403")]
    [InlineData("F5", "open504", 10.9f, "apartment 504")]
    [InlineData("02_F5", "", -12.1f, "apartment 502")]
    [InlineData("04_os_F3", "301", -19.8f, "apartment 301")]
    [InlineData("02_F2", "door 204 - open", 10.7f, "apartment 204")]
    public void DoorsAreNumberedByFloorAndPosition(string scene, string obj, float x, string expected) =>
        Assert.Equal(expected, WorldText.HallwayDoor(scene, obj, x));

    [Theory]
    [InlineData("F3", "chugHS", 4.2f)]        // a character standing by a door
    [InlineData("F4", "elevator", -4.5f)]
    [InlineData("F4r2", "Open Door", 12.8f)]  // not a hallway
    [InlineData("03_F1", "mailboxes", 23.4f)]
    public void OtherThingsAreNotDoors(string scene, string obj, float x) =>
        Assert.Null(WorldText.HallwayDoor(scene, obj, x));

    [Theory]
    [InlineData("apartment 402", "Open 402", "Sal's Place", "apartment 402, Sal's Place")]
    [InlineData("apartment 401", "Knock Door", "knock", "apartment 401, knock")]
    [InlineData("apartment 104", "104", "104", "apartment 104")]
    public void TableNameKeepsALabelThatAddsSomething(string table, string obj, string label, string expected) =>
        Assert.Equal(expected, WorldText.HotspotName(table, obj, label));
}

public class NumberedTests
{
    [Fact]
    public void NumberGoesAfterTheNameBeforeThePrompt() =>
        Assert.Equal("door 2, open", WorldText.Numbered("door, open", 2));

    [Fact]
    public void NameWithoutPromptGetsNumberAtEnd() =>
        Assert.Equal("bags 3", WorldText.Numbered("bags", 3));
}

public class RoomNameTests
{
    [Theory]
    [InlineData("salsRoomCam", "sals room")]
    [InlineData("dadsRoomCam", "dads room")]
    [InlineData("bathroomCam", "bathroom")]
    [InlineData("NavCam 1", null)]
    [InlineData("navCam1", null)]
    [InlineData("GameCamera2D", null)]
    public void FromCameraName(string camera, string expected) =>
        Assert.Equal(expected, WorldText.RoomFromCamera(camera));
}

public class DirectionTests
{
    [Fact]
    public void RightWithSteps() => Assert.Equal("right, 3 steps", WorldText.Direction(3.2f, 1f));

    [Fact]
    public void LeftWithOneStep() => Assert.Equal("left, 1 step", WorldText.Direction(-1.4f, 1f));

    [Fact]
    public void WithinHalfAStepIsHere() => Assert.Equal("here", WorldText.Direction(0.4f, 1f));

    [Fact]
    public void StepSizeScales() => Assert.Equal("right, 2 steps", WorldText.Direction(4f, 2f));
}

public class SceneNameTests
{
    [Theory]
    [InlineData("F4", "floor 4 hallway")]
    [InlineData("02_F1", "floor 1 hallway")]
    [InlineData("Fb", "basement")]
    [InlineData("03_Fbr2", "basement room 2")]
    [InlineData("F4r2", "floor 4 room 2")]
    [InlineData("02_402", "apartment 402")]
    [InlineData("04_os_402", "apartment 402")]
    [InlineData("03_schoolHall", "school hall")]
    [InlineData("02_treeHouse 2", "tree house")]
    [InlineData("05_house1_neilsRoom", "house neils room")]
    public void Humanize(string scene, string expected) =>
        Assert.Equal(expected, WorldText.SceneName(scene));
}
