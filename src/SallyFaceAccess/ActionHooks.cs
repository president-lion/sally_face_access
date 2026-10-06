using HarmonyLib;
using Il2CppAC;

namespace SallyFaceAccess;

/// <summary>
/// Describes visual events by what they are rather than where they happen: when a cutscene
/// plays an animation ("ghostGirlUp") or shows or hides an object ("demon"), the matching
/// entry in <see cref="Descriptions"/>' visual table is spoken - in whichever scene it occurs.
/// Keys: "anim:clipName", "show:objectName", "hide:objectName".
///
/// AC calls an action's Run() repeatedly while it waits, so each action instance is spoken
/// once. In debug mode every unmatched event is logged to visuals.log, as a to-do list.
/// </summary>
internal static class ActionHooks
{
    private static readonly Dictionary<IntPtr, float> _recent = new();

    private static void Seen(Il2CppAC.Action action, string key)
    {
        if (string.IsNullOrEmpty(key)) return;
        var now = UnityEngine.Time.unscaledTime;
        // A barrier rune is a timed prompt to play its note, so it is spoken at once, and again
        // whenever the sequence restarts after a miss.
        var rune = PuzzleText.RuneNote(key);
        if (_recent.TryGetValue(action.Pointer, out var at) && now - at < (rune > 0 ? 0.5f : 3f)) return;
        _recent[action.Pointer] = now;
        if (_recent.Count > 256) _recent.Clear();

        if (rune > 0) { Speech.Say(PuzzleText.GuitarNote(rune), interrupt: true); return; }
        // A scene's own entry ("05_alt_House/anim:tv_on") wins over the one for every scene.
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        var text = Descriptions.Visual($"{scene}/{key}") ?? Descriptions.Visual(key);
        // "" = known, deliberately silent; a leading "!" marks a warning that cuts in at once.
        if (text != null && text.StartsWith("!")) Speech.Say(text[1..], interrupt: true);
        else if (text != null) { if (text.Length > 0) Speech.Say(text, interrupt: false); }
        if (text == null && Settings.DebugLog) Log.Append("visuals.log", $"{scene} {key}");
    }

    [HarmonyPatch(typeof(ActionCharAnim), nameof(ActionCharAnim.Run))]
    private static class CharAnim
    {
        private static void Prefix(ActionCharAnim __instance)
        {
            try
            {
                if (__instance.clip2D == "glob_death") Arena.Killed(__instance.animChar);
                Seen(__instance, "anim:" + __instance.clip2D);
            }
            catch (Exception ex) { Log.Once("ActionHooks.CharAnim", ex); }
        }
    }

    [HarmonyPatch(typeof(ActionAnim), nameof(ActionAnim.Run))]
    private static class Anim
    {
        private static void Prefix(ActionAnim __instance)
        {
            try
            {
                Arena.ArmAnim(__instance.runtimeAnimator ?? __instance.animator, __instance.clip2D);
                Seen(__instance, "anim:" + __instance.clip2D);
            }
            catch (Exception ex) { Log.Once("ActionHooks.Anim", ex); }
        }
    }

    [HarmonyPatch(typeof(ActionVisible), nameof(ActionVisible.Run))]
    private static class Visible
    {
        private static void Prefix(ActionVisible __instance)
        {
            try
            {
                var ob = __instance.obToAffect;
                if (ob == null) return;
                Seen(__instance, (__instance.visState == VisState.Visible ? "show:" : "hide:") + ob.name);
            }
            catch (Exception ex) { Log.Once("ActionHooks.Visible", ex); }
        }
    }

    [HarmonyPatch(typeof(ActionInstantiate), nameof(ActionInstantiate.Run))]
    private static class Instantiate
    {
        private static void Prefix(ActionInstantiate __instance)
        {
            try
            {
                var ob = __instance.gameObject;
                if (ob == null) return;
                var verb = __instance.invAction switch { InvAction.Add => "show:", InvAction.Remove => "hide:", _ => null };
                if (verb != null) Seen(__instance, verb + ob.name);
            }
            catch (Exception ex) { Log.Once("ActionHooks.Instantiate", ex); }
        }
    }
}
