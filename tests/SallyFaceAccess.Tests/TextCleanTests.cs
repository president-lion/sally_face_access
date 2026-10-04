using SallyFaceAccess;
using Xunit;

public class TextCleanTests
{
    [Theory]
    [InlineData("<b>Hello</b>", "Hello")]
    [InlineData("<color=#FF0000>Red</color> text", "Red text")]
    [InlineData("<size=20>Big</size>", "Big")]
    [InlineData("<sprite name=\"button_a\"> to continue", "to continue")]
    public void StripsRichTextTags(string input, string expected) =>
        Assert.Equal(expected, TextClean.Clean(input));

    [Theory]
    [InlineData("Hey, dad.[wait:1] Dad?", "Hey, dad. Dad?")]
    [InlineData("[expression:Happy]Hi!", "Hi!")]
    [InlineData("Hold on[hold]", "Hold on")]
    public void StripsAdventureCreatorTokens(string input, string expected) =>
        Assert.Equal(expected, TextClean.Clean(input));

    [Fact]
    public void KeepsOrdinaryBracketsAndComparisons() =>
        Assert.Equal("Score [3] is < 5", TextClean.Clean("Score [3] is < 5"));

    [Fact]
    public void CollapsesWhitespaceAndNewlines() =>
        Assert.Equal("Line one Line two", TextClean.Clean("  Line one\r\n\n   Line two  "));

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void EmptyInputGivesEmptyString(string input) =>
        Assert.Equal("", TextClean.Clean(input));
}
