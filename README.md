# Sally Face Access

A mod that makes Sally Face playable with a screen reader. It speaks dialogue, menus, items and goals, describes rooms, cutscenes and pictures, and adds audio cues for the minigames and fights. It works with NVDA, JAWS and other screen readers, or Windows' own voices if none is running.

It is built for Sally Face 1.5.50 on Windows (1.5.51, the current Steam version, only fixes the Clumpy minigame and should work too), and loads through [MelonLoader](https://github.com/LavaGang/MelonLoader) 0.7.3.

## Installing

### With setup (recommended)

1. Close the game.
2. Run `Setup sally_face_access.exe` from the release page.
3. Check the game folder it found, then press Alt+I to install.

Setup finds the game in Steam or GOG, installs MelonLoader and the mod, and offers to install Microsoft's .NET 6 runtime if it is missing (MelonLoader needs it). Alt+U uninstalls everything again. Your saves are never touched.

### By hand (portable zip)

1. Install the [.NET 6 runtime (x64)](https://dotnet.microsoft.com/download/dotnet/6.0) if you do not have it.
2. Unzip `sally_face_access_portable.zip` into the game folder, the one holding `Sally Face.exe`, so that `version.dll` sits next to it.

To uninstall, delete `version.dll`, the `MelonLoader` folder, `Mods\SallyFaceAccess.dll` and `UserLibs\prism.dll`.

### The first start

The first time the game starts with the mod, MelonLoader spends a minute or two preparing the game, and the game stays silent while it does. It needs an internet connection for this, once. Wait until you hear "Sally Face Access loaded".

## Keys

The game's own keys work as usual. The mod adds these:

| Key | What it does |
| --- | --- |
| Page Down / Page Up | Next / previous thing in the room, with its direction and distance |
| Home | The current thing again, with its updated distance |
| End | In 3D areas: use the thing you last scanned to |
| F3 | Repeat the last thing said |
| F4 | Where am I: the room and how many things are in it |
| F5 | Your current goal |
| F6 | Describe the room |
| Ctrl | Stop speech |
| Any arrow key, during a cutscene | Repeat the current line of dialogue |
| Delete | Skip a puzzle or autoplay a fight, where the mod says it can |
| Backspace | Close the pyramids screen |

In the Super Gear Boy games:

| Key | What it does |
| --- | --- |
| F7 | How far the walls are up, down, left and right |
| F8 | Positional audio on or off |

## Settings

Settings are in `UserData\MelonPreferences.cfg` in the game folder, under `[SallyFaceAccess]`. Open it in a text editor while the game is closed.

| Setting | What it does |
| --- | --- |
| `Backend` | Which speech to use: `auto`, `nvda`, `jaws`, `sapi`, `onecore` and others |
| `Hints` | Item hints: `off`, `subtle` (says when something you carry is worth trying on a thing) or `full` (names the item) |
| `SpeakerNames` | Say who is talking before each line |
| `InstantSkip` | Enter moves to the next line at once, without the game's short wait |
| `RainVolume` | How loud the rain is, from 0 (silent) to 1 (unchanged) |
| `StepSize` | How far one "step" is when the room scan gives distances |

## Credits

Made by president-lion, with Claude.

Speech by [Prism](https://github.com/ethindp/prism). Mod loading by [MelonLoader](https://github.com/LavaGang/MelonLoader). Sally Face is by Portable Moose; no part of the game is included.
