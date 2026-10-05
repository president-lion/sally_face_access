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
    private static float _queueSelectionUntil;

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
            else { CheckState(m, lang); CheckNewLabels(m, lang); }
            if (m.selected_element != null) focused = m;
        }
        foreach (var id in _open)
            if (!_scratch.Contains(id)) { Log.Debug("Menu", $"closed {id}"); _states.Remove(id); _labels.Remove(id); }
        _open.Clear();
        _open.UnionWith(_scratch);

        if (focused != null) CheckSelection(focused, lang);
        else _announcer.Clear();
    }

    // Menus whose labels are not worth reading on open: the title screen's version number,
    // the episode cards (their buttons read the labels), and the goal and inventory hint
    // overlays that open alongside the inventory (the goal has its own announcement and F5),
    // and the Super Gear Boy's button prompt (the Detector says what it does).
    private static readonly HashSet<string> NoIntro = new()
        { "Start Screen 2", "Episode Select 2", "Goal", "Invenotry Icon", "Super Gear Boy", "R" };

    private static void OnOpened(Menu m, int lang)
    {
        Log.Debug("Menu", $"opened {m.id} '{m.title}' appear={m.appearType}");

        // Subtitle, conversation and hotspot menus carry text the other handlers speak.
        if (m.appearType is AppearType.WhenSpeechPlays or AppearType.DuringConversation
            or AppearType.OnHotspot) return;

        _labels[m.id] = VisibleLabels(m);
        var state = State(m, lang);
        if (state != null) _states[m.id] = state;
        var intro = m.title switch
        {
            "Inventory" => InventoryHandler.Summary(lang),
            // The newspaper the journalist shows Sal: its article label is hidden at first.
            "Newspaper" => ElementText(m, "Text", lang) ?? "",
            _ when state != null => Descriptions.MenuPage(m.title) is { } shown ? $"{shown} {state}" : state,
            // Picture-only menus (a photo close-up) are described by their title.
            _ when Descriptions.MenuPage(m.title) is { } page => page,
            _ when NoIntro.Contains(m.title) => "",
            _ => IntroText(m, lang),
        };
        if (intro.Length == 0) return;
        Log.Debug("Menu", $"intro {m.title} -> '{intro}'");
        Speech.Say(intro);
        // The selection usually lands in the same frame or just after (later in a menu that
        // fades in); queue it behind the intro instead of letting it cut the intro off.
        _queueSelectionUntil = UnityEngine.Time.unscaledTime + Math.Max(1f, intro.Length * 0.06f);
    }

    private static readonly Dictionary<int, string> _states = new();
    private static readonly HashSet<IntPtr> _loggedButtons = new();
    private static readonly Dictionary<int, HashSet<int>> _labels = new();

    private static HashSet<int> VisibleLabels(Menu m)
    {
        var set = new HashSet<int>();
        var els = m.visibleElements;
        for (var i = 0; els != null && i < els.Count; i++)
            if (els[i]?.TryCast<MenuLabel>() != null) set.Add(els[i].ID);
        return set;
    }

    /// <summary>
    /// Text that appears in an open menu after it opened - what Sal says about an item picked
    /// in the treehouse chest or a locker. Only menus that pause the game (close-ups and
    /// puzzles), not on-screen overlays whose labels come and go.
    /// </summary>
    private static void CheckNewLabels(Menu m, int lang)
    {
        if (!m.pauseWhenEnabled || NoIntro.Contains(m.title) || State(m, lang) != null) return;
        var now = VisibleLabels(m);
        var before = _labels.TryGetValue(m.id, out var b) ? b : new HashSet<int>();
        _labels[m.id] = now;
        var parts = new List<string>();
        var els = m.visibleElements;
        for (var i = 0; els != null && i < els.Count; i++)
        {
            var label = els[i]?.TryCast<MenuLabel>();
            if (label == null || before.Contains(label.ID)) continue;
            var text = LabelText(label, lang);
            if (text.Length > 0 && !parts.Contains(text)) parts.Add(text);
        }
        if (parts.Count == 0) return;
        Log.Debug("Menu", $"new text in {m.title}: '{string.Join(". ", parts)}'");
        Speech.Say(string.Join(". ", parts));
        // The game moves the selection onto a close button at the same moment; let that wait its turn.
        _queueSelectionUntil = UnityEngine.Time.unscaledTime + 1f;
    }

    /// <summary>Speaks a menu's current choice when it changes (the stereo station, the game book page).</summary>
    private static void CheckState(Menu m, int lang)
    {
        var state = State(m, lang);
        if (state == null || (_states.TryGetValue(m.id, out var old) && old == state)) return;
        _states[m.id] = state;
        Log.Debug("Menu", $"state {m.title} -> '{state}'");
        Speech.Say(state);
    }

    private static string State(Menu m, int lang)
    {
        // The hex keypad keeps what was entered only in a variable, as digits.
        if (m.title == "HexPad") return MenuStates.HexPadEntry(GlobalVariables.GetVariable(17)?.IntegerValue ?? 0);
        // Todd's door keypad in the cartoon house: the digits typed so far, also kept in variable 17.
        if (m.title == "Toon Code") return MenuStates.DigitsEntry(GlobalVariables.GetVariable(17)?.IntegerValue ?? 0);
        if (m.title is not ("Stereo" or "Stereo 2" or "Stereo OS" or "Game Book" or "Packerton's Clock" or "Dark Place Lock" or "Cell Phone" or "bird cage" or "HexCU")) return null;
        var visible = new Dictionary<string, string>();
        var els = m.visibleElements;
        for (var i = 0; els != null && i < els.Count; i++)
        {
            var el = els[i];
            if (el == null || string.IsNullOrEmpty(el.title)) continue;
            var label = el.TryCast<MenuLabel>();
            visible[el.title] = label != null ? TextClean.Clean(label.GetLabel(0, lang)) : "";
        }
        return MenuStates.State(m.title, visible);
    }

    /// <summary>
    /// The text labels a menu shows when it opens - its title, or the question of a
    /// Yes/No prompt - in element order.
    /// </summary>
    private static string IntroText(Menu m, int lang)
    {
        var parts = new List<string>();
        var els = m.visibleElements;
        for (var i = 0; els != null && i < els.Count; i++)
        {
            var label = els[i]?.TryCast<MenuLabel>();
            if (label == null) continue;
            var text = LabelText(label, lang);
            if (text.Length > 0 && !parts.Contains(text)) parts.Add(text);
        }
        return string.Join(". ", parts);
    }

    private static void CheckSelection(Menu m, int lang)
    {
        var el = m.selected_element;
        var slot = m.selected_slot;
        // An empty inventory box still reports a selection; the summary already said "empty".
        if (el.TryCast<MenuInventoryBox>() != null && el.GetNumSlots() == 0) return;
        var label = Label(m, el, slot, lang);
        var value = Value(el, slot);
        var say = _announcer.Next($"{m.id}/{el.ID}/{slot}", label, value);
        if (say == null) return;

        Log.Debug("Menu", $"select {m.title}/{el.title} [{el.GetIl2CppType().Name}] slot {slot} -> '{say}'");
        var queue = UnityEngine.Time.unscaledTime < _queueSelectionUntil;
        _queueSelectionUntil = 0;
        Speech.Say(say, interrupt: !queue);
    }

    /// <summary>
    /// The element's spoken name for a slot: the label table first (image buttons have no
    /// text of their own), then the game's label, then a readable form of the internal name.
    /// </summary>
    internal static string Label(Menu m, MenuElement el, int slot, int lang)
    {
        if (Labels.TryGet(m.title, el.title, out var template))
            return Labels.Expand(template, title => ElementText(m, title, lang));
        var label = TextClean.Clean(el.GetLabel(slot, lang));
        if (!string.IsNullOrEmpty(label)) return label;
        var does = ButtonPurpose(el);
        if (does != null) return does;
        return el.TryCast<MenuInventoryBox>() != null ? Strings.Empty : Labels.Humanize(el.title);
    }

    /// <summary>
    /// What an unnamed button ("Button", "Button1") does: "close" if it turns the menu off,
    /// else the name of the script it runs. Null when it can't be told.
    /// </summary>
    private static string ButtonPurpose(MenuElement el)
    {
        var b = el.TryCast<MenuButton>();
        if (b == null || !Labels.IsPlaceholder(el.title)) return null;
        var list = b.actionList != null ? b.actionList.name : null;
        if (_loggedButtons.Add(el.Pointer)) Log.Debug("Menu", $"unnamed button {el.title}: click={b.buttonClickType} list={list}");
        if (b.buttonClickType == AC_ButtonClickType.TurnOffMenu) return Strings.Close;
        if (b.buttonClickType == AC_ButtonClickType.RunActionList && !string.IsNullOrEmpty(list))
            return Labels.Humanize(System.Text.RegularExpressions.Regex.Replace(list, "(?<=[a-z])(?=[A-Z])", " ")).ToLowerInvariant();
        return null;
    }

    /// <summary>A label's text, plus what its picture shows when the text leaves it out.</summary>
    private static string LabelText(MenuLabel label, int lang)
    {
        var text = TextClean.Clean(label.GetLabel(0, lang));
        var picture = Descriptions.MenuPicture(label.lineID);
        return picture == null || text.Length == 0 ? text : $"{text} {picture}";
    }

    /// <summary>The current text of another element in the same menu, found by its title.</summary>
    private static string ElementText(Menu m, string title, int lang)
    {
        var els = m.elements;
        for (var i = 0; els != null && i < els.Count; i++)
            if (els[i] != null && els[i].title == title)
                return TextClean.Clean(els[i].GetLabel(0, lang));
        return null;
    }

    /// <summary>The element's current setting, for elements whose label does not already carry it.</summary>
    internal static string Value(MenuElement el, int slot)
    {
        if (el.TryCast<MenuInventoryBox>() != null)
            return string.Format(Strings.Position, slot + 1, el.GetNumSlots());

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
