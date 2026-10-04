using System.Text.RegularExpressions;

namespace SallyFaceAccess;

/// <summary>
/// Wording for the world: object names, direction and distance, and area names.
/// Pure logic - no Unity types - so it is unit tested.
/// </summary>
internal static class WorldText
{
    // Object names that say nothing about the object.
    private static readonly Regex GenericObject = new(@"^(hotspot|hs|game ?object|object|trigger|interaction|collider|new)\b",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    // Verbs the game shows as placeholders.
    private static readonly HashSet<string> EmptyVerbs = new() { "???", "...", "?", "" };

    /// <summary>
    /// "name, verb" for an interactive object. The name comes from the table, else from the
    /// object's internal name if that is informative; the verb is the game's prompt.
    /// </summary>
    public static string HotspotName(string tableName, string objectName, string verb)
    {
        var v = TextClean.Clean(verb);
        if (EmptyVerbs.Contains(v)) v = "";

        var name = tableName;
        if (string.IsNullOrEmpty(name))
        {
            var readable = Words(objectName);
            if (readable.Length > 0 && !GenericObject.IsMatch(readable)) name = readable;
        }

        if (string.IsNullOrEmpty(name)) return v;
        if (v.Length == 0 || v.Equals(name, StringComparison.OrdinalIgnoreCase)) return name;
        return $"{name}, {v}";
    }

    /// <summary>"right, 3 steps", "left, 1 step", or "here" for an x offset from the player.</summary>
    public static string Direction(float dx, float stepSize)
    {
        var steps = Math.Abs(dx) / stepSize;
        if (steps < 0.5f) return Strings.Here;
        var n = Math.Max(1, (int)Math.Round(steps));
        var side = dx > 0 ? Strings.Right : Strings.Left;
        return $"{side}, {(n == 1 ? Strings.OneStep : string.Format(Strings.Steps, n))}";
    }

    private static readonly Regex EpisodePrefix = new(@"^(\d\d_)?((os|2|3)_)?", RegexOptions.Compiled);

    /// <summary>A readable area name generated from a scene name.</summary>
    public static string SceneName(string scene)
    {
        if (string.IsNullOrWhiteSpace(scene)) return "";
        var s = EpisodePrefix.Replace(scene.Trim(), "");
        s = Regex.Replace(s, @"\s+\d+$", "");

        Match m;
        if ((m = Regex.Match(s, @"^F(\d)$")).Success) return string.Format(Strings.FloorHallway, m.Groups[1].Value);
        if (s == "Fb") return Strings.Basement;
        if ((m = Regex.Match(s, @"^Fbr(\d)$")).Success) return string.Format(Strings.BasementRoom, m.Groups[1].Value);
        if ((m = Regex.Match(s, @"^F(\d)r(\d)$")).Success)
            return string.Format(Strings.FloorRoom, m.Groups[1].Value, m.Groups[2].Value);
        if ((m = Regex.Match(s, @"^(\d{3})(_\d)?$")).Success) return string.Format(Strings.Apartment, m.Groups[1].Value);

        // Digits glued to words are variant numbers (house1), not part of the name.
        return Words(Regex.Replace(s, @"(?<=[a-zA-Z])\d+", ""));
    }

    /// <summary>Splits camelCase, underscores and digits into lower-case words.</summary>
    private static string Words(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "";
        var s = Regex.Replace(name, @"\([^)]*\)", " ");
        s = Regex.Replace(s, @"(?<=[a-z])(?=[A-Z])|(?<=[A-Za-z])(?=\d)|(?<=\d)(?=[A-Za-z])", " ");
        s = Regex.Replace(s, @"[_\-\s]+", " ");
        return s.Trim().ToLowerInvariant();
    }
}
