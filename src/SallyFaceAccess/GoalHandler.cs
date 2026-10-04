using Il2CppAC;
using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// The current goal: a global variable the game shows in its "Goal" menu ("???" when there
/// is none). Announces a new goal when it changes, and F5 reads it on demand.
/// </summary>
internal static class GoalHandler
{
    private static string _goal;
    private static bool _baseline;
    private static float _nextCheck;

    /// <summary>A scene load may be a save being loaded: take a fresh baseline silently.</summary>
    public static void OnSceneLoaded() => _baseline = false;

    public static void Tick()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            var g = Current();
            Speech.Say(g.Length > 0 ? string.Format(Strings.Goal, g) : Strings.NoGoal);
        }

        if (Time.unscaledTime < _nextCheck) return;
        _nextCheck = Time.unscaledTime + 1f;

        var goal = Current();
        if (_baseline && goal != _goal && goal.Length > 0)
            Speech.Say(string.Format(Strings.NewGoal, goal), interrupt: false);
        _goal = goal;
        _baseline = true;
    }

    /// <summary>The goal text, or "" when there is none.</summary>
    public static string Current()
    {
        if (KickStarter.playerMenus == null) return "";
        // The "Goal" label shows a global variable, and only refreshes while its menu is
        // drawn - so read the variable itself.
        var label = PlayerMenus.GetElementWithName("Goal", "Goal")?.TryCast<MenuLabel>();
        if (label == null) return "";
        var lang = Options.GetLanguage();
        string text;
        if (label.labelType == AC_LabelType.GlobalVariable)
            text = GlobalVariables.GetVariable(label.variableID)?.GetValue(lang);
        else
            text = label.GetLabel(0, lang);
        text = TextClean.Clean(text);
        return text is "???" or "?" or "0" ? "" : text;
    }
}
