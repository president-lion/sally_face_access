using SallyFaceAccess;
using Xunit;

public class MenuStatesTests
{
    private static Dictionary<string, string> Visible(params (string title, string text)[] els) =>
        els.ToDictionary(e => e.title, e => e.text);

    [Fact]
    public void Stereo_reads_frequency_and_genre()
    {
        var v = Visible(("BG", ""), ("Select", "Select"), ("classic rock", "102.5"));
        Assert.Equal("102.5, classic rock", MenuStates.State("Stereo", v));
    }

    [Fact]
    public void Stereo_hip_is_hip_hop()
    {
        Assert.Equal("93.7, hip hop", MenuStates.State("Stereo 2", Visible(("hip", "93.7"))));
    }

    [Fact]
    public void Stereo_static_is_said()
    {
        Assert.Equal("96.1, pop, static", MenuStates.State("Stereo OS", Visible(("pop", "96.1"), ("static", ""))));
    }

    [Fact]
    public void Game_book_left_page()
    {
        var v = Visible(("MC", ""), ("C", ""), ("SelectL", ""), ("Next", "Next"));
        Assert.Equal("Musical Chairs, 1 of 6", MenuStates.State("Game Book", v));
    }

    [Fact]
    public void Game_book_right_page()
    {
        var v = Visible(("T", ""), ("PP", ""), ("SelectR", ""));
        Assert.Equal("Ping Pong, 6 of 6", MenuStates.State("Game Book", v));
    }

    [Fact]
    public void Game_book_middle_spread()
    {
        Assert.Equal("Duck, Duck, Moose!, 3 of 6", MenuStates.State("Game Book", Visible(("DDM", ""), ("DB", ""), ("SelectL", ""))));
        Assert.Equal("Dodge Ball, 4 of 6", MenuStates.State("Game Book", Visible(("DDM", ""), ("DB", ""), ("SelectR", ""))));
    }

    [Fact]
    public void Clock_reads_the_whole_time()
    {
        var v = Visible(("Label", "Change Time"), ("1", "0"), ("2", "3"), ("3", "1"), ("4", "4"), (":", ":"));
        Assert.Equal("03:14", MenuStates.State("Packerton's Clock", v));
    }

    [Fact]
    public void Dark_place_lock_names_each_dial()
    {
        var v = Visible(("Graphic", ""), ("1-4", ""), ("2-0", ""), ("3-5", ""));
        Assert.Equal("dial 1: double N. dial 2: dot. dial 3: zigzag", MenuStates.State("Dark Place Lock", v));
    }

    [Theory]
    [InlineData(0, "nothing entered")]
    [InlineData(6, "entered: white")]
    [InlineData(612, "entered: white, black, blue")]
    [InlineData(345, "entered: purple, green, red")]
    public void Hex_pad_reads_the_colours_entered(int code, string expected) =>
        Assert.Equal(expected, MenuStates.HexPadEntry(code));

    [Theory]
    [InlineData(0, "nothing entered")]
    [InlineData(502, "entered: 5 0 2")]
    public void Number_pad_reads_the_digits_entered(int code, string expected) =>
        Assert.Equal(expected, MenuStates.DigitsEntry(code));

    [Fact]
    public void Hex_close_up_describes_the_hexagon_shown() =>
        Assert.Equal("a black hexagon with five green dots",
            MenuStates.State("HexCU", Visible(("close", ""), ("black", ""))));

    [Fact]
    public void Bird_cage_names_what_each_compass_slot_holds()
    {
        // Slot pictures are "tooth <letter> <slot>", slots numbered S=1 W=2 N=3 E=4 centre=5.
        var v = Visible(("compass", ""), ("tooth s 3", ""), ("tooth x 5", ""), ("tooth n icon", ""));
        Assert.Equal("north: tooth S. east: empty. south: empty. west: empty. centre: tooth X",
            MenuStates.State("bird cage", v));
    }

    [Fact]
    public void Cell_phone_reads_sender_and_message()
    {
        var v = Visible(("inbox name", "Larry"), ("Text", "r u awake?"), ("Reply", "Reply"), ("Exit", "Exit"));
        Assert.Equal("Larry: r u awake?", MenuStates.State("Cell Phone", v));
    }

    [Fact]
    public void Other_menus_have_no_state()
    {
        Assert.Null(MenuStates.State("Pause", Visible(("Resume", "Resume"))));
    }

    [Fact]
    public void Nothing_visible_yet_is_null()
    {
        Assert.Null(MenuStates.State("Game Book", Visible(("Next", "Next"))));
    }
}
