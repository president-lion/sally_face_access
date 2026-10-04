using SallyFaceAccess;
using Xunit;

public class DetectorLogicTests
{
    [Fact] public void Raising() => Assert.Equal("Gear Boy up", DetectorLogic.Transition(2, 555));
    [Fact] public void Glow() => Assert.Equal("Gear Boy glowing, press R", DetectorLogic.Transition(555, 777));
    [Fact] public void Fade() => Assert.Equal("glow fades", DetectorLogic.Transition(777, 555));
    [Fact] public void Lowering() => Assert.Equal("Gear Boy down", DetectorLogic.Transition(555, 2));
    [Fact] public void Lowering_while_glowing() => Assert.Equal("Gear Boy down", DetectorLogic.Transition(777, 0));
    [Fact] public void Story_values_are_silent() => Assert.Null(DetectorLogic.Transition(1, 4));
    [Fact] public void No_change_is_silent() => Assert.Null(DetectorLogic.Transition(555, 555));
}
