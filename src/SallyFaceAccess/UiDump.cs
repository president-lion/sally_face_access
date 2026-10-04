using System.Text;
using Il2CppAC;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SallyFaceAccess;

/// <summary>
/// Writes what is on screen right now to UserData\SallyFaceAccess\screen.txt: the open
/// Adventure Creator menus with their visible elements, the EventSystem selection, and
/// every active UI Text with its hierarchy path. The quickest way to see why something
/// is not being read.
/// </summary>
internal static class UiDump
{
    public static void Write()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        try
        {
            sb.AppendLine($"Game state: {KickStarter.stateHandler?.gameState}");
            var sel = EventSystem.current?.currentSelectedGameObject;
            sb.AppendLine($"EventSystem selected: {(sel != null ? PathOf(sel.transform) : "<none>")}");

            sb.AppendLine().AppendLine("== Open AC menus");
            var menus = KickStarter.menuManager?.menus;
            for (var i = 0; menus != null && i < menus.Count; i++)
            {
                var m = menus[i];
                if (m == null || !m.IsOn()) continue;
                sb.AppendLine($"[{m.id}] {m.title} ({m.menuSource}) selected={m.selected_element?.title}:{m.selected_slot}");
                for (var e = 0; e < m.visibleElements.Count; e++)
                {
                    var el = m.visibleElements[e];
                    sb.AppendLine($"    {el.GetIl2CppType().Name} '{el.title}' label='{TextDump.ElementLabel(el)}' slots={el.GetNumSlots()}");
                }
            }
        }
        catch (Exception ex) { sb.AppendLine("AC section failed: " + ex.Message); }

        sb.AppendLine().AppendLine("== Active UI Text");
        foreach (var t in UnityEngine.Object.FindObjectsOfType<Text>())
        {
            if (t == null || !t.isActiveAndEnabled || string.IsNullOrWhiteSpace(t.text)) continue;
            sb.AppendLine($"{PathOf(t.transform)} = \"{t.text.Replace("\n", "\\n")}\"");
        }

        File.WriteAllText(Path.Combine(Log.Dir, "screen.txt"), sb.ToString());
        Log.Info("UiDump", "written");
    }

    public static string PathOf(Transform t)
    {
        var parts = new List<string>();
        for (; t != null; t = t.parent) parts.Add(t.name);
        parts.Reverse();
        return string.Join("/", parts);
    }
}
