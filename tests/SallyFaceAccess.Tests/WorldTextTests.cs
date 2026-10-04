using SallyFaceAccess;
using Xunit;

public class HotspotNameTests
{
    [Fact]
    public void TableNameComesFirstThenVerb() =>
        Assert.Equal("Apartment 401 door, open", WorldText.HotspotName("Apartment 401 door", "door401", "open"));

    [Fact]
    public void InformativeObjectNameIsUsedWhenNoTableName() =>
        Assert.Equal("light switch, push", WorldText.HotspotName(null, "lightSwitch", "push"));

    [Theory]
    [InlineData("Hotspot")]
    [InlineData("Hotspot (3)")]
    [InlineData("HS_2")]
    [InlineData("GameObject")]
    public void GenericObjectNamesAreIgnored(string objectName) =>
        Assert.Equal("open", WorldText.HotspotName(null, objectName, "open"));

    [Fact]
    public void VerbIsNotRepeatedWhenItMatchesTheName() =>
        Assert.Equal("light switch", WorldText.HotspotName(null, "light switch", "Light Switch"));

    [Fact]
    public void QuestionMarkVerbIsDropped() =>
        Assert.Equal("poster", WorldText.HotspotName("poster", "x", "???"));

    [Fact]
    public void NothingKnownGivesTheVerb() =>
        Assert.Equal("talk", WorldText.HotspotName(null, null, "talk"));
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
