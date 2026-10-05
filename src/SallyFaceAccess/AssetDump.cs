using System.Text;
using Il2CppAC;
using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// Development dump (dump\assets.txt): the scripts the game keeps outside scenes - every
/// loaded ActionListAsset with its actions and flow - plus which script each menu button and
/// menu runs. Puzzle menus (locks, keypads) keep their logic here, out of the tour's reach.
/// Written once, the first time the game reaches the title screen without the file.
/// </summary>
internal static class AssetDump
{
    public static string File => Path.Combine(TextDump.DumpDir, "assets.txt");

    public static void Write()
    {
        var sb = new StringBuilder();
        sb.AppendLine("== Menu buttons (menu / element: click type, script)");
        var menus = PlayerMenus.GetMenus(true);
        for (var i = 0; menus != null && i < menus.Count; i++)
        {
            var m = menus[i];
            if (m == null) continue;
            var on = m.actionListOnTurnOn != null ? m.actionListOnTurnOn.name : null;
            var off = m.actionListOnTurnOff != null ? m.actionListOnTurnOff.name : null;
            if (on != null || off != null) sb.AppendLine($"  {m.title}: on={on} off={off}");
            var els = m.elements;
            for (var j = 0; els != null && j < els.Count; j++)
            {
                var b = els[j]?.TryCast<MenuButton>();
                if (b == null) continue;
                sb.AppendLine($"  {m.title} / {b.title}: {b.buttonClickType}, {(b.actionList != null ? b.actionList.name : "-")}");
            }
        }

        sb.AppendLine().AppendLine("== ActionList assets");
        foreach (var asset in Resources.FindObjectsOfTypeAll<ActionListAsset>().OrderBy(a => a.name))
        {
            if (asset == null) continue;
            sb.AppendLine($"-- {asset.name}");
            TourMode.AppendActions(sb, asset.actions);
        }
        Directory.CreateDirectory(TextDump.DumpDir);
        System.IO.File.WriteAllText(File, sb.ToString());
        Log.Info("AssetDump", $"wrote {File}");
    }
}
