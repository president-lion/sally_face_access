using Il2CppAC;
using UnityEngine;
using ActionListT = Il2CppSystem.Collections.Generic.List<Il2CppAC.Action>;

namespace SallyFaceAccess;

/// <summary>
/// Item hints (Hints setting: off / subtle / full). An object "reacts to" an item when it has a
/// use-item button for it, or when one of its interaction scripts checks for, swaps or takes
/// away that item - the way the sleep aid turns Sal's tea into sleepy tea. The hint holds until
/// the player interacts with the object while carrying the item.
/// </summary>
internal static class ItemHints
{
    private sealed class Info
    {
        public string Key;
        public List<int> ReactsTo = new();
        public List<IntPtr> Lists = new(); // the object's interaction ActionLists / assets
    }

    private static readonly ItemHintLogic _logic = new();
    private static readonly Dictionary<IntPtr, Info> _cache = new();
    private static readonly Dictionary<IntPtr, Info> _byList = new();
    private static readonly HashSet<IntPtr> _seenRunning = new();
    private static readonly HashSet<IntPtr> _nowRunning = new();
    private static string _scene = "";

    public static void OnSceneLoaded(string scene)
    {
        _scene = scene;
        _cache.Clear();
        _byList.Clear();
        _seenRunning.Clear();
    }

    /// <summary>The hint to add to this object's name, or null.</summary>
    public static string For(Hotspot h)
    {
        if (Settings.Hints == "off" || h == null) return null;
        var info = Get(h);
        if (info.ReactsTo.Count == 0) return null;
        var relevant = _logic.Relevant(info.Key, info.ReactsTo, Carried());
        if (relevant.Count == 0) return null;
        var lang = Options.GetLanguage();
        var names = relevant.Select(id => TextClean.Clean(KickStarter.inventoryManager?.GetItem(id)?.GetLabel(lang) ?? ""))
                            .Where(n => n.Length > 0);
        return ItemHintLogic.Wording(Settings.Hints, string.Join(Strings.And, names));
    }

    /// <summary>Marks objects as tried when one of their interaction scripts starts running.</summary>
    public static void Tick()
    {
        var lists = KickStarter.actionListManager?.activeLists;
        if (lists == null || _byList.Count == 0) return;
        _nowRunning.Clear();
        for (var i = 0; i < lists.Count; i++)
        {
            var a = lists[i];
            if (a == null || !a.IsRunning()) continue;
            var ptr = a.actionList?.Pointer ?? a.actionListAsset?.Pointer ?? IntPtr.Zero;
            if (ptr == IntPtr.Zero) continue;
            _nowRunning.Add(ptr);
            if (_seenRunning.Contains(ptr) || !_byList.TryGetValue(ptr, out var info)) continue;
            _logic.Tried(info.Key, Carried());
            Log.Debug("Hints", $"tried {info.Key}");
        }
        _seenRunning.Clear();
        _seenRunning.UnionWith(_nowRunning);
    }

    private static Info Get(Hotspot h)
    {
        if (_cache.TryGetValue(h.Pointer, out var info)) return info;
        info = new Info { Key = $"{_scene}/{h.gameObject.name}@{Math.Round(h.transform.position.x)}" };
        try
        {
            var buttons = new List<Il2CppAC.Button>();
            if (h.useButtons != null) for (var i = 0; i < h.useButtons.Count; i++) buttons.Add(h.useButtons[i]);
            if (h.invButtons != null) for (var i = 0; i < h.invButtons.Count; i++) buttons.Add(h.invButtons[i]);
            buttons.Add(h.lookButton);
            buttons.Add(h.useButton);

            var fromAsset = h.interactionSource == InteractionSource.AssetFile;
            foreach (var b in buttons)
            {
                if (b == null || b.isDisabled) continue;
                if (h.invButtons != null && h.invButtons.Contains(b) && b.invID >= 0) info.ReactsTo.Add(b.invID);

                ActionListT actions = null;
                if (fromAsset && b.assetFile != null) { actions = b.assetFile.actions; info.Lists.Add(b.assetFile.Pointer); }
                else if (b.interaction != null)
                {
                    info.Lists.Add(b.interaction.Pointer);
                    actions = b.interaction.source == ActionListSource.AssetFile && b.interaction.assetFile != null
                        ? b.interaction.assetFile.actions : b.interaction.actions;
                }
                for (var i = 0; actions != null && i < actions.Count; i++)
                {
                    var a = actions[i];
                    if (a == null || !a.isEnabled) continue;
                    var check = a.TryCast<ActionInventoryCheck>();
                    if (check != null && check.invID >= 0) info.ReactsTo.Add(check.invID);
                    // Swapping or taking an item away also means the object uses it (tea -> sleepy tea).
                    var set = a.TryCast<ActionInventorySet>();
                    if (set != null && set.invAction == InvAction.Replace && set.invIDReplace >= 0) info.ReactsTo.Add(set.invIDReplace);
                    if (set != null && set.invAction == InvAction.Remove && set.invID >= 0) info.ReactsTo.Add(set.invID);
                }
            }
        }
        catch (Exception ex) { Log.Once("ItemHints.Get", ex); }

        info.ReactsTo = info.ReactsTo.Distinct().ToList();
        foreach (var l in info.Lists) _byList[l] = info;
        _cache[h.Pointer] = info;
        return info;
    }

    private static HashSet<int> Carried()
    {
        var set = new HashSet<int>();
        var items = KickStarter.runtimeInventory?.localItems;
        for (var i = 0; items != null && i < items.Count; i++)
            if (items[i] != null) set.Add(items[i].id);
        return set;
    }
}
