using Il2CppAC;
using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// The cult temple's stealth section (Episode 5). The rooms are two rows of wide halls; a
/// patrolling cultist shuts the doorways on his route (the game turns their hotspots off)
/// while he walks and looks around, standing guards catch anyone who touches them, and a
/// broken jar starts a 12-second count before the room is searched.
///   - Doorways: a shut one stays in the room scan marked "blocked", and the change either
///     way is spoken for the room the player is in.
///   - Cultists: touching one (his 5.5-wide capture box) is capture, whichever way he faces.
///     Each one in the player's row hums from where he stands, higher while he is
///     coming closer.
///   - The jar timer (the game's autoKillTimer list): spoken at the start, then counted down.
/// </summary>
internal static class TempleStealth
{
    private const float RowHeight = 10f;   // the two rows of halls are 50 units apart
    private const float HumRange = 45f;    // about one hall
    private const int Voices = 4;
    private const float TimerLength = 12f; // autoKillTimer's pause

    private static bool _active;
    private static readonly Dictionary<IntPtr, bool> _doorOn = new();
    private static List<Transform> _cultists = new();
    private static readonly Dictionary<int, float> _lastGap = new();
    private static readonly Dictionary<int, bool> _closing = new();
    private static float _nextGather, _nextDoors;
    private static float _timerStart = -1f;
    private static int _lastCount;

    public static bool Active => _active;

    // The C-4 jars: West 33, East 34, Inner West 35, Inner East 36 (the last, in Inner East).
    private const int FirstJarVar = 33, LastJarVar = 36, C4Item = 56;
    private static bool _offered;

    public static void OnSceneLoaded(string scene)
    {
        if (_active) ToneEngine.StopLoops();
        _active = scene.StartsWith("05_temple");
        _doorOn.Clear();
        _cultists.Clear();
        _lastGap.Clear();
        _closing.Clear();
        _nextGather = 0;
        _nextDoors = 0;
        _timerStart = -1f;
        _offered = false;
        _autoStep = -1;
        _killTimer = null;
    }

    /// <summary>A doorway a patrol can shut: "hs_W-N", not the locked doors.</summary>
    public static bool IsGuardedDoor(Hotspot h) =>
        _active && h != null && h.gameObject.name.StartsWith("hs_") && !h.gameObject.name.StartsWith("hs_locked");

    /// <summary>Extra words for the room scan: "blocked" for a shut doorway.</summary>
    public static string For(Hotspot h) => IsGuardedDoor(h) && !h.IsOn() ? Strings.Blocked : null;

    public static void Tick()
    {
        if (!_active || KickStarter.player == null) return;
        OfferSkip();
        if (IsTemple1 && Input.GetKeyDown(KeyCode.Delete)) Skip();
        Autoplay();
        if (Autoplaying) return;
        var p = KickStarter.player.transform.position;
        if (Time.unscaledTime >= _nextDoors) { _nextDoors = Time.unscaledTime + 0.25f; Doors(p); }
        Hum(p);
        Timer();
    }

    private static bool IsTemple1 => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "05_temple1";

    private static bool AllPlaced => (GlobalVariables.GetVariable(LastJarVar)?.IntegerValue ?? 0) == 1;

    private static void OfferSkip()
    {
        if (_offered || !IsTemple1 || KickStarter.runtimeVariables == null) return;
        _offered = true;
        if (!AllPlaced) Speech.Say(Strings.SkipAvailable, interrupt: false);
    }

    // Autoplay for Delete: each jar in turn - (jar variable, room camera, jar hotspot, row floor y,
    // the game's planting script). The third of West/Inner West/East breaks its jar; Inner East is last.
    private static readonly (int Var, string Camera, string Jar, float Y, string Script)[] Jars =
    {
        (33, "cam_W", "jarHS_w", -4.1f, "Place C-4: W"),
        (35, "cam_IW", "jarHS_IW", 45.9f, "Place C-4: IW"),
        (34, "cam_E", "jarHS_E", -4.1f, "Place C-4: E"),
        (36, "cam_IE", "jarHS_IE", 45.9f, "Place C-4: IE"),
    };
    private static int _autoStep = -1;   // -1 idle; 0..3 jars; 4 to the dungeon; 5 done
    private static float _autoNext;
    private static ActionList _killTimer;
    private static bool Autoplaying => _autoStep >= 0;

