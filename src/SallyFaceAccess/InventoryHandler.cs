using Il2CppAC;

namespace SallyFaceAccess;

/// <summary>
/// Announces the item the player is holding to use. Browsing the inventory and examining
/// items are AC menus ("Inventory", "Item Close Up"), so MenuHandler reads those.
/// </summary>
internal static class InventoryHandler
{
    private static IntPtr _held;
    private static string _heldName;

    public static void Tick()
    {
        var inv = KickStarter.runtimeInventory;
        if (inv == null) return;

        var item = inv.SelectedItem;
        var ptr = item?.Pointer ?? IntPtr.Zero;
        if (ptr == _held) return;

        if (item != null)
        {
            var name = TextClean.Clean(item.GetLabel(Options.GetLanguage()));
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
