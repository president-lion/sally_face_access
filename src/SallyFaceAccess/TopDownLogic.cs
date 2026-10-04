using System.Text.RegularExpressions;

namespace SallyFaceAccess;

/// <summary>
/// The arithmetic of the top-down audio layer (Super Gear Boy): where a sound sits for a
/// thing at an offset from the player, how fast walls tick, when a side passage opens, and
/// how a 2D offset is said. Screen up is world +y.
/// Pure logic - no Unity types - so it is unit tested.
/// </summary>
internal static class TopDownLogic
{
    /// <summary>One "step" in speech: the spacing of the games' floor tiles.</summary>
    public const float Step = 0.32f;

    public readonly record struct Placement(float Pan, float Pitch, float Volume);

    /// <summary>
    /// Stereo pan from the horizontal offset, pitch from the vertical one (higher = above),
    /// volume falling to nothing at <paramref name="range"/>.
    /// </summary>
    public static Placement Place(float dx, float dy, float range)
    {
        var dist = MathF.Sqrt(dx * dx + dy * dy);
        var pan = Math.Clamp(dx / (range * 0.5f), -1f, 1f);
        var pitch = 1f + Math.Clamp(dy / range, -1f, 1f) * 0.5f;
        var near = Math.Clamp(1f - dist / range, 0f, 1f);
        return new Placement(pan, pitch, near * near);
    }

    /// <summary>Seconds between wall ticks at this distance; null when the wall is too far to tick.</summary>
    public static float? TickInterval(float distance, float range = 1.0f)
    {
        if (distance > range) return null;
        return 0.06f + Math.Max(0f, distance) / range * 0.4f;
    }

    /// <summary>Distance from a point to an axis-aligned rectangle (0 inside it).</summary>
    public static float RectDistance(float px, float py, float cx, float cy, float w, float h)
    {
        var dx = Math.Max(Math.Abs(px - cx) - w / 2, 0f);
        var dy = Math.Max(Math.Abs(py - cy) - h / 2, 0f);
        return MathF.Sqrt(dx * dx + dy * dy);
    }

    /// <summary>The point of a rectangle nearest to a point.</summary>
    public static (float X, float Y) NearestPoint(float px, float py, float cx, float cy, float w, float h) =>
        (Math.Clamp(px, cx - w / 2, cx + w / 2), Math.Clamp(py, cy - h / 2, cy + h / 2));

    /// <summary>"up 3, right 2"; "here" when within half a step both ways.</summary>
    public static string Direction(float dx, float dy)
    {
        var parts = new List<string>();
        var v = (int)MathF.Round(Math.Abs(dy) / Step);
        var h = (int)MathF.Round(Math.Abs(dx) / Step);
        if (v > 0) parts.Add($"{(dy > 0 ? Strings.Up : Strings.Down)} {v}");
        if (h > 0) parts.Add($"{(dx > 0 ? Strings.Right : Strings.Left)} {h}");
        return parts.Count == 0 ? Strings.Here : string.Join(", ", parts);
    }

    /// <summary>Wall distance in steps for the F7 report: "3", or "open" beyond the probe.</summary>
    public static string WallSteps(float? distance) =>
        distance == null ? Strings.Open : ((int)MathF.Round(distance.Value / Step)).ToString();

    // Scripts a trap trigger runs: "1_spikeHit", "2_electricHurt3", "7_electricHurt1".
    private static readonly Regex TrapList = new(@"(spike|electric)\w*(Hit|Hurt)", RegexOptions.IgnoreCase);
    private static readonly Regex EnemyList = new(@"cultHit", RegexOptions.IgnoreCase);

    public static bool IsTrapList(string name) => name != null && TrapList.IsMatch(name);

    public static bool IsEnemyList(string name) => name != null && EnemyList.IsMatch(name);

    /// <summary>What to say when a game script starts, for the ones that are deaths or resets.</summary>
    public static string Event(string listName)
    {
        if (listName == null) return null;
        if (Regex.IsMatch(listName, @"spikeHit", RegexOptions.IgnoreCase)) return Strings.HitSpikes;
        if (Regex.IsMatch(listName, @"electricHurt", RegexOptions.IgnoreCase)) return Strings.HitElectric;
        if (IsEnemyList(listName)) return Strings.HitCultist;
        if (Regex.IsMatch(listName, @"timesUp", RegexOptions.IgnoreCase)) return Strings.TimesUp;
        return null;
    }
}

/// <summary>
/// Says when a side passage opens: the wall beside the player was close and now is not.
/// Hysteresis keeps a ragged wall edge from chirping repeatedly.
/// </summary>
internal sealed class OpeningDetector
{
    private readonly float _closed, _open;
    private bool _wasClosed;

    public OpeningDetector(float closed = 0.3f, float open = 0.6f)
    {
        _closed = closed;
        _open = open;
    }

    /// <summary>Feed the side distance (null = nothing within the probe); true once when it opens.</summary>
    public bool Update(float? distance, bool moving)
    {
        var d = distance ?? float.MaxValue;
        if (d < _closed) { _wasClosed = true; return false; }
        if (d > _open && _wasClosed)
        {
            _wasClosed = false;
            return moving;
        }
        return false;
    }

    public void Reset() => _wasClosed = false;
}
