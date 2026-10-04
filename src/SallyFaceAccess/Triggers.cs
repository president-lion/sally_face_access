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
          "ActionVarCheck", "ActionComment", "ActionPause", "ActionCharFaceDirection" };

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
        try
        {
            var actions = t.source == ActionListSource.AssetFile && t.assetFile != null ? t.assetFile.actions : t.actions;
            var meaningful = false;
            for (var i = 0; actions != null && i < actions.Count; i++)
            {
                var a = actions[i];
                if (a == null || !a.isEnabled) continue;
                var scene = a.TryCast<ActionScene>();
                if (scene != null && !string.IsNullOrEmpty(scene.sceneName))
                {
                    name = string.Format(Strings.WayTo, WorldTables.Area(scene.sceneName));
                    break;
                }
                if (!Silent.Contains(a.GetIl2CppType().Name)) meaningful = true;
            }
            if (name == null && meaningful) name = Strings.SomethingHappens;
        }
        catch (Exception ex) { Log.Once("Triggers.NameOf", ex); }
        _names[t.Pointer] = name;
        return name;
    }
}
