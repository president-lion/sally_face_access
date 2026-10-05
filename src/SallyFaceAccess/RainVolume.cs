using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// Turns the game's rain down (RainVolume setting): heavy rain loops drown out speech in
/// Episode 4's night scenes. Every playing sound whose clip is named for rain is scaled
/// after the game has set its volume for the frame, so the game's own fades still work.
/// </summary>
internal static class RainVolume
{
    // The volume each source had after scaling, to tell the game's changes from ours.
    private static readonly Dictionary<int, float> _scaled = new();
    private static AudioSource[] _sources = System.Array.Empty<AudioSource>();
    private static float _nextGather;

    public static void LateTick()
    {
        var factor = Settings.RainVolume;
        if (factor >= 1f) return;

        // Finding sources is slow; new ones appear only with scene changes and spawned rain.
        if (Time.unscaledTime >= _nextGather)
        {
            _nextGather = Time.unscaledTime + 2f;
            _sources = UnityEngine.Object.FindObjectsOfType<AudioSource>();
        }

        foreach (var s in _sources)
        {
            if (s == null || !s.isPlaying || s.clip == null) continue;
            if (s.clip.name.IndexOf("rain", StringComparison.OrdinalIgnoreCase) < 0) continue;
            var id = s.GetInstanceID();
            // Unchanged since we scaled it: leave it, or it would shrink every frame.
            if (_scaled.TryGetValue(id, out var v) && Mathf.Approximately(s.volume, v)) continue;
            if (!_scaled.ContainsKey(id)) Log.Debug("Rain", $"turning down '{s.clip.name}' on {s.gameObject.name}");
            s.volume *= factor;
            _scaled[id] = s.volume;
        }
    }
}
