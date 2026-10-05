using Il2CppAC;
using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// Puzzles whose state the game shows only as pictures, read from its global variables.
///   Cult temple floor seal (03_cult_Main): ring positions in variables 33 (right lever's
///   ring) and 34 (left lever's), 1-4 each, solved at 1 and 1.
/// The state is spoken once the rings stop turning after a pull, and added to the levers'
/// names in the room scan.
/// </summary>
internal static class Puzzles
{
    private const int RightRing = 33, LeftRing = 34;
    private const float Settle = 1.2f; // a pull turns the rings over about a second

    private static string _scene = "";
    private static string _last;
    private static string _pending;
    private static float _pendingSince;

    public static void OnSceneLoaded(string scene)
    {
        _scene = scene;
        _last = null;
        _pending = null;
    }

    public static void Tick()
    {
        if (_scene != "03_cult_Main") return;
        var state = Seal();
        if (state == null) return;
        if (_last == null) { _last = state; return; } // baseline: a load is not a pull
        if (state != _pending) { _pending = state; _pendingSince = Time.unscaledTime; }
        if (_pending != _last && Time.unscaledTime - _pendingSince >= Settle)
        {
            _last = _pending;
            Speech.Say(_last, interrupt: false);
        }
    }

    /// <summary>Extra words for an object's name in the room scan, or null.</summary>
    public static string For(Hotspot h)
    {
        if (_scene == "03_cult_Main" && h.gameObject.name is "pullL" or "pullR") return Seal();
        return null;
    }

    private static string Seal()
    {
        if (KickStarter.runtimeVariables == null) return null;
        var r = GlobalVariables.GetVariable(RightRing);
        var l = GlobalVariables.GetVariable(LeftRing);
        if (r == null || l == null || r.IntegerValue < 1 || l.IntegerValue < 1) return null;
        return PuzzleText.Seal(r.IntegerValue, l.IntegerValue);
    }
}
