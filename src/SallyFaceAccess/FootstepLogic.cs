namespace SallyFaceAccess;

/// <summary>
/// When the mod should play its own footstep: while the player walks somewhere the game
/// plays none. A game footstep heard within <see cref="GameStepMemory"/> seconds means the
/// area has its own, and the mod stays quiet.
/// Pure logic - no Unity types - so it is unit tested.
/// </summary>
internal static class FootstepLogic
{
    public const float WalkInterval = 0.42f;
    public const float RunInterval = 0.27f;
    public const float GameStepMemory = 1.0f;

    public static bool ShouldStep(float now, float lastGameStep, float lastOwnStep, bool moving, bool running)
    {
        if (!moving) return false;
        if (now - lastGameStep < GameStepMemory) return false;
        return now - lastOwnStep >= (running ? RunInterval : WalkInterval);
    }
}
