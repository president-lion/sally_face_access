using Il2CppAC;
using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// Puzzles whose state the game shows only as pictures, read from its global variables.
///   Cult temple floor seal (03_cult_Main): ring positions in variables 33 (right lever's
///   ring) and 34 (left lever's), 1-4 each, solved at 1 and 1.
///   Floor switch rooms (03_cult_L2, 03_cult_S2): five switches in variables 33-37, 1 = down;
///   the gate opens when all are down.
/// The state is spoken once it settles after a move, and added to the puzzle's objects' names
/// in the room scan.
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
        var state = State();
        if (state == null) return;
        if (_last == null)
        {
            // Baseline: a load is not a move. The switch rooms say where they start.
            _last = state;
            if (IsSwitchRoom) Speech.Say(state, interrupt: false);
            return;
        }
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

    private static bool IsSwitchRoom => _scene is "03_cult_L2" or "03_cult_S2";

    private static string State() => _scene == "03_cult_Main" ? Seal() : IsSwitchRoom ? Switches() : null;

    private static string Switches()
    {
        if (KickStarter.runtimeVariables == null) return null;
        var down = new bool[5];
        for (var i = 0; i < 5; i++)
        {
            var v = GlobalVariables.GetVariable(33 + i);
            if (v == null) return null;
            down[i] = v.IntegerValue == 1;
        }
        return PuzzleText.Switches(down);
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
