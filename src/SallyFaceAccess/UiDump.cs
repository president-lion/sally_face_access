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
    public static void Write() => Write(Path.Combine(Log.Dir, "screen.txt"));

    public static void Write(string path)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Scene: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().name}");
        try
        {
            sb.AppendLine($"Game state: {KickStarter.stateHandler?.gameState}");
            var sel = EventSystem.current?.currentSelectedGameObject;
            sb.AppendLine($"EventSystem selected: {(sel != null ? PathOf(sel.transform) : "<none>")}");

            sb.AppendLine().AppendLine("== Open AC menus");
            var menus = KickStarter.playerMenus != null ? PlayerMenus.GetMenus(true) : null;
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

        try
        {
            sb.AppendLine().AppendLine("== Hotspots (x order)");
            var player = KickStarter.player;
            if (player != null)
                sb.AppendLine($"Player '{player.gameObject.name}' at {player.transform.position}");
            var active = KickStarter.playerInteraction?.GetActiveHotspot();
            sb.AppendLine($"Active: {(active != null ? active.gameObject.name : "<none>")}");
            sb.AppendLine($"Camera: {WorldHandler.CameraName()}  NavMesh: {WorldHandler.NavMeshName()}");
            foreach (var cam in UnityEngine.Object.FindObjectsOfType<_Camera>())
                if (cam != null) sb.AppendLine($"  camera '{cam.gameObject.name}' at {cam.transform.position} type={cam.GetIl2CppType().Name}");
            foreach (var nm in UnityEngine.Object.FindObjectsOfType<NavigationMesh>())
                if (nm != null) sb.AppendLine($"  navmesh '{nm.gameObject.name}' active={nm.gameObject.activeInHierarchy}");
            var lang = Options.GetLanguage();
            var all = UnityEngine.Object.FindObjectsOfType<Hotspot>().ToList();
            all.Sort((a, b) => WorldHandler.X(a).CompareTo(WorldHandler.X(b)));
            foreach (var h in all)
            {
                if (h == null) continue;
                var p = h.transform.position;
                sb.AppendLine($"  x={WorldHandler.X(h):0.00} y={p.y:0.00} z={p.z:0.00} on={h.IsOn()} active={h.gameObject.activeInHierarchy} " +
                              $"obj='{h.gameObject.name}' hsName='{h.hotspotName}' label='{h.GetName(lang)}' " +
                              $"-> '{WorldHandler.Name(h)}' path={PathOf(h.transform)}");
            }
        }
        catch (Exception ex) { sb.AppendLine("Hotspot section failed: " + ex.Message); }

        sb.AppendLine().AppendLine("== Active UI Text");
        foreach (var t in UnityEngine.Object.FindObjectsOfType<Text>())
        {
            if (t == null || !t.isActiveAndEnabled || string.IsNullOrWhiteSpace(t.text)) continue;
            sb.AppendLine($"{PathOf(t.transform)} = \"{t.text.Replace("\n", "\\n")}\"");
        }

        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, sb.ToString());
        Log.Info("UiDump", "written " + path);
    }

    public static string PathOf(Transform t)
    {
        var parts = new List<string>();
        for (; t != null; t = t.parent) parts.Add(t.name);
        parts.Reverse();
        return string.Join("/", parts);
    }
}
