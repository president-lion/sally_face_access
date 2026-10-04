using Il2CppAC;

namespace SallyFaceAccess;

/// <summary>
/// Reads Adventure Creator menus. All 191 of the game's menus are AC-drawn (OnGUI), so
/// they are read from AC's Menu/MenuElement objects, not from Unity UI.
///
/// Polls every frame rather than relying only on EventManager hooks: it cannot be
/// defeated by IL2CPP inlining an event method, and walking the menu list is cheap.
/// </summary>
internal static class MenuHandler
{
    private static readonly HashSet<int> _open = new();
    private static readonly HashSet<int> _scratch = new();
    private static readonly SelectionAnnouncer _announcer = new();

    public static void Tick()
    {
        // Runtime menus live in PlayerMenus; menuManager.menus holds only the design-time templates.
        if (KickStarter.playerMenus == null) return;
        var menus = PlayerMenus.GetMenus(true);
        if (menus == null) return;
        var lang = Options.GetLanguage();

        _scratch.Clear();
        Menu focused = null;
        for (var i = 0; i < menus.Count; i++)
        {
            var m = menus[i];
            if (m == null || !m.IsOn()) continue;
            _scratch.Add(m.id);
            if (!_open.Contains(m.id)) OnOpened(m, lang);
            if (m.selected_element != null) focused = m;
        }
        foreach (var id in _open)
            if (!_scratch.Contains(id)) Log.Debug("Menu", $"closed {id}");
        _open.Clear();
        _open.UnionWith(_scratch);

        if (focused != null) CheckSelection(focused, lang);
        else _announcer.Clear();
    }

    private static void OnOpened(Menu m, int lang)
    {
        Log.Debug("Menu", $"opened {m.id} '{m.title}' appear={m.appearType}");
    }

    private static void CheckSelection(Menu m, int lang)
    {
        var el = m.selected_element;
        var slot = m.selected_slot;
        var label = Label(el, slot, lang);
        var value = Value(el);
        var say = _announcer.Next($"{m.id}/{el.ID}/{slot}", label, value);
        if (say == null) return;

        Log.Debug("Menu", $"select {m.title}/{el.title} [{el.GetIl2CppType().Name}] slot {slot} -> '{say}'");
        Speech.Say(say);
    }

    /// <summary>The element's spoken name for a slot, falling back to its design-time title.</summary>
    internal static string Label(MenuElement el, int slot, int lang)
    {
        var label = TextClean.Clean(el.GetLabel(slot, lang));
        return string.IsNullOrEmpty(label) ? el.title : label;
    }

    /// <summary>The element's current setting, for elements whose label does not already carry it.</summary>
    internal static string Value(MenuElement el)
    {
        var slider = el.TryCast<MenuSlider>();
        if (slider != null)
        {
            var range = slider.maxValue - slider.minValue;
            var pct = range > 0 ? (int)Math.Round((slider.amount - slider.minValue) / range * 100) : 0;
            return string.Format(Strings.Percent, pct);
        }
        return null;
    }
}
