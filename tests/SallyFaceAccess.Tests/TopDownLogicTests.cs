using SallyFaceAccess;
using Xunit;

public class TopDownLogicTests
{
    [Fact]
    public void Right_and_above_pans_right_and_raises_pitch()
    {
        var p = TopDownLogic.Place(0.5f, 0.5f, 2f);
        Assert.True(p.Pan > 0);
        Assert.True(p.Pitch > 1);
        Assert.True(p.Volume > 0 && p.Volume < 1);
    }

    [Fact]
    public void Left_and_below()
    {
        var p = TopDownLogic.Place(-3f, -3f, 2f);
        Assert.Equal(-1f, p.Pan);
        Assert.Equal(0.5f, p.Pitch);
        Assert.Equal(0f, p.Volume); // out of range
    }

    [Fact]
    public void On_top_is_centred_and_loud()
    {
        var p = TopDownLogic.Place(0, 0, 2f);
        Assert.Equal(0f, p.Pan);
        Assert.Equal(1f, p.Pitch);
        Assert.Equal(1f, p.Volume);
    }

    [Fact]
    public void Strike_spot_is_said_in_steps_or_on_you()
    {
        Assert.Equal("on you", TopDownLogic.Beside(0.2f, 0.45f));
        Assert.Equal("3 steps right of you", TopDownLogic.Beside(0.96f, 0.45f));
        Assert.Equal("5 steps left of you", TopDownLogic.Beside(-1.6f, 0.45f));
    }

    [Fact]
    public void Ticks_speed_up_near_walls()
    {
        Assert.Null(TopDownLogic.TickInterval(1.5f));
        Assert.True(TopDownLogic.TickInterval(0.1f) < TopDownLogic.TickInterval(0.8f));
    }

    [Fact]
    public void Rect_distance()
    {
        Assert.Equal(0f, TopDownLogic.RectDistance(1, 1, 1, 1, 0.5f, 0.5f));
        Assert.Equal(0.75f, TopDownLogic.RectDistance(2, 1, 1, 1, 0.5f, 0.5f), 3);
        Assert.Equal((1.25f, 1f), TopDownLogic.NearestPoint(2, 1, 1, 1, 0.5f, 0.5f));
    }

    [Fact]
    public void Direction_words()
    {
        Assert.Equal("up 3, right 2", TopDownLogic.Direction(0.64f, 0.96f));
        Assert.Equal("down 1", TopDownLogic.Direction(0.05f, -0.3f));
        Assert.Equal("left 4", TopDownLogic.Direction(-1.28f, 0));
        Assert.Equal("here", TopDownLogic.Direction(0.1f, -0.1f));
    }

    [Fact]
    public void Wall_steps()
    {
        Assert.Equal("open", TopDownLogic.WallSteps(null));
        Assert.Equal("2", TopDownLogic.WallSteps(0.64f));
    }

    [Fact]
    public void Trap_and_enemy_scripts()
    {
        Assert.True(TopDownLogic.IsTrapList("1_spikeHit"));
        Assert.True(TopDownLogic.IsTrapList("2_electricHurt3"));
        Assert.True(TopDownLogic.IsTrapList("7_electricHurt1"));
        Assert.False(TopDownLogic.IsTrapList("4_cultHit"));
        Assert.False(TopDownLogic.IsTrapList("6_killSkull1"));
        Assert.True(TopDownLogic.IsEnemyList("8_cultHit"));
    }

    [Fact]
    public void Events()
    {
        Assert.Equal("spikes! back to the start", TopDownLogic.Event("6_spikeHit"));
        Assert.Equal("shocked! back to the start", TopDownLogic.Event("2_electricHurt2"));
        Assert.Equal("caught! back to the start", TopDownLogic.Event("4_cultHit"));
        Assert.Equal("time's up, back to the start", TopDownLogic.Event("5_timesUp"));
        Assert.Null(TopDownLogic.Event("3_enterRoom1"));
    }

    [Fact]
    public void Opening_chirps_once_when_a_close_wall_falls_away()
    {
        var o = new OpeningDetector();
        Assert.False(o.Update(0.2f, true));
        Assert.False(o.Update(0.45f, true)); // in the dead band
        Assert.True(o.Update(null, true));
        Assert.False(o.Update(null, true));
    }

    [Fact]
    public void Opening_needs_a_wall_first_and_movement()
    {
        var o = new OpeningDetector();
        Assert.False(o.Update(null, true));
        o.Update(0.1f, false);
        Assert.False(o.Update(1f, false)); // standing still: consumed silently
        Assert.False(o.Update(1f, true));
    }
}
