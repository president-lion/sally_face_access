using UnityEngine;

namespace SallyFaceAccess;

/// <summary>
/// The mod's own keys. F-keys are used because the game binds letters, arrows and Escape
/// (and F12, to its demo reset). Room-scan keys live in WorldHandler.
///   F3   repeat the last line
///   Ctrl stop speech
///   F10  write screen.txt (what is on screen now)
///   F11  write the text dump
///   F9   toggle speaking of unhandled text (development aid)
/// </summary>
internal static class Hotkeys
{
    public static void Tick()
    {
        if (Input.GetKeyDown(KeyCode.F3)) Speech.Repeat();

        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) Speech.Stop();

        if (Input.GetKeyDown(KeyCode.F10))
        {
            UiDump.Write();
            Speech.Say(Strings.UiDumpWritten);
        }

        if (Input.GetKeyDown(KeyCode.F11) && TextDump.Ready)
        {
            TextDump.WriteAll();
            Speech.Say(Strings.DumpWritten);
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            Settings.SpeakUnhandled = !Settings.SpeakUnhandled;
            Speech.Say(Settings.SpeakUnhandled ? Strings.DebugSpeechOn : Strings.DebugSpeechOff);
        }
    }
}
