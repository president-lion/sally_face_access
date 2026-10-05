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

    // Labels the game shows as placeholders.
    private static readonly HashSet<string> EmptyLabels = new() { "???", "...", ". . .", "?", "" };

    // First words that make a label a prompt ("talk", "climb down") rather than a name.
    private static readonly HashSet<string> Verbs = new(StringComparer.OrdinalIgnoreCase)
    {
        "open", "close", "push", "pull", "talk", "speak", "check", "leave", "climb", "answer", "use",
        "look", "examine", "inspect", "pick", "take", "grab", "enter", "exit", "read", "knock", "ring",
        "press", "go", "sit", "sleep", "turn", "switch", "flip", "search", "watch", "play", "listen",
        "call", "give", "wake", "move", "jump", "crawl", "hide", "unlock", "lock", "drink", "eat",
    };

    // Words in internal object names that are scaffolding, not part of the thing's name.
    private static readonly HashSet<string> ObjectNoise = new(StringComparer.OrdinalIgnoreCase)
        { "hs", "hotspot", "spot", "trigger", "on", "off", "up", "down", "idle" };

    /// <summary>
    /// The spoken name of an interactive object. The game's label is a noun ("living room",
    /// "Gear Boy") or a prompt ("talk", "climb down"); a noun label is the name, a prompt is
    /// kept after a name taken from the table or the internal object name.
    /// </summary>
    public static string HotspotName(string tableName, string objectName, string label)
    {
        var l = TextClean.Clean(label);
        if (EmptyLabels.Contains(l)) l = "";
        var isPrompt = l.Length > 0 && Verbs.Contains(l.Split(' ')[0]);

        if (!string.IsNullOrEmpty(tableName))
            return l.Length == 0 || tableName.Contains(l, StringComparison.OrdinalIgnoreCase) ? tableName : $"{tableName}, {l}";

        var noun = ObjectNoun(objectName);
        if (l.Length == 0) return noun;
        if (!isPrompt) return noun == "door" && !l.Equals("door", StringComparison.OrdinalIgnoreCase) ? string.Format(Strings.DoorTo, l) : l;
        if (noun.Length == 0 || l.Contains(noun, StringComparison.OrdinalIgnoreCase)) return l;
        return $"{noun}, {l}";
    }

    /// <summary>The thing an internal object name refers to: "pickUpKey01" -> "key", "dadHS" -> "dad".</summary>
    private static string ObjectNoun(string objectName)
    {
        var w = Words(objectName);
        if (w.Length == 0 || GenericObject.IsMatch(w)) return "";
        var words = w.Split(' ').Where(x => !ObjectNoise.Contains(x) && !x.All(char.IsDigit)).ToList();
        // A leading verb names the action, not the object ("push drawer", "pick up key").
        while (words.Count > 1 && Verbs.Contains(words[0])) words.RemoveAt(0);
        while (words.Count > 1 && Verbs.Contains(words[^1])) words.RemoveAt(words.Count - 1); // "lisaTalkHS"
        // A lone letter is a sign on the thing itself ("door D"), so it reads as a capital.
        return string.Join(" ", words.Select(x => x.Length == 1 ? x.ToUpperInvariant() : x));
    }

    // Every hallway of Addison Apartments has the same layout: doors 1 and 2 left of the
    // elevator, 3 and 4 right of it, at these x positions in every episode's version.
    private static readonly float[] DoorSlots = { -19.8f, -12.1f, 3.15f, 10.8f };
    private static readonly Regex HallwayScene = new(@"^(\d\d_)?((\d|os)_)?F([1-5])$", RegexOptions.Compiled);
    private static readonly Regex DoorLike = new(@"^$|door|knock|open|^\d{3}$|^(knock|open) \d{3}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>The floor number of an Addison Apartments hallway scene ("02_F3" -> 3), or 0.</summary>
    public static int HallwayFloor(string scene)
    {
        var m = HallwayScene.Match(scene ?? "");
        return m.Success ? m.Groups[4].Value[0] - '0' : 0;
    }

    /// <summary>"apartment 403" for a door in a hallway scene, from the floor and the door's position.</summary>
    public static string HallwayDoor(string scene, string objectName, float x)
    {
        var m = HallwayScene.Match(scene ?? "");
        if (!m.Success || !DoorLike.IsMatch((objectName ?? "").Trim())) return null;
        for (var i = 0; i < DoorSlots.Length; i++)
            if (Math.Abs(x - DoorSlots[i]) < 1.2f)
                return string.Format(Strings.Apartment, $"{m.Groups[4].Value}0{i + 1}");
        return null;
    }

    /// <summary>Numbers one of several same-named things: "door, open" + 2 -> "door 2, open".</summary>
    public static string Numbered(string name, int n)
    {
        var i = name.IndexOf(", ", StringComparison.Ordinal);
        return i < 0 ? $"{name} {n}" : $"{name[..i]} {n}{name[i..]}";
    }

    /// <summary>
    /// The room a camera shows, from its name ("salsRoomCam" -> "sals room"), or null for the
    /// area's main camera ("NavCam 1", "GameCamera2D"), which is just the area itself.
    /// </summary>
    public static string RoomFromCamera(string camera)
    {
        if (string.IsNullOrWhiteSpace(camera)) return null;
        var w = Words(camera).Split(' ').Where(x => x != "cam" && x != "camera" && !x.All(char.IsDigit)).ToList();
        if (w.Count == 0 || w.Contains("nav") || w.Contains("game")) return null;
        return string.Join(" ", w);
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
    /// <summary>
    /// A name for an object the game left unnamed, from the script it runs: "tv: Use" -> "tv",
    /// "enterLarsRoom" -> "enter lars room", "talk: Larry" -> "Larry".
    /// </summary>
    public static string NameFromScript(string script)
    {
        if (string.IsNullOrWhiteSpace(script)) return "";
        var parts = script.Split(':', 2);
        var left = parts[0].Trim();
        var right = parts.Length > 1 ? parts[1].Trim() : "";
        // "talk: Larry": the verb comes first, the thing after the colon.
        if (right.Length > 0 && Verbs.Contains(Words(left)) && !Verbs.Contains(Words(right).Split(' ')[0])) return right;
        return Words(left);
    }

    private static string Words(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "";
        var s = Regex.Replace(name, @"\([^)]*\)", " ");
        s = Regex.Replace(s, @"(?<=[a-z])(?=[A-Z])|(?<=[A-Za-z])(?=\d)|(?<=\d)(?=[A-Za-z])", " ");
        s = Regex.Replace(s, @"[_\-\s]+", " ");
        return s.Trim().ToLowerInvariant();
    }
}
