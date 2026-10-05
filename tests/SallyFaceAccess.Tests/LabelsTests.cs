using SallyFaceAccess;
using Xunit;

public class GenericButtonTests
{
    [Theory]
    [InlineData("Button", true)]
    [InlineData("Button1", true)]
    [InlineData("button 2", true)]
    [InlineData("Left Arrow", false)]
    [InlineData("Close", false)]
    public void Placeholder_names(string title, bool generic) => Assert.Equal(generic, Labels.IsPlaceholder(title));
}

public class LabelsTests
{
    private static string Lookup(string title) => title switch
    {
        "l_ep1" => "Episode One",
        "l_ep1_sn" => "Strange Neighbors",
        "l_ep1%" => "40%",
        "empty" => "",
        _ => null,
    };

    [Fact]
    public void PlainTemplateIsReturnedAsIs() =>
        Assert.Equal("Back", Labels.Expand("Back", Lookup));

    [Fact]
    public void ReferencesAreReplacedByOtherElementsText() =>
        Assert.Equal("Episode One, Strange Neighbors, 40%",
            Labels.Expand("{l_ep1}, {l_ep1_sn}, {l_ep1%}", Lookup));

    [Fact]
    public void MissingOrEmptyReferencesAreDroppedWithTheirSeparator()
    {
        Assert.Equal("Episode One", Labels.Expand("{l_ep1}, {nope}", Lookup));
        Assert.Equal("Episode One", Labels.Expand("{empty}, {l_ep1}", Lookup));
    }

    [Theory]
    [InlineData("b_ep1_x", "close")]
    [InlineData("close button (MM)", "close")]
    [InlineData("back", "back")]
    [InlineData("Left Arrow", "Left Arrow")]
    [InlineData("b-Movie", "Movie")]
    [InlineData("Turn Right - top", "Turn Right top")]
    public void HumanizeMakesInternalNamesReadable(string title, string expected) =>
        Assert.Equal(expected, Labels.Humanize(title));
}
