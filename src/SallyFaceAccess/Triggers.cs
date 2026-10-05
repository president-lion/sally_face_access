using Il2CppAC;
using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// Adventure Creator triggers: invisible zones that run a script when the player walks into
/// them - most story beats in walk-only areas. The ones that do something worth knowing
/// about (speech, a scene change, a cutscene) join the room scan; triggers that only move the
/// camera or other triggers around are left out.
/// </summary>
internal static class Triggers
{
    public sealed class Entry
    {
        public AC_Trigger Trigger;
        public float X, Y;
        public string Name;
    }

    // Actions that are scaffolding, not events.
    private static readonly HashSet<string> Silent = new()
        { "ActionCamera", "ActionTeleport", "ActionInstantiate", "ActionPlayerLock", "ActionHotspotEnable", "ActionVarSet",
          "ActionVarCheck", "ActionComment", "ActionPause", "ActionCharFaceDirection", "ActionCharRender", "ActionCharFollow" };

    // Triggers whose purpose is known, by scene/name; "" hides scaffolding the generic rules let through.
    private static readonly Dictionary<string, string> Known = new()
    {
        ["03_cult_Main/switchStep"] = "floor switch",
        ["03_cult_Main/doorFalls"] = "gate",
        ["03_cult_Main/doorFalls (1)"] = "gate",
        ["03_cult_Center/AshTrigger"] = "Ash",
        ["04_darkPlace/eyeTriggerR"] = "the eyes",
        ["04_darkPlace/faceTrigger"] = "the faces",
        ["04_darkPlace/herTrigger"] = "her",
        ["04_house/tvOff"] = "",          // turns the TV off when you walk away from it
        ["04_house/tvOff (1)"] = "",
        // The barrier spells (Episode 4): standing here and playing the guitar starts the runes.
        ["04_2_F1/guitarIN"] = "barrier spell",
        ["04_2_F2/guitarIN"] = "barrier spell",
        ["04_2_F3/guitarIN"] = "barrier spell",
        ["04_2_F4/guitarIN"] = "barrier spell",
        ["04_2_roof/guitarIN"] = "barrier spell",
        ["04_2_403/Trigger2D"] = "",      // only plays a stinger as Sal sees Mr. Sanderson; described instead
        ["04_201/safeTrig"] = "",         // only flags "standing at the safe"; the safe is its own hotspot
        ["04_201/safeTrigOut"] = "",
        ["04_201/safeTrigOut (1)"] = "",
        ["03_cult_L2/switch1"] = "floor switch 1",
        ["03_cult_L2/switch2"] = "floor switch 2",
        ["03_cult_L2/switch3"] = "floor switch 3",
        ["03_cult_L2/switch4"] = "floor switch 4",
        ["03_cult_L2/switch5"] = "floor switch 5",
        ["03_cult_S2/switch1"] = "floor switch 1",
        ["03_cult_S2/switch2"] = "floor switch 2",
        ["03_cult_S2/switch3"] = "floor switch 3",
        ["03_cult_S2/switch4"] = "floor switch 4",
        ["03_cult_S2/switch5"] = "floor switch 5",
    };

    private static readonly Dictionary<IntPtr, string> _names = new();

    public static void OnSceneLoaded() => _names.Clear();

    /// <summary>The meaningful, live triggers in the scene.</summary>
    public static List<Entry> Collect()
    {
        var list = new List<Entry>();
        foreach (var t in UnityEngine.Object.FindObjectsOfType<AC_Trigger>())
        {
            if (t == null || !t.gameObject.activeInHierarchy || !t.IsOn() || !t.detectsPlayer) continue;
            var name = NameOf(t);
            if (name == null) continue;
            // Finding hot spots is the search itself; with hints off only the glow gives them away.
            if ((name is Strings.HotSpot or Strings.DeadstoneSpot or Strings.PonySpot) && Settings.Hints == "off") continue;
            var c = t.GetComponent<Collider2D>()?.bounds.center ?? t.GetComponent<Collider>()?.bounds.center ?? t.transform.position;
            list.Add(new Entry { Trigger = t, X = c.x, Y = c.y, Name = name });
        }
        return list;
    }

    /// <summary>"way to (area)" for a scene change, "something happens here" for other events, null for scaffolding.</summary>
    private static string NameOf(AC_Trigger t)
    {
        if (_names.TryGetValue(t.Pointer, out var cached)) return cached;
        string name = null;
        if (Known.TryGetValue($"{UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}/{t.gameObject.name}", out var known))
        {
            known = known.Length == 0 ? null : known;
            _names[t.Pointer] = known;
            return known;
        }
        // The other side (Episode 4): "stoneIN"/"dsIN" mark where squeezing the Deadstone works,
        // "ponyIN" where the dark pony can be used; the OUT zones only clear that again.
        var obj = t.gameObject.name;
        if (System.Text.RegularExpressions.Regex.IsMatch(obj, @"^(stone|ds|pony)OUT\b"))
        {
            _names[t.Pointer] = null;
            return null;
        }
        if (System.Text.RegularExpressions.Regex.IsMatch(obj, @"^(stone|ds)IN\b")) return _names[t.Pointer] = Strings.DeadstoneSpot;
        if (System.Text.RegularExpressions.Regex.IsMatch(obj, @"^ponyIN\b")) return _names[t.Pointer] = Strings.PonySpot;

        try
        {
            var actions = t.source == ActionListSource.AssetFile && t.assetFile != null ? t.assetFile.actions : t.actions;
            var meaningful = false;
            for (var i = 0; actions != null && i < actions.Count; i++)
            {
                var a = actions[i];
                if (a == null || !a.isEnabled) continue;
                // Super Gear Boy zones: "hot spot" for the one that makes the screen glow,
                // nothing for the ones around it that only stop the glow.
                var check = a.TryCast<ActionVarCheck>();
                if (check != null && check.location == VariableLocation.Global && check.variableID == DetectorLogic.VariableId)
                {
                    if (check.intValue == DetectorLogic.Held) name = Strings.HotSpot;
                    break;
                }
                // Handheld games: traps and enemies have their own sounds, not scan entries;
                // the dark maze's room-to-room zones are passages.
                var run = a.TryCast<ActionRunActionList>();
                var target = run?.actionList?.gameObject.name;
                if (TopDownLogic.IsTrapList(target) || TopDownLogic.IsEnemyList(target)) break;
                if (target != null && System.Text.RegularExpressions.Regex.IsMatch(target, @"^5_to[A-Z]{1,2}$")) { name = Strings.Passage; break; }
                var scene = a.TryCast<ActionScene>();
                if (scene != null && !string.IsNullOrEmpty(scene.sceneName))
                {
                    name = string.Format(Strings.WayTo, WorldTables.Area(scene.sceneName));
                    break;
                }
                // Changing anyone's standard walk (and its footstep sound, e.g. onto grass) is scaffolding too.
                var anim = a.TryCast<ActionCharAnim>();
                if (anim != null && anim.method == ActionCharAnim.AnimMethodChar.SetStandard) continue;
                if (!Silent.Contains(a.GetIl2CppType().Name)) meaningful = true;
            }
            if (name == null && meaningful) name = Strings.SomethingHappens;
        }
        catch (Exception ex) { Log.Once("Triggers.NameOf", ex); }
        _names[t.Pointer] = name;
        return name;
    }
}
