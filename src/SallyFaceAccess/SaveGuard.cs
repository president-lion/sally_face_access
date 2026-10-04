using HarmonyLib;
using Il2CppAC;

namespace SallyFaceAccess;

/// <summary>
/// While the tour runs, nothing may be saved: scenes run their autosave scripts when they
/// load, and the autosave is the player's slot 0. Every save entry point is blocked (the
/// public ones and the private workers behind them, in case IL2CPP calls those directly),
/// and each block is logged so it can be checked.
/// </summary>
internal static class SaveGuard
{
    public static bool Blocking => Settings.Tour || Settings.SelfTest;

    private static bool Block(string what)
    {
        if (!Blocking) return true;
        Log.Info("SaveGuard", $"blocked {what} (development tool running)");
        return false; // skip the original
    }

    [HarmonyPatch(typeof(SaveSystem), nameof(SaveSystem.SaveAutoSave))]
    private static class AutoSave { private static bool Prefix() => Block("SaveAutoSave"); }

    [HarmonyPatch(typeof(SaveSystem), nameof(SaveSystem.SaveNewGame))]
    private static class NewGame { private static bool Prefix() => Block("SaveNewGame"); }

    [HarmonyPatch(typeof(SaveSystem), nameof(SaveSystem.SaveGame), new[] { typeof(int), typeof(bool), typeof(string) })]
    private static class Game1 { private static bool Prefix() => Block("SaveGame(id)"); }

    [HarmonyPatch(typeof(SaveSystem), nameof(SaveSystem.SaveGame), new[] { typeof(int), typeof(int), typeof(bool), typeof(bool), typeof(string) })]
    private static class Game2 { private static bool Prefix() => Block("SaveGame(slot)"); }

    [HarmonyPatch(typeof(SaveSystem), "SaveSaveGame")]
    private static class Worker { private static bool Prefix() => Block("SaveSaveGame"); }

    [HarmonyPatch(typeof(SaveSystem), "SaveNewSaveGame")]
    private static class NewWorker { private static bool Prefix() => Block("SaveNewSaveGame"); }

    [HarmonyPatch(typeof(ActionSaveHandle), nameof(ActionSaveHandle.Run))]
    private static class SaveAction { private static bool Prefix() => Block("ActionSaveHandle"); }
}
