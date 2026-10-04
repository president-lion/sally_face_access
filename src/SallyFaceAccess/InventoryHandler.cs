using Il2CppAC;

namespace SallyFaceAccess;

/// <summary>
/// Announces items gained and lost, and the item the player is holding to use. Browsing
/// the inventory and examining items are AC menus ("Inventory", "Item Close Up"), so
/// MenuHandler reads those.
/// </summary>
internal static class InventoryHandler
{
    private static IntPtr _held;
    private static string _heldName;
    private static readonly Dictionary<int, string> _carried = new();
    private static readonly Dictionary<int, string> _now = new();
    private static bool _baseline;

    /// <summary>A scene load may be a save being loaded: take a fresh baseline silently.</summary>
    public static void OnSceneLoaded() => _baseline = false;

    /// <summary>What to say as the inventory opens: "Inventory, 2 items" or "Inventory empty".</summary>
    public static string Summary(int lang)
    {
        var n = 0;
        var items = KickStarter.runtimeInventory?.localItems;
        for (var i = 0; items != null && i < items.Count; i++)
            if (items[i] != null) n++;
        return n switch
        {
            0 => Strings.InventoryEmpty,
            1 => Strings.InventoryOne,
            _ => string.Format(Strings.InventoryMany, n),
        };
    }

    public static void Tick()
    {
        var inv = KickStarter.runtimeInventory;
        if (inv == null) return;
        var lang = Options.GetLanguage();
        CheckCarried(inv, lang);
        CheckHeld(inv, lang);
    }

    private static void CheckCarried(RuntimeInventory inv, int lang)
    {
        var items = inv.localItems;
        if (items == null) return;
        _now.Clear();
        for (var i = 0; i < items.Count; i++)
        {
            var it = items[i];
            if (it != null && !_now.ContainsKey(it.id)) _now[it.id] = TextClean.Clean(it.GetLabel(lang));
        }

        if (_baseline)
        {
            foreach (var (id, name) in _now)
                if (!_carried.ContainsKey(id)) Speech.Say(string.Format(Strings.Got, name), interrupt: false);
            foreach (var (id, name) in _carried)
                if (!_now.ContainsKey(id)) Speech.Say(string.Format(Strings.Removed, name), interrupt: false);
        }
        _baseline = true;
        _carried.Clear();
        foreach (var kv in _now) _carried[kv.Key] = kv.Value;
    }

    private static void CheckHeld(RuntimeInventory inv, int lang)
    {
        var item = inv.SelectedItem;
        var ptr = item?.Pointer ?? IntPtr.Zero;
        if (ptr == _held) return;

        if (item != null)
        {
            var name = TextClean.Clean(item.GetLabel(lang));
            Speech.Say(string.Format(Strings.Holding, name));
            _heldName = name;
        }
        else if (_heldName != null)
        {
            Speech.Say(string.Format(Strings.PutAway, _heldName));
            _heldName = null;
        }
        _held = ptr;
    }
}
