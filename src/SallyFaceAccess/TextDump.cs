using System.Text.Encodings.Web;
using System.Text.Json;
using Il2CppAC;

namespace SallyFaceAccess;

/// <summary>
/// Writes the game's text out of Adventure Creator's managers to UserData\SallyFaceAccess\dump.
/// speechManager.lines is AC's master list of every translatable string - speech, item
/// names, hotspot labels, menu labels, documents - each with all of its translations.
/// The menu and inventory dumps give the structure the handlers navigate.
/// </summary>
internal static class TextDump
{
    private static readonly JsonWriterOptions Opts = new()
    {
        Indented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    };

    public static string DumpDir => Path.Combine(Log.Dir, "dump");

    public static bool Ready => KickStarter.speechManager != null && KickStarter.menuManager != null
                                && KickStarter.inventoryManager != null;

    public static void WriteAll()
    {
        Directory.CreateDirectory(DumpDir);
        Write("lines.json", WriteLines);
        Write("menus.json", WriteMenus);
        Write("inventory.json", WriteInventory);
        Log.Info("Dump", "written to " + DumpDir);
    }

    private static void Write(string file, Action<Utf8JsonWriter> body)
    {
        try
        {
            using var fs = File.Create(Path.Combine(DumpDir, file));
            using var w = new Utf8JsonWriter(fs, Opts);
            body(w);
        }
        catch (Exception ex) { Log.Once("Dump." + file, ex); }
    }

    private static void WriteLines(Utf8JsonWriter w)
    {
        var sm = KickStarter.speechManager;
        w.WriteStartObject();
        w.WriteStartArray("languages");
        for (var i = 0; i < sm.languages.Count; i++) w.WriteStringValue(sm.languages[i]);
        w.WriteEndArray();

        w.WriteStartArray("lines");
        for (var i = 0; i < sm.lines.Count; i++)
        {
            var l = sm.lines[i];
            if (l == null) continue;
            w.WriteStartObject();
            w.WriteNumber("id", l.lineID);
            w.WriteString("type", l.textType.ToString());
            w.WriteString("scene", l.scene ?? "");
            w.WriteString("owner", l.owner ?? "");
            w.WriteString("speaker", l.characterName ?? "");
            w.WriteBoolean("player", l.isPlayer);
            w.WriteString("text", l.text ?? "");
            if (!string.IsNullOrEmpty(l.description)) w.WriteString("description", l.description);
            w.WriteStartArray("translations");
            if (l.translationText != null)
                for (var t = 0; t < l.translationText.Count; t++) w.WriteStringValue(l.translationText[t] ?? "");
            w.WriteEndArray();
            w.WriteEndObject();
        }
        w.WriteEndArray();
        w.WriteEndObject();
    }

    private static void WriteMenus(Utf8JsonWriter w)
    {
        var mm = KickStarter.menuManager;
        w.WriteStartArray();
        for (var i = 0; i < mm.menus.Count; i++)
        {
            var m = mm.menus[i];
            if (m == null) continue;
            w.WriteStartObject();
            w.WriteNumber("id", m.id);
            w.WriteString("title", m.title ?? "");
            w.WriteString("source", m.menuSource.ToString());
            w.WriteString("appear", m.appearType.ToString());
            w.WriteBoolean("pauses", m.pauseWhenEnabled);
            w.WriteStartArray("elements");
            var els = m.elements;
            for (var e = 0; els != null && e < els.Count; e++)
            {
                var el = els[e];
                if (el == null) continue;
                w.WriteStartObject();
                w.WriteNumber("id", el.ID);
                w.WriteString("title", el.title ?? "");
                w.WriteString("kind", el.GetIl2CppType().Name);
                w.WriteNumber("lineID", el.lineID);
                var label = ElementLabel(el);
                if (label != null) w.WriteString("label", label);
                w.WriteEndObject();
            }
            w.WriteEndArray();
            w.WriteEndObject();
        }
        w.WriteEndArray();
    }

    /// <summary>The design-time label of an element, for the element kinds that carry one.</summary>
    internal static string ElementLabel(MenuElement el)
    {
        var b = el.TryCast<MenuButton>();
        if (b != null) return b.label;
        var l = el.TryCast<MenuLabel>();
        if (l != null) return l.label;
        var t = el.TryCast<MenuToggle>();
        if (t != null) return t.label;
        var s = el.TryCast<MenuSlider>();
        if (s != null) return s.label;
        var c = el.TryCast<MenuCycle>();
        if (c != null) return c.label;
        return null;
    }

    private static void WriteInventory(Utf8JsonWriter w)
    {
        var im = KickStarter.inventoryManager;
        w.WriteStartObject();
        w.WriteStartArray("items");
        for (var i = 0; i < im.items.Count; i++)
        {
            var it = im.items[i];
            if (it == null) continue;
            w.WriteStartObject();
            w.WriteNumber("id", it.id);
            w.WriteString("label", it.label ?? "");
            w.WriteString("altLabel", it.altLabel ?? "");
            w.WriteNumber("lineID", it.lineID);
            w.WriteEndObject();
        }
        w.WriteEndArray();
        w.WriteNumber("documents", im.documents?.Count ?? 0);
        w.WriteEndObject();
    }
}
