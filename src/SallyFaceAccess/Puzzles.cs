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
///
/// Skippable puzzles (the switch rooms) say so on entry; Delete solves them by setting the
/// switches and running the game's own unlock script, so the game sees a normal solve.
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
        if (IsSwitchRoom && Input.GetKeyDown(KeyCode.Delete)) Skip();
        var state = State();
        if (state == null) return;
        if (_last == null)
        {
            // Baseline: a load is not a move. The switch rooms say where they start.
            _last = state;
            if (IsSwitchRoom)
            {
                Speech.Say(state, interrupt: false);
                if (!GateOpen()) Speech.Say(Strings.SkipAvailable, interrupt: false);
            }
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
        if (_scene == "05_alt_House" && DialVar(h.gameObject.name) is int v)
            return PuzzleText.HexDial(GlobalVariables.GetVariable(v)?.IntegerValue ?? 1);
        return null;
    }

    private static bool IsSwitchRoom => _scene is "03_cult_L2" or "03_cult_S2";

    private static string State() =>
        _scene == "03_cult_Main" ? Seal() : IsSwitchRoom ? Switches() : _scene == "05_alt_House" ? Dials() : null;

    // The other house's hexagon dials (Episode 5): "gear 1 hs" is the right dial (variable 85),
    // "gear 2 hs" the middle (86, and turns the right one too), "gear 3 hs" the left (87, and
    // turns the middle one). Upright is 1; the door opens when all three are.
    private static int? DialVar(string obj) => obj switch
    {
        "gear 1 hs" => 85, "gear 2 hs" => 86, "gear 3 hs" => 87, _ => null,
    };

    private static string Dials()
    {
        if (KickStarter.runtimeVariables == null) return null;
        int V(int id) => GlobalVariables.GetVariable(id)?.IntegerValue ?? 1;
        return PuzzleText.HexDials(V(87), V(86), V(85));
    }

    // The switch rooms' gate: variable 38 is 55 once it has been opened.
    private static bool GateOpen() => GlobalVariables.GetVariable(38)?.IntegerValue == 55;

    private static void Skip()
    {
        if (KickStarter.stateHandler?.gameState != GameState.Normal) return;
        if (GateOpen()) { Speech.Say(Strings.GateAlreadyOpen); return; }
        ActionList unlock = null;
        foreach (var l in UnityEngine.Object.FindObjectsOfType<ActionList>())
            if (l != null && l.gameObject.name == "doorUnlocked") { unlock = l; break; }
        if (unlock == null) { Log.Warn("Puzzles", $"no doorUnlocked in {_scene}"); return; }
        for (var i = 0; i < 5; i++) GlobalVariables.GetVariable(33 + i)?.SetValue(1);
        Log.Info("Puzzles", $"skipped the switch puzzle in {_scene}");
        Speech.Say(Strings.PuzzleSkipped);
        _last = Switches(); // no "switches down" announcement for the skip itself
        unlock.Interact();
    }

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
