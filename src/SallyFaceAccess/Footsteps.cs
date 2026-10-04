using HarmonyLib;
using Il2CppAC;
using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// A soft noise footstep while the player walks somewhere the game plays none (the dream
/// fields, some handheld games). The game's own footsteps are detected by listening to the
/// player's audio sources - every source on the player other than its voice, plus the ones
/// AC's FootstepSounds play through - since the school's footsteps bypass FootstepSounds'
/// methods. While any of them plays, the mod stays quiet.
/// </summary>
internal static class Footsteps
{
    private static float _lastGameStep = -100f, _lastOwnStep = -100f;
    private static Vector3 _lastPos;
    private static readonly System.Random _rng = new();

    private static readonly List<AudioSource> _sources = new();
    private static IntPtr _sourcesFor;
    private static float _nextGather;
    private static string _lastSummary;

    public static void OnSceneLoaded()
    {
        _lastGameStep = -100f;
        _sourcesFor = IntPtr.Zero;
    }

    /// <summary>The audio sources the player's footsteps could come from.</summary>
    private static void Gather(Player player)
    {
        _sources.Clear();
        _sourcesFor = player.Pointer;
        var names = new List<string>();
        foreach (var a in player.GetComponentsInChildren<AudioSource>(true))
            if (a != null && a != player.speechAudioSource) { _sources.Add(a); names.Add(a.gameObject.name); }
        foreach (var f in player.GetComponentsInChildren<FootstepSounds>(true))
        {
            var a = f?.soundToPlayFrom?.audioSource;
            if (a != null && !_sources.Contains(a)) { _sources.Add(a); names.Add("footsteps:" + a.gameObject.name); }
        }
        var summary = string.Join(", ", names);
        if (summary != _lastSummary) Log.Debug("Footsteps", $"listening to {_sources.Count} source(s): {summary}");
        _lastSummary = summary;
    }

    private static void Listen()
    {
        foreach (var a in _sources)
            if (a != null && a.isPlaying)
            {
                if (Time.unscaledTime - _lastGameStep > 5f) Log.Debug("Footsteps", $"game sound on the player: {a.gameObject.name} '{a.clip?.name}'");
                _lastGameStep = Time.unscaledTime;
                return;
            }
    }

    public static void Tick()
    {
        var player = KickStarter.player;
        if (player == null) return;
        // Re-gather now and then: footstep sets and players change with the story.
        if (player.Pointer != _sourcesFor || Time.unscaledTime >= _nextGather)
        {
            _nextGather = Time.unscaledTime + 10f;
            Gather(player);
        }
        Listen();
        var pos = player.transform.position;
        var moved = (pos - _lastPos).magnitude / Math.Max(Time.deltaTime, 0.001f);
        _lastPos = pos;
        // Walking means the player is moving on their own in normal play, not being moved by a cutscene.
        var moving = moved > 0.05f && KickStarter.stateHandler?.gameState == GameState.Normal && Time.timeScale > 0f;
        var now = Time.unscaledTime;
        if (!FootstepLogic.ShouldStep(now, _lastGameStep, _lastOwnStep, moving, player.isRunning)) return;
        _lastOwnStep = now;
        var pitch = 0.9f + (float)_rng.NextDouble() * 0.2f;
        ToneEngine.Play(ToneEngine.Cue.Step, new TopDownLogic.Placement(0f, pitch, 1f), 0.5f);
    }

    private static bool IsPlayers(FootstepSounds f)
    {
        var c = f.character;
        return c != null ? c.IsPlayer : f.GetComponentInParent<Player>() != null;
    }

    private static void Heard(FootstepSounds f, bool hasSound, string via)
    {
        try
        {
            if (!hasSound || !IsPlayers(f)) return;
            if (Time.unscaledTime - _lastGameStep > 5f) Log.Debug("Footsteps", $"game footstep ({via})");
            _lastGameStep = Time.unscaledTime;
        }
        catch { }
    }

    private static bool Any(Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<AudioClip> clips)
    {
        for (var i = 0; clips != null && i < clips.Length; i++) if (clips[i] != null) return true;
        return false;
    }

    // The game played a footstep for the player: this area has its own footsteps. All three
    // routes are watched in case IL2CPP inlined one into another.
    [HarmonyPatch(typeof(FootstepSounds), "PlaySound", new[] { typeof(AudioClip), typeof(bool) })]
    private static class OneClip
    {
        private static void Prefix(FootstepSounds __instance, AudioClip __0) => Heard(__instance, __0 != null, "clip");
    }

    [HarmonyPatch(typeof(FootstepSounds), "PlaySound", new[] { typeof(Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<AudioClip>), typeof(bool) })]
    private static class ClipSet
    {
        private static void Prefix(FootstepSounds __instance, Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppReferenceArray<AudioClip> __0) =>
            Heard(__instance, Any(__0), "set");
    }

    [HarmonyPatch(typeof(FootstepSounds), nameof(FootstepSounds.PlayFootstep))]
    private static class Footstep
    {
        private static void Prefix(FootstepSounds __instance) =>
            Heard(__instance, Any(__instance.footstepSounds) || Any(__instance.runSounds), "footstep");
    }
}
