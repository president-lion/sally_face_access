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
