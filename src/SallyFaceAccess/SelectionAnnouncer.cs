namespace SallyFaceAccess;

/// <summary>
/// Decides what to say as the selected menu item is polled each frame: the full
/// "label, value" when the selection moves, only the value when the same item's value
/// changes (a slider being dragged), and nothing when nothing changed.
/// Pure logic - no Unity types - so it is unit tested.
/// </summary>
internal sealed class SelectionAnnouncer
{
    private string _key, _label, _value;

    /// <returns>Text to speak, or null for silence.</returns>
    public string Next(string key, string label, string value)
    {
        string say;
        if (key != _key) say = Join(label, value);
        else if (label != _label) say = Join(label, value);
        else if (value != _value) say = value;
        else say = null;

        _key = key;
        _label = label;
        _value = value;
        return say;
    }

    public void Clear() => _key = _label = _value = null;

    private static string Join(string label, string value) =>
        string.IsNullOrEmpty(value) ? label : $"{label}, {value}";
}
