using SallyFaceAccess;
using Xunit;

public class SelectionAnnouncerTests
{
    private readonly SelectionAnnouncer _a = new();

    [Fact]
    public void NewSelectionSpeaksLabelAndValue() =>
        Assert.Equal("Music volume, 92 percent", _a.Next("4/1/0", "Music volume", "92 percent"));

    [Fact]
    public void NewSelectionWithoutValueSpeaksLabel() =>
        Assert.Equal("Options", _a.Next("126/3/0", "Options", null));

    [Fact]
    public void UnchangedSelectionIsSilent()
    {
        _a.Next("4/1/0", "Music volume", "92 percent");
        Assert.Null(_a.Next("4/1/0", "Music volume", "92 percent"));
    }

    [Fact]
    public void ValueChangeOnSameSelectionSpeaksOnlyValue()
    {
        _a.Next("4/1/0", "Music volume", "92 percent");
        Assert.Equal("90 percent", _a.Next("4/1/0", "Music volume", "90 percent"));
    }

    [Fact]
    public void LabelChangeOnSameSelectionSpeaksLabel()
    {
        // e.g. a toggle or cycle whose label embeds its state
        _a.Next("4/5/0", "Subtitles: On", null);
        Assert.Equal("Subtitles: Off", _a.Next("4/5/0", "Subtitles: Off", null));
    }

    [Fact]
    public void ReturningToPreviousItemSpeaksItAgain()
    {
        _a.Next("126/1/0", "New game", null);
        _a.Next("126/2/0", "Episodes", null);
        Assert.Equal("New game", _a.Next("126/1/0", "New game", null));
    }

    [Fact]
    public void ClearMakesSameItemSpeakAgain()
    {
        _a.Next("126/1/0", "New game", null);
        _a.Clear();
        Assert.Equal("New game", _a.Next("126/1/0", "New game", null));
    }
}