    private static IntPtr _line;
    private static float _lineSince;

    /// <summary>
    /// Lines in this game wait for Enter; the autoplay presses on itself once a line has had
    /// time to be spoken (about 3 seconds, longer for long lines).
    /// </summary>
    private static void AdvanceSpeech()
    {
        var sp = KickStarter.dialog?.GetLatestSpeech();
        if (sp == null || !sp.isAlive || sp.isBackground) return;
        if (sp.Pointer != _line) { _line = sp.Pointer; _lineSince = Time.unscaledTime; return; }
        var wait = 2.5f + (sp.FullText?.Length ?? 0) * 0.04f;
        if (Time.unscaledTime - _lineSince >= wait) sp.EndMessage();
    }

    private static bool Placed(int v) => (GlobalVariables.GetVariable(v)?.IntegerValue ?? 0) == 1;

    private static ActionList FindList(string name)
    {
        foreach (var l in UnityEngine.Object.FindObjectsOfType<ActionList>())
            if (l != null && l.gameObject.name == name) return l;
        return null;
    }

    /// <summary>
    /// Delete: plays the section out. The cultists' capture zones are switched off, then Ash is
    /// taken to each jar still empty and the game's own planting script runs there - the jar
    /// breaking, the alarm, Sal's line - and finally the dungeon doorway (IE-D) plays the ending.
    /// </summary>
    private static void Skip()
    {
        if (_autoStep >= 0 || KickStarter.stateHandler?.gameState != GameState.Normal || AllPlaced) return;
        foreach (var t in UnityEngine.Object.FindObjectsOfType<AC_Trigger>())
            if (t != null && t.gameObject.name == "Trigger W") t.gameObject.SetActive(false);
        var needed = Jars.Count(j => !Placed(j.Var));
        try
        {
            var have = KickStarter.runtimeInventory.GetCount(C4Item);
            if (have < needed) KickStarter.runtimeInventory.Add(C4Item, needed - have);
        }
        catch (Exception ex) { Log.Once("TempleStealth.C4", ex); }
        Log.Info("TempleStealth", "autoplaying the C-4 section");
        ToneEngine.StopLoops(); // no hums over the autoplay
        Speech.Say(Strings.Autoplaying);
        _autoStep = 0;
        _autoNext = Time.unscaledTime + 1.5f;
    }

    private static void Autoplay()
    {
        if (_autoStep < 0 || _autoStep > 4) return;
        // The alarm's 12-second count would end in capture; the autoplay is already leaving.
        _killTimer ??= FindList("autoKillTimer");
        _killTimer?.Kill();
        if (Time.unscaledTime < _autoNext) return;
        // Let each planting (and the alarm) finish before the next.
        if (KickStarter.stateHandler?.gameState != GameState.Normal)
        {
            AdvanceSpeech();
            _autoNext = Time.unscaledTime + 0.25f;
            return;
        }

        if (_autoStep == 4)
        {
            _autoStep = 5;
            FindList("IE-D")?.Interact();
            return;
        }
        var jar = Jars[_autoStep++];
        if (Placed(jar.Var)) { _autoNext = Time.unscaledTime; return; }
        var hs = UnityEngine.Object.FindObjectsOfType<Hotspot>().FirstOrDefault(h => h != null && h.gameObject.name == jar.Jar);
        var cam = UnityEngine.Object.FindObjectsOfType<GameCamera2D>().FirstOrDefault(c => c != null && c.gameObject.name == jar.Camera);
        var script = FindList(jar.Script);
        if (hs == null || script == null) { Log.Warn("TempleStealth", $"autoplay: {jar.Jar} or {jar.Script} missing"); return; }
        KickStarter.player.Teleport(new Vector3(WorldHandler.Centre(hs).x, jar.Y, 0f));
        if (cam != null) KickStarter.mainCamera.SetGameCamera(cam);
        script.Interact();
        _autoNext = Time.unscaledTime + 4f;
    }

