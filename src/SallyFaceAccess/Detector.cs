using Il2CppAC;

namespace SallyFaceAccess;

/// <summary>
/// The Super Gear Boy detector: says when it is raised or put away and when its screen
/// glows. The glow is the game's only sign of a hot spot and is purely visual.
/// </summary>
internal static class Detector
{
    private static int _value = int.MinValue;

    public static void OnSceneLoaded() => _value = int.MinValue; // a load may restore any value: re-baseline

    public static void Tick()
    {
        if (KickStarter.runtimeVariables == null) return;
        var v = GlobalVariables.GetVariable(DetectorLogic.VariableId);
        if (v == null) return;
        var now = v.IntegerValue;
        if (now == _value) return;
        var was = _value;
        _value = now;
        if (was == int.MinValue) return;
        Log.Debug("Detector", $"var {DetectorLogic.VariableId}: {was} -> {now}");
        var say = DetectorLogic.Transition(was, now);
        if (say != null) Speech.Say(say);
    }
}
