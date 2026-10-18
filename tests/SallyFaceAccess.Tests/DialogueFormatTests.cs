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
    public void MirroredTextReadsForwards() =>
        Assert.Equal("some things will appear broken. do not be afraid. your mind cannot handle it all at once",
            DialogueFormat.Unmirror("ecno ta lla ti eldnah tonnac dnim ruoy.diarfa eb ton od.nekorb raeppa lliw sgniht emos"));

    [Fact]
    public void OnlyMirroredLinesAreFlipped()
    {
        Assert.True(DialogueFormat.IsMirrored(1820));
        Assert.False(DialogueFormat.IsMirrored(1821));
    }

    [Fact]
    public void EmptyTextGivesEmpty() =>
        Assert.Equal("", DialogueFormat.Line("Larry", "  ", true));
}
