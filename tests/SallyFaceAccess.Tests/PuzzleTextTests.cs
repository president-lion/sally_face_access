using SallyFaceAccess;
using Xunit;

public class PuzzleTextTests
{
    [Fact]
    public void Seal_positions() =>
        Assert.Equal("seal: right lever ring 2, left lever ring 3, of 4. Line both up at 1", PuzzleText.Seal(2, 3));

    [Fact]
    public void Seal_solved() => Assert.Equal("seal lined up", PuzzleText.Seal(1, 1));

    [Fact]
    public void Switches_down() =>
        Assert.Equal("switches down: 1, 3, 5", PuzzleText.Switches(new[] { true, false, true, false, true }));

    [Fact]
    public void Switches_none() => Assert.Equal("no switches down", PuzzleText.Switches(new bool[5]));

    [Fact]
    public void Switches_all() => Assert.Equal("all switches down", PuzzleText.Switches(new[] { true, true, true, true, true }));
}
