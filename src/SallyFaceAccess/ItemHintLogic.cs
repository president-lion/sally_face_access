namespace SallyFaceAccess;

/// <summary>
/// Decides when an object deserves an item hint: some item it reacts to is being carried, and
/// the player has not yet interacted with it while carrying that item.
/// Pure logic - no Unity types - so it is unit tested.
/// </summary>
internal sealed class ItemHintLogic
{
    // Object key -> item IDs the player has already tried it with.
    private readonly Dictionary<string, HashSet<int>> _tried = new();

    /// <summary>The items worth hinting at for this object, in the order the object lists them.</summary>
    public IReadOnlyList<int> Relevant(string objectKey, IEnumerable<int> reactsTo, IEnumerable<int> carried)
    {
        var have = carried as ISet<int> ?? new HashSet<int>(carried);
        _tried.TryGetValue(objectKey, out var tried);
        return reactsTo.Distinct().Where(id => have.Contains(id) && (tried == null || !tried.Contains(id))).ToList();
    }

    /// <summary>The player interacted with the object while carrying these items.</summary>
    public void Tried(string objectKey, IEnumerable<int> carried)
    {
        if (!_tried.TryGetValue(objectKey, out var set)) _tried[objectKey] = set = new HashSet<int>();
        set.UnionWith(carried);
    }

    public void Clear() => _tried.Clear();

    /// <summary>The hint as spoken for a mode ("off", "subtle", "full"), or null for none.</summary>
    public static string Wording(string mode, string itemNames) => mode switch
    {
        "full" => string.Format(Strings.HintFull, itemNames),
        "subtle" => Strings.HintSubtle,
        _ => null,
    };
}
