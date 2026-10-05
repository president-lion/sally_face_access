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

public class GuitarTextTests
{
    // Notes 1-3 are up, 4-6 right, 7-9 down, 10-12 left; within a group J, K, L.
    [Theory]
    [InlineData(1, "up, J")]
    [InlineData(5, "right, K")]
    [InlineData(9, "down, L")]
    [InlineData(10, "left, J")]
    [InlineData(12, "left, L")]
    public void Note_names_direction_and_key(int note, string expected) => Assert.Equal(expected, PuzzleText.GuitarNote(note));

    [Theory]
    [InlineData("anim:rune_7", 7)]
    [InlineData("anim:rune_12", 12)]
    [InlineData("anim:rune_7_gone", 0)]
    [InlineData("anim:rune_Null", 0)]
    [InlineData("anim:symbol_timer3", 0)]
    public void Rune_clips_give_their_note(string key, int note) => Assert.Equal(note, PuzzleText.RuneNote(key));
}
