using Il2CppAC;
using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// The fight with the Endless One (05_endless_3_B): three mouths hang across the top
/// ("Mouth 1/2/3" = left, middle, right, each animating its child "mouth1".."mouth3"). Each one
/// wiggles ("endlessB_wiggle") and spits straight down its column ("endlessB_spit"); the spit
/// kills Sal if he is under it. R ("Blast") fires a bolt straight up that hits the mouth above
/// ("endlessB_hit"); a mouth dies after its hits ("endlessB_dead") and the fight ends when all
/// three are dead. Sal walks left and right only.
///
/// Spoken: where Sal is among the mouths as he walks, each wiggle (urgent, with "move" if he is
/// under it), hits and deaths. Delete toggles auto strike: Sal blasts whenever he stands under a
/// living mouth.
/// </summary>
internal static class Endless
{
    private static bool _active;
    private static readonly Transform[] _mouths = new Transform[3];
    private static readonly bool[] _dead = new bool[3];
    private static readonly string[] _clip = new string[3];
    private static string _place;
    private static string _pending;
    private static float _pendingSince;
    private static float _nextGather;

    private static string Name(int i) => i switch { 0 => Strings.Left, 1 => Strings.Middle, _ => Strings.Right };

    public static void OnSceneLoaded(string scene)
    {
        _active = scene == "05_endless_3_B";
        Array.Clear(_mouths);
        Array.Clear(_dead);
        Array.Clear(_clip);
        _place = _pending = null;
        _nextGather = 0;
    }

    /// <summary>The damage box's centre and half width: where the bolt hits and the spit falls.</summary>
    private static (float X, float Half) Column(int i)
    {
        var m = _mouths[i];
        var box = m?.Find("damageBox");
        if (box == null) return (m != null ? m.position.x : 0f, 1.5f);
        var col = box.GetComponent<BoxCollider2D>();
        return (box.position.x, col != null ? col.bounds.extents.x : 1.5f);
    }

    /// <summary>The mouth Sal stands under (its spit would hit him), or -1.</summary>
    private static int Under(float x)
    {
        for (var i = 0; i < 3; i++)
        {
            if (_mouths[i] == null) continue;
            var (cx, half) = Column(i);
            if (Math.Abs(x - cx) < half + 0.3f) return i;
        }
        return -1;
    }

    /// <summary>"under the left mouth", "between the left and middle mouths", "far left".</summary>
    private static string Place(float x)
    {
        var u = Under(x);
        if (u >= 0) return string.Format(_dead[u] ? Strings.UnderDeadMouth : Strings.UnderMouth, Name(u));
        if (_mouths[0] == null || _mouths[2] == null) return "";
        if (x < Column(0).X) return Strings.FarLeft;
        if (x > Column(2).X) return Strings.FarRight;
        var left = x < Column(1).X ? 0 : 1;
        return string.Format(Strings.BetweenMouths, Name(left), Name(left + 1));
    }

    /// <summary>An ActionAnim on a mouth's animator.</summary>
    public static void MouthAnim(Animator a, string clip)
    {
        if (!_active || a == null || clip == null || KickStarter.player == null) return;
        var i = a.gameObject.name switch { "mouth1" => 0, "mouth2" => 1, "mouth3" => 2, _ => -1 };
        if (i < 0) return;
        _mouths[i] = a.transform;
        if (_clip[i] == clip) return; // AC calls Run again while it waits
        _clip[i] = clip;
        var under = Under(KickStarter.player.transform.position.x) == i;
        switch (clip)
        {
            case "endlessB_wiggle":
                Speech.Say(string.Format(under ? Strings.MouthSpitsMove : Strings.MouthSpits, Name(i)), interrupt: true);
                break;
            case "endlessB_hit":
                Speech.Say(string.Format(Strings.MouthHit, Name(i)), interrupt: true);
                break;
            case "endlessB_dead":
                _dead[i] = true;
                var left = 3 - _dead.Count(d => d);
                Speech.Say(string.Format(Strings.MouthDead, Name(i), left), interrupt: true);
                break;
        }
    }

    public static void Tick()
    {
        if (!_active || KickStarter.player == null) return;
        if (Time.unscaledTime >= _nextGather)
        {
            _nextGather = Time.unscaledTime + 2f;
            for (var i = 0; i < 3; i++)
                if (_mouths[i] == null) _mouths[i] = GameObject.Find($"Mouth {i + 1}")?.transform.Find($"mouth{i + 1}");
        }

        if (Input.GetKeyDown(KeyCode.Delete)) Arena.ToggleAutoStrike();

        var x = KickStarter.player.transform.position.x;
        var under = Under(x);
        if (Arena.AutoStrikeOn && under >= 0 && !_dead[under]) Arena.AutoStrike("Blast");

        // Where Sal is, once he has settled there for a moment.
        var place = Place(x);
        if (place != _pending) { _pending = place; _pendingSince = Time.unscaledTime; }
        else if (place != _place && Time.unscaledTime - _pendingSince > 0.12f
                 && KickStarter.stateHandler?.gameState == GameState.Normal)
        {
            _place = place;
            if (place.Length > 0) Speech.Say(place, interrupt: false);
        }
    }
}
