using SallyFaceAccess;
using Xunit;

public class DialogueFormatTests
{
    [Fact]
    public void SpeakerAndText() =>
        Assert.Equal("Dad: Can't talk right now, Sal.", DialogueFormat.Line("Dad", "Can't talk right now, Sal.", true));

    [Fact]
    public void NarrationHasNoSpeaker() =>
        Assert.Equal("It was a dark night.", DialogueFormat.Line("", "It was a dark night.", true));

    [Fact]
    public void SpeakerNamesCanBeTurnedOff() =>
        Assert.Equal("Hey, dad.", DialogueFormat.Line("Sal", "Hey, dad.", false));

    [Fact]
    public void TextIsCleaned() =>
        Assert.Equal("Larry: Hi there.", DialogueFormat.Line(" Larry ", "<b>Hi</b>[wait] there.", true));

    [Fact]
    public void EmptyTextGivesEmpty() =>
        Assert.Equal("", DialogueFormat.Line("Larry", "  ", true));
}