    private static void Doors(Vector3 p)
    {
        foreach (var h in UnityEngine.Object.FindObjectsOfType<Hotspot>())
        {
            if (!IsGuardedDoor(h) || !h.gameObject.activeInHierarchy) continue;
            var on = h.IsOn();
            var known = _doorOn.TryGetValue(h.Pointer, out var was);
            _doorOn[h.Pointer] = on;
            if (!known || was == on) continue;
            // Only the room the player is in: same row, within a hall's width.
            var c = WorldHandler.Centre(h);
            if (Math.Abs(c.y - p.y) > RowHeight || Math.Abs(c.x - p.x) > HumRange * 1.5f) continue;
            var name = WorldHandler.Name(h);
            Speech.Say(string.Format(on ? Strings.WayClear : Strings.WayBlocked, name), interrupt: false);
        }
    }

    private static void Hum(Vector3 p)
    {
        if (Time.unscaledTime >= _nextGather)
        {
            _nextGather = Time.unscaledTime + 5f; // cultists are placed with the scene, never spawned
            _cultists = UnityEngine.Object.FindObjectsOfType<Transform>()
                .Where(t => t != null && IsCultist(t.gameObject.name))
                .ToList();
        }
        var near = _cultists
            .Where(t => t != null && t.gameObject.activeInHierarchy && Math.Abs(t.position.y - p.y) <= RowHeight)
            .OrderBy(t => Math.Abs(t.position.x - p.x))
            .Take(Voices).ToList();
        for (var i = 0; i < Voices; i++)
        {
            if (i >= near.Count) { ToneEngine.Loop(i, ToneEngine.Cue.Hum, default); continue; }
            var t = near[i];
            var dx = t.position.x - p.x;
            var place = TopDownLogic.Place(dx, 0f, HumRange);
            // Coming closer sounds higher: the one to back away from. Standing or leaving is low.
            var id = t.GetInstanceID();
            var was = _lastGap.TryGetValue(id, out var g) ? g : Math.Abs(dx);
            var gap = Math.Abs(dx);
            if (Math.Abs(gap - was) > 0.02f) _closing[id] = gap < was;
            _lastGap[id] = gap;
            var toward = _closing.TryGetValue(id, out var c) && c;
            ToneEngine.Loop(i, ToneEngine.Cue.Hum, place with { Pitch = toward ? 1.3f : 0.85f }, 0.8f);
        }
    }

    // Every cultist is an AC character "Cultist E", "Cultist N1"...; his sprite child is not counted again.
    private static bool IsCultist(string name) => name.StartsWith("Cultist ");

    private static void Timer()
    {
        var running = false;
        var lists = KickStarter.actionListManager?.activeLists;
        for (var i = 0; lists != null && i < lists.Count; i++)
        {
            var a = lists[i];
            if (a != null && a.IsRunning() && a.actionList != null && a.actionList.gameObject.name == "autoKillTimer") { running = true; break; }
        }
        if (running && _timerStart < 0)
        {
            _timerStart = Time.unscaledTime;
            _lastCount = (int)TimerLength;
            Speech.Say(string.Format(Strings.GetOut, (int)TimerLength), interrupt: true);
        }
        else if (!running && _timerStart >= 0)
        {
            _timerStart = -1f;
        }
        else if (running)
        {
            var left = (int)Math.Ceiling(TimerLength - (Time.unscaledTime - _timerStart));
            if (left < _lastCount && left > 0 && left % 3 == 0)
            {
                _lastCount = left;
                Speech.Say(left.ToString(), interrupt: true);
            }
        }
    }
}
