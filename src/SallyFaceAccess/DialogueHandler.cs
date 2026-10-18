using Il2CppAC;

namespace SallyFaceAccess;

/// <summary>
/// Speaks Adventure Creator speech lines as they appear. Polls the live speech list
/// (KickStarter.dialog.speechList) and speaks each line once, using its full text rather
/// than the part the typewriter effect has revealed so far.
///
/// A new foreground line interrupts: it means the previous one finished or was skipped.
/// Background chatter queues so it does not cut off what the player is listening to.
///
/// During a cutscene the arrow keys repeat the current line until it is advanced, and every
/// new foreground line plays a short E-flat major chord as it appears.
/// </summary>
internal static class DialogueHandler
{
    private static readonly HashSet<IntPtr> _spoken = new();
    private static readonly HashSet<IntPtr> _alive = new();

    // The foreground line on screen now, for the arrow-key repeat.
    private static IntPtr _current;
    private static string _currentText;

    private static bool _skipTuned;

    /// <summary>
    /// The game waits a moment before a line can be skipped and spends the first press finishing
    /// the text's scroll. The line is already spoken in full, so (InstantSkip) Enter skips at once.
    /// </summary>
    private static void TuneSkipping()
    {
        if (_skipTuned || !Settings.InstantSkip) return;
        var sm = KickStarter.speechManager;
        if (sm == null) return;
        _skipTuned = true;
        Log.Info("Dialogue", $"speech skip: threshold {sm.skipThresholdTime}s, minimum display {sm.minimumDisplayTime}s, " +
                             $"end scroll first {sm.endScrollBeforeSkip}; setting threshold 0, no scroll step");
        sm.skipThresholdTime = 0f;
        sm.endScrollBeforeSkip = false;
    }

    public static void Tick()
    {
        TuneSkipping();
        var list = KickStarter.dialog?.speechList;
        if (list == null) return;
        var lang = Options.GetLanguage();

        _alive.Clear();
        for (var i = 0; i < list.Count; i++)
        {
            var sp = list[i];
            if (sp == null || !sp.isAlive) continue;
            var key = sp.Pointer;
            _alive.Add(key);
            if (_spoken.Contains(key)) continue;
            _spoken.Add(key);

            var text = DialogueFormat.IsMirrored(sp.LineID)
                ? string.Format(Strings.Backwards, DialogueFormat.Unmirror(sp.FullText)) : sp.FullText;
            var line = DialogueFormat.Line(sp.GetSpeaker(lang), text, Settings.SpeakerNames);
            if (!sp.isBackground)
            {
                _current = key;
                _currentText = line;
                ToneEngine.Play(ToneEngine.Cue.Advance, new TopDownLogic.Placement(0f, 1f, 1f), 0.45f);
            }
            Log.Debug("Dialogue", $"line {sp.LineID} bg={sp.isBackground} -> '{line}'");
            // What the scene shows just before this line, if it has been described.
            var shown = Descriptions.Line(sp.LineID);
            if (shown != null) Speech.Say(shown, interrupt: !sp.isBackground);
            Speech.Say(line, interrupt: !sp.isBackground && shown == null);
            var after = Descriptions.AfterLine(sp.LineID);
            if (after != null) Speech.Say(after, interrupt: false);
            var note = Descriptions.AfterLineText(sp.LineID) is { } t
                ? TextClean.Clean(PlayerMenus.GetElementWithName(t.Menu, t.Element)?.GetLabel(0, lang)) : "";
            if (note.Length > 0)
            {
                Speech.Say(note, interrupt: false);
                if (!sp.isBackground) _currentText = $"{line} {note}"; // the arrow keys repeat it too
            }
        }

        // Forget lines that have ended, so the set does not grow and a reused pointer
        // is treated as a new line.
        _spoken.IntersectWith(_alive);

        if (_current != IntPtr.Zero && !_alive.Contains(_current)) { _current = IntPtr.Zero; _currentText = null; }

        RepeatAndAdvance();
    }

    private static void RepeatAndAdvance()
    {
        if (_current == IntPtr.Zero) return;

        // Arrow keys move the player in normal play and move through menus and conversation
        // choices, so they only repeat during cutscenes.
        if (KickStarter.stateHandler?.gameState != GameState.Cutscene) return;
        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.UpArrow) || UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.DownArrow)
            || UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.LeftArrow) || UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.RightArrow))
            Speech.Say(_currentText);
    }
}
