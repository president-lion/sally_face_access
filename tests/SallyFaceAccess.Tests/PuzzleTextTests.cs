using SallyFaceAccess;
using Xunit;

public class PuzzleTextTests
{
    [Fact]
    public void Seal_positions() =>
        Assert.Equal("seal: right lever ring 2, left lever ring 3, of 4. Line both up at 1", PuzzleText.Seal(2, 3));

    [Fact]
    public void Seal_solved() => Assert.Equal("seal lined up", PuzzleText.Seal(1, 1));
}
