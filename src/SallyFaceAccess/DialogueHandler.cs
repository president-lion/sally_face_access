using Il2CppAC;

namespace SallyFaceAccess;

/// <summary>
/// Speaks Adventure Creator speech lines as they appear. Polls the live speech list
/// (KickStarter.dialog.speechList) and speaks each line once, using its full text rather
/// than the part the typewriter effect has revealed so far.
///
/// A new foreground line interrupts: it means the previous one finished or was skipped.
/// Background chatter queues so it does not cut off what the player is listening to.
/// </summary>
internal static class DialogueHandler
{
    private static readonly HashSet<IntPtr> _spoken = new();
    private static readonly HashSet<IntPtr> _alive = new();

    public static void Tick()
    {
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

            var line = DialogueFormat.Line(sp.GetSpeaker(lang), sp.FullText, Settings.SpeakerNames);
            Log.Debug("Dialogue", $"line {sp.LineID} bg={sp.isBackground} -> '{line}'");
            // What the scene shows just before this line, if it has been described.
            var shown = Descriptions.Line(sp.LineID);
            if (shown != null) Speech.Say(shown, interrupt: !sp.isBackground);
            Speech.Say(line, interrupt: !sp.isBackground && shown == null);
        }

        // Forget lines that have ended, so the set does not grow and a reused pointer
        // is treated as a new line.
        _spoken.IntersectWith(_alive);
    }
}
