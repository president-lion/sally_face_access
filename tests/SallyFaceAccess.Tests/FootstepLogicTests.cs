using SallyFaceAccess;
using Xunit;

public class FootstepLogicTests
{
    private const float Never = -100f;

    [Fact] public void Walking_in_silence_steps() => Assert.True(FootstepLogic.ShouldStep(10f, Never, 9.5f, true, false));
    [Fact] public void Standing_still_is_silent() => Assert.False(FootstepLogic.ShouldStep(10f, Never, Never, false, false));
    [Fact] public void Game_footsteps_win() => Assert.False(FootstepLogic.ShouldStep(10f, 9.5f, Never, true, false));
    [Fact] public void Game_footsteps_forgotten_after_a_second() => Assert.True(FootstepLogic.ShouldStep(10f, 8.9f, Never, true, false));
    [Fact] public void Walking_pace() => Assert.False(FootstepLogic.ShouldStep(10f, Never, 9.7f, true, false));
    [Fact] public void Running_is_quicker() => Assert.True(FootstepLogic.ShouldStep(10f, Never, 9.7f, true, true));
}
