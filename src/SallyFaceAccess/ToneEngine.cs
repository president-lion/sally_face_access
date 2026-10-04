using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// Placeholder sounds for the positional audio layer: tones generated in code and played
/// through Unity audio sources with pan, pitch and volume. Every cue goes through one named
/// method, so swapping a tone for a recorded sound later means changing <see cref="Make"/>
/// only (or loading a file there).
/// </summary>
internal static class ToneEngine
{
    public enum Cue { Tick, Thud, Chirp, Trap, Beacon, Hum, Step, Advance }

    private const int Rate = 44100;
    private const int OneShotVoices = 10;
    private const int LoopVoices = 6;

    private static GameObject _host;
    private static readonly Dictionary<Cue, AudioClip> _clips = new();
    private static readonly List<AudioSource> _oneShots = new();
    private static readonly List<AudioSource> _loops = new();
    private static int _next;

    /// <summary>Master volume for every cue, 0..1.</summary>
    public static float Volume = 0.6f;

    private static bool Ready()
    {
        if (_host != null) return true;
        _host = new GameObject("SallyFaceAccess.Tones");
        UnityEngine.Object.DontDestroyOnLoad(_host);
        _host.hideFlags = HideFlags.HideAndDontSave;
        for (var i = 0; i < OneShotVoices; i++) _oneShots.Add(NewSource(false));
        for (var i = 0; i < LoopVoices; i++) _loops.Add(NewSource(true));
        foreach (Cue c in Enum.GetValues(typeof(Cue))) _clips[c] = Make(c);
        Log.Info("Tones", "tone engine ready");
        return true;
    }

    private static AudioSource NewSource(bool loop)
    {
        var s = _host.AddComponent<AudioSource>();
        s.playOnAwake = false;
        s.loop = loop;
        s.spatialBlend = 0f; // 2D: placement is done with pan and pitch
        s.bypassReverbZones = true;
        return s;
    }

    /// <summary>Plays a one-shot cue at a placement.</summary>
    public static void Play(Cue cue, TopDownLogic.Placement p, float gain = 1f)
    {
        if (p.Volume <= 0.001f || !Ready()) return;
        var s = _oneShots[_next];
        _next = (_next + 1) % _oneShots.Count;
        s.Stop();
        s.clip = _clips[cue];
        s.panStereo = p.Pan;
        s.pitch = p.Pitch;
        s.volume = Math.Clamp(p.Volume * gain * Volume, 0f, 1f);
        s.Play();
    }

    /// <summary>
    /// Sets looping voice <paramref name="voice"/> (0..5) to a cue at a placement; a volume of
    /// zero silences it. Call every frame for moving things.
    /// </summary>
    public static void Loop(int voice, Cue cue, TopDownLogic.Placement p, float gain = 1f)
    {
        if (voice < 0 || voice >= LoopVoices || !Ready()) return;
        var s = _loops[voice];
        var vol = Math.Clamp(p.Volume * gain * Volume, 0f, 1f);
        if (vol <= 0.001f) { if (s.isPlaying) s.Stop(); return; }
        if (s.clip != _clips[cue]) { s.Stop(); s.clip = _clips[cue]; }
        s.panStereo = p.Pan;
        s.pitch = p.Pitch;
        s.volume = vol;
        if (!s.isPlaying) s.Play();
    }

    /// <summary>For the self-test: how many voices are sounding right now.</summary>
    public static int Playing => _oneShots.Concat(_loops).Count(s => s != null && s.isPlaying);

    public static void StopLoops()
    {
        foreach (var s in _loops) if (s != null && s.isPlaying) s.Stop();
    }

    public static void StopAll()
    {
        StopLoops();
        foreach (var s in _oneShots) if (s != null && s.isPlaying) s.Stop();
    }

    // ---- the placeholder tones ----------------------------------------------------------

    private static AudioClip Make(Cue cue) => cue switch
    {
        // Short, bright click: wall ahead.
        Cue.Tick => Tone("tick", 0.025f, t => Sine(1500, t), Decay(120)),
        // Low knock: walked into a wall.
        Cue.Thud => Tone("thud", 0.14f, t => Sine(110, t) + 0.4f * Sine(220, t), Decay(25)),
        // Rising sweep: a side passage opened.
        Cue.Chirp => Tone("chirp", 0.11f, t => Sine(500 + 9000 * t, t), Decay(12)),
        // Harsh buzz: a trap.
        Cue.Trap => Tone("trap", 0.12f, t => 0.6f * Square(180, t) + 0.3f * Square(271, t), Decay(10)),
        // Bell: the thing to reach.
        Cue.Beacon => Tone("beacon", 0.22f, t => Sine(880, t) + 0.3f * Sine(1760, t), Decay(14)),
        // Seamless loop (whole cycles): an enemy.
        Cue.Hum => Tone("hum", 1f, t => 0.6f * Triangle(150, t) + 0.25f * Sine(300, t) * (0.7f + 0.3f * Sine(3, t)), _ => 1f),
        // Soft, dull white-noise pluck: a footstep where the game plays none.
        Cue.Step => SoftNoise("step", 0.08f, decay: 55f, smoothing: 0.18f),
        // E-flat major chord (E-flat, G, B-flat): a dialogue line advanced.
        Cue.Advance => Tone("advance", 0.45f, t => (Sine(311.13f, t) + Sine(392.00f, t) + Sine(466.16f, t)) / 2.2f, Decay(7)),
        _ => Tone("silence", 0.01f, _ => 0f, _ => 0f),
    };

    private static float Sine(float hz, float t) => MathF.Sin(2 * MathF.PI * hz * t);
    private static float Square(float hz, float t) => Sine(hz, t) >= 0 ? 1f : -1f;
    private static float Triangle(float hz, float t) => 2f * MathF.Abs(2f * (t * hz - MathF.Floor(t * hz + 0.5f))) - 1f;
    private static Func<float, float> Decay(float k) => t => MathF.Exp(-k * t);

    /// <summary>White noise through a one-pole low-pass (smaller smoothing = duller), with an exponential fade.</summary>
    private static AudioClip SoftNoise(string name, float seconds, float decay, float smoothing)
    {
        var rng = new System.Random(7);
        var n = (int)(seconds * Rate);
        var data = new Il2CppStructArray<float>(n);
        var y = 0f;
        for (var i = 0; i < n; i++)
        {
            var t = (float)i / Rate;
            y += smoothing * ((float)(rng.NextDouble() * 2 - 1) - y);
            var attack = Math.Min(1f, i / (Rate * 0.003f));
            data[i] = 1.6f * y * MathF.Exp(-decay * t) * attack;
        }
        return Clip(name, data);
    }

    private static AudioClip Clip(string name, Il2CppStructArray<float> data)
    {
        var clip = AudioClip.Create($"SallyFaceAccess.{name}", data.Length, 1, Rate, false);
        clip.SetData(data, 0);
        clip.hideFlags = HideFlags.DontUnloadUnusedAsset;
        return clip;
    }

    private static AudioClip Tone(string name, float seconds, Func<float, float> wave, Func<float, float> envelope)
    {
        var n = (int)(seconds * Rate);
        var data = new Il2CppStructArray<float>(n);
        for (var i = 0; i < n; i++)
        {
            var t = (float)i / Rate;
            var attack = Math.Min(1f, i / (Rate * 0.002f)); // 2 ms fade-in: no click
            data[i] = 0.5f * wave(t) * envelope(t) * attack;
        }
        return Clip(name, data);
    }
}
