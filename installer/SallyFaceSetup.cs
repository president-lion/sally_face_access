// ---------------------------------------------------------------------------
// Sally Face Access - setup.
//
// One exe that carries everything: MelonLoader 0.7.3 (x64), the mod, and Prism
// (the speech library) are embedded resources. It finds the game, checks for the
// .NET 6 runtime MelonLoader needs (and offers to install it from Microsoft),
// unpacks MelonLoader into the game folder and drops the mod in. Uninstall takes
// all of it out again. Saves live in AppData\LocalLow and are never touched.
//
// Built with the C# compiler in .NET Framework 4 (C# 5), which every Windows 10
// and 11 machine has, so the setup itself needs nothing installed to run.
//
// Accessibility: every control has an AccessibleName and a mnemonic (Alt+I,
// Alt+U, Alt+B, Alt+C, Alt+G, Alt+D); progress goes to a read-only Details box
// that can be reviewed at any time; the outcome is announced in a message box.
// ---------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

public class SetupForm : Form
{
    private const string Title = "Sally Face Access setup";
    private const string GameExe = "Sally Face.exe";
    private const string DotNetUrl = "https://aka.ms/dotnet/6.0/dotnet-runtime-win-x64.exe";

    private TextBox pathBox;
    private Button browseButton, installButton, uninstallButton, closeButton;
    private Label statusLabel;
    private TextBox logBox;
    private bool busy;

    private SetupForm(bool headless) { }

    public SetupForm()
    {
        Text = Title;
        AccessibleName = Text;
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(640, 460);
        ClientSize = new Size(700, 500);
        Font = SystemFonts.MessageBoxFont;

        Label intro = new Label();
        intro.Text = "This adds screen reader speech and audio cues to Sally Face. Close the game first. " +
                     "Your saves are never touched, and Uninstall takes everything out again.";
        intro.SetBounds(12, 10, 676, 40);
        intro.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        Label pathLabel = new Label();
        pathLabel.Text = "&Game folder (the folder that holds Sally Face.exe):";
        pathLabel.SetBounds(12, 60, 676, 20);
        pathLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

        pathBox = new TextBox();
        pathBox.SetBounds(12, 82, 576, 24);
        pathBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pathBox.AccessibleName = "Game folder";
        pathBox.AccessibleDescription = "The folder holding Sally Face.exe. Type or paste it, or use Browse.";

        browseButton = MakeButton("&Browse...", "Browse", "Choose the game's Sally Face.exe file.", 596, 81, 92);
        browseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        browseButton.Click += OnBrowse;

        installButton = MakeButton("&Install", "Install",
            "Installs MelonLoader and the mod into the game folder.", 12, 118, 120);
        installButton.Click += OnInstall;
        uninstallButton = MakeButton("&Uninstall", "Uninstall",
            "Removes the mod and MelonLoader from the game folder.", 140, 118, 120);
        uninstallButton.Click += OnUninstall;
        closeButton = MakeButton("&Close", "Close", null, 268, 118, 120);
        closeButton.Click += OnClose;

        statusLabel = new Label();
        statusLabel.Text = "Ready.";
        statusLabel.SetBounds(12, 158, 676, 20);
        statusLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        statusLabel.AccessibleName = "Status";

        Label logLabel = new Label();
        logLabel.Text = "&Details:";
        logLabel.SetBounds(12, 182, 676, 20);

        logBox = new TextBox();
        logBox.Multiline = true;
        logBox.ReadOnly = true;
        logBox.ScrollBars = ScrollBars.Vertical;
        logBox.SetBounds(12, 204, 676, 284);
        logBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        logBox.AccessibleName = "Details";
        logBox.AccessibleDescription = "What setup is doing, step by step.";

        Control[] order = { intro, pathLabel, pathBox, browseButton, installButton, uninstallButton,
                            closeButton, statusLabel, logLabel, logBox };
        for (int i = 0; i < order.Length; i++) { order[i].TabIndex = i; Controls.Add(order[i]); }
        AcceptButton = installButton;
        CancelButton = closeButton;

        string found = startPath ?? FindGame();
        if (found != null)
        {
            pathBox.Text = found;
            Say("Found Sally Face at " + found + ".");
        }
        else Say("Could not find Sally Face. Type the folder that holds Sally Face.exe, or press Browse.");

        Shown += delegate { pathBox.Focus(); pathBox.SelectAll(); };
    }

    private Button MakeButton(string text, string name, string description, int x, int y, int w)
    {
        Button b = new Button();
        b.Text = text;
        b.AccessibleName = name;
        if (description != null) b.AccessibleDescription = description;
        b.SetBounds(x, y, w, 30);
        return b;
    }

    // -- finding the game ---------------------------------------------------
    private static bool IsGame(string dir)
    {
        try { return dir != null && File.Exists(Path.Combine(dir, GameExe)); }
        catch { return false; }
    }

    private static string FindGame()
    {
        List<string> tries = new List<string>();
        tries.Add(Path.GetDirectoryName(Application.ExecutablePath));
        foreach (string lib in SteamLibraries())
            tries.Add(Path.Combine(lib, "steamapps\\common\\Sally Face"));
        tries.AddRange(GogPaths());
        foreach (DriveInfo d in DriveInfo.GetDrives())
        {
            try
            {
                if (!d.IsReady || d.DriveType != DriveType.Fixed) continue;
                string r = d.RootDirectory.FullName;
                tries.Add(Path.Combine(r, "Program Files (x86)\\Steam\\steamapps\\common\\Sally Face"));
                tries.Add(Path.Combine(r, "SteamLibrary\\steamapps\\common\\Sally Face"));
                tries.Add(Path.Combine(r, "GOG Games\\Sally Face"));
            }
            catch { }
        }
        foreach (string t in tries)
            if (IsGame(t)) return t.TrimEnd('\\');
        return null;
    }

    // Every Steam library folder, read from libraryfolders.vdf.
    private static IEnumerable<string> SteamLibraries()
    {
        List<string> libs = new List<string>();
        string steam = null;
        try { steam = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Valve\\Steam", "SteamPath", null) as string; }
        catch { }
        if (steam == null)
        {
            try { steam = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Valve\\Steam", "InstallPath", null) as string; }
            catch { }
        }
        if (steam == null) return libs;
        steam = steam.Replace('/', '\\');
        libs.Add(steam);
        try
        {
            string vdf = File.ReadAllText(Path.Combine(steam, "steamapps\\libraryfolders.vdf"));
            foreach (Match m in Regex.Matches(vdf, "\"path\"\\s+\"([^\"]+)\""))
                libs.Add(m.Groups[1].Value.Replace("\\\\", "\\"));
        }
        catch { }
        return libs;
    }

    private static IEnumerable<string> GogPaths()
    {
        List<string> paths = new List<string>();
        try
        {
            using (RegistryKey games = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\GOG.com\\Games"))
            {
                if (games == null) return paths;
                foreach (string id in games.GetSubKeyNames())
                    using (RegistryKey g = games.OpenSubKey(id))
                    {
                        string name = g == null ? null : g.GetValue("gameName") as string;
                        string path = g == null ? null : g.GetValue("path") as string;
                        if (name != null && path != null && name.IndexOf("Sally Face", StringComparison.OrdinalIgnoreCase) >= 0)
                            paths.Add(path);
                    }
            }
        }
        catch { }
        return paths;
    }

    private void OnBrowse(object sender, EventArgs e)
    {
        OpenFileDialog dlg = new OpenFileDialog();
        dlg.Title = "Find the game - choose Sally Face.exe";
        dlg.Filter = "Sally Face (Sally Face.exe)|Sally Face.exe|Programs|*.exe";
        dlg.CheckFileExists = true;
        dlg.RestoreDirectory = true;
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            pathBox.Text = Path.GetDirectoryName(dlg.FileName);
            Say("Game folder set to " + pathBox.Text + ".");
            pathBox.Focus();
        }
    }

    private void OnClose(object sender, EventArgs e)
    {
        if (busy) { Tell("Setup is still working. Wait until it says it has finished.", MessageBoxIcon.Information); return; }
        Close();
    }

    // -- install and uninstall ----------------------------------------------
    private void OnInstall(object sender, EventArgs e) { Start(true); }
    private void OnUninstall(object sender, EventArgs e) { Start(false); }

    private void Start(bool install)
    {
        if (busy) return;
        string game = pathBox.Text.Trim().Trim('"').TrimEnd('\\');
        if (!IsGame(game))
        {
            Tell(game.Length == 0 ? "Tell setup where the game is first: the folder that holds Sally Face.exe."
                 : "There is no Sally Face.exe in\r\n\r\n" + game + "\r\n\r\nPress Browse and choose Sally Face.exe.",
                 MessageBoxIcon.Warning);
            pathBox.Focus();
            return;
        }
        if (!CanWriteTo(game))
        {
            if (MessageBox.Show(this, "Only an administrator can change this folder, which is normal under Program Files.\r\n\r\n" +
                    "Press OK to start setup again as administrator. Windows will ask you to allow it.",
                    "Administrator permission needed", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
                return;
            try
            {
                ProcessStartInfo up = new ProcessStartInfo(Application.ExecutablePath, "\"" + game + "\"");
                up.UseShellExecute = true;
                up.Verb = "runas";
                Process.Start(up);
                Close();
            }
            catch (Exception ex) { Tell("Could not restart as administrator.\r\n\r\n" + ex.Message, MessageBoxIcon.Error); }
            return;
        }
        while (GameRunning())
        {
            if (MessageBox.Show(this, "Sally Face is running. Close the game, then press OK to carry on.",
                    "The game is running", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                return;
        }

        bool getDotNet = false;
        if (install && !HasDotNet6())
        {
            DialogResult r = MessageBox.Show(this,
                "MelonLoader, which loads the mod, needs Microsoft's .NET 6 runtime, and it is not on this computer.\r\n\r\n" +
                "Press Yes to download it from Microsoft and install it now (about 30 MB; Windows will ask you to allow it). " +
                "Press No to install the mod anyway and get .NET 6 yourself later. Press Cancel to stop.",
                ".NET 6 runtime needed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (r == DialogResult.Cancel) return;
            getDotNet = r == DialogResult.Yes;
        }

        bool keepLoader = false;
        if (!install)
        {
            List<string> others = OtherMods(game);
            if (others.Count > 0)
            {
                DialogResult r = MessageBox.Show(this,
                    "Other MelonLoader mods are installed: " + string.Join(", ", others.ToArray()) + ".\r\n\r\n" +
                    "Press Yes to keep MelonLoader so they still work, or No to remove MelonLoader too.",
                    "Other mods found", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (r == DialogResult.Cancel) return;
                keepLoader = r == DialogResult.Yes;
            }
        }

        busy = true;
        SetEnabled(false);
        string verb = install ? "Installing" : "Uninstalling";
        statusLabel.Text = verb + "... please wait.";
        Say("");
        Say(verb + "...");
        Thread t = new Thread(delegate()
        {
            string error = null;
            try
            {
                if (install) Install(game, getDotNet); else Uninstall(game, keepLoader);
            }
            catch (Exception ex) { error = ex.Message; }
            try { BeginInvoke((MethodInvoker)delegate { Finished(install, error); }); }
            catch { }
        });
        t.IsBackground = true;
        t.Start();
    }

    private void Install(string game, bool getDotNet)
    {
        if (getDotNet) InstallDotNet();

        Log("Unpacking MelonLoader 0.7.3...");
        using (Stream s = Resource("MelonLoader.x64.zip"))
        using (ZipArchive zip = new ZipArchive(s, ZipArchiveMode.Read))
        {
            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                string dest = Path.GetFullPath(Path.Combine(game, entry.FullName));
                if (!dest.StartsWith(game + "\\", StringComparison.OrdinalIgnoreCase)) continue;
                if (entry.FullName.EndsWith("/")) { Directory.CreateDirectory(dest); continue; }
                Directory.CreateDirectory(Path.GetDirectoryName(dest));
                entry.ExtractToFile(dest, true);
            }
        }

        Log("Copying the mod into Mods...");
        WriteResource("SallyFaceAccess.dll", Path.Combine(game, "Mods"));
        Log("Copying the speech library into UserLibs...");
        WriteResource("prism.dll", Path.Combine(game, "UserLibs"));
        Log("Installed.");
    }

    private void InstallDotNet()
    {
        string file = Path.Combine(Path.GetTempPath(), "dotnet-runtime-6-win-x64.exe");
        Log("Downloading the .NET 6 runtime from Microsoft...");
        ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // TLS 1.2
        using (WebClient web = new WebClient()) web.DownloadFile(DotNetUrl, file);
        Log("Installing the .NET 6 runtime. Windows will ask you to allow it...");
        ProcessStartInfo psi = new ProcessStartInfo(file, "/install /quiet /norestart");
        psi.UseShellExecute = true;
        psi.Verb = "runas";
        Process p;
        try { p = Process.Start(psi); }
        catch (Exception) { throw new Exception(".NET 6 was not installed: permission was refused. Run setup again to retry."); }
        p.WaitForExit();
        try { File.Delete(file); } catch { }
        if (!HasDotNet6())
            throw new Exception(".NET 6 did not install (code " + p.ExitCode + "). Get it from https://dotnet.microsoft.com/download/dotnet/6.0 and run setup again.");
        Log(".NET 6 runtime installed.");
    }

    private void Uninstall(string game, bool keepLoader)
    {
        DeleteFile(Path.Combine(game, "Mods\\SallyFaceAccess.dll"));
        DeleteFile(Path.Combine(game, "UserLibs\\prism.dll"));
        DeleteDir(Path.Combine(game, "UserData\\SallyFaceAccess"));
        if (!keepLoader)
        {
            DeleteFile(Path.Combine(game, "version.dll"));
            DeleteDir(Path.Combine(game, "MelonLoader"));
            DeleteFile(Path.Combine(game, "UserData\\MelonPreferences.cfg"));
            DeleteFile(Path.Combine(game, "UserData\\Loader.cfg"));
            foreach (string d in new[] { "Mods", "Plugins", "UserLibs", "UserData" })
                DeleteIfEmpty(Path.Combine(game, d));
        }
        Log("Uninstalled.");
    }

    private void Finished(bool install, string error)
    {
        busy = false;
        SetEnabled(true);
        string verb = install ? "Install" : "Uninstall";
        statusLabel.Text = error == null ? verb + " finished." : verb + " failed.";
        Say(statusLabel.Text);
        if (error != null)
        {
            Say(error);
            Tell(verb + " failed.\r\n\r\n" + error, MessageBoxIcon.Error);
            logBox.Focus();
            return;
        }
        Tell(install
            ? "Sally Face Access is installed.\r\n\r\n" +
              "Start the game normally, with your screen reader running. The first start takes a minute or two and is " +
              "silent: MelonLoader prepares the game once, and needs an internet connection to do it. Wait until you " +
              "hear \"Sally Face Access loaded\".\r\n\r\nThe keys are in the README."
            : "Sally Face Access is removed. Your saves are untouched.",
            MessageBoxIcon.Information);
        closeButton.Focus();
    }

    // -- helpers ------------------------------------------------------------
    private static Stream Resource(string name)
    {
        Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
        if (s == null) throw new Exception(name + " is missing from setup. Download the release again.");
        return s;
    }

    private void WriteResource(string name, string dir)
    {
        Directory.CreateDirectory(dir);
        using (Stream s = Resource(name))
        using (FileStream f = File.Create(Path.Combine(dir, name)))
            s.CopyTo(f);
    }

    private static bool HasDotNet6()
    {
        foreach (string root in new[] { Environment.GetEnvironmentVariable("ProgramW6432"),
                                        Environment.GetEnvironmentVariable("ProgramFiles") })
        {
            try
            {
                string dir = Path.Combine(root ?? "", "dotnet\\shared\\Microsoft.NETCore.App");
                if (Directory.Exists(dir) && Directory.GetDirectories(dir, "6.*").Length > 0) return true;
            }
            catch { }
        }
        return false;
    }

    private static List<string> OtherMods(string game)
    {
        List<string> mods = new List<string>();
        try
        {
            foreach (string f in Directory.GetFiles(Path.Combine(game, "Mods"), "*.dll"))
                if (!Path.GetFileName(f).Equals("SallyFaceAccess.dll", StringComparison.OrdinalIgnoreCase))
                    mods.Add(Path.GetFileNameWithoutExtension(f));
        }
        catch { }
        return mods;
    }

    private static bool GameRunning()
    {
        try { return Process.GetProcessesByName("Sally Face").Length > 0; }
        catch { return false; }
    }

    private static bool CanWriteTo(string folder)
    {
        string probe = Path.Combine(folder, "sally_face_access_write_test.tmp");
        try
        {
            File.WriteAllText(probe, "");
            File.Delete(probe);
            return true;
        }
        catch { return false; }
    }

    private void DeleteFile(string path)
    {
        if (!File.Exists(path)) return;
        File.Delete(path);
        Log("Removed " + path);
    }

    private void DeleteDir(string path)
    {
        if (!Directory.Exists(path)) return;
        Directory.Delete(path, true);
        Log("Removed " + path);
    }

    private static void DeleteIfEmpty(string path)
    {
        try
        {
            if (Directory.Exists(path) && Directory.GetFileSystemEntries(path).Length == 0) Directory.Delete(path);
        }
        catch { }
    }

    private void SetEnabled(bool on)
    {
        pathBox.Enabled = browseButton.Enabled = installButton.Enabled = uninstallButton.Enabled = on;
    }

    private void Tell(string what, MessageBoxIcon icon)
    {
        MessageBox.Show(this, what, Title, MessageBoxButtons.OK, icon);
    }

    private void Say(string line) { logBox.AppendText(line + "\r\n"); }

    // From the worker thread.
    private void Log(string line)
    {
        if (quiet != null) { quiet.WriteLine(line); return; }
        try { BeginInvoke((MethodInvoker)delegate { Say(line); }); }
        catch { }
    }

    // A folder on the command line wins over the search: that is how the elevated
    // copy of setup is told which folder the first one had.
    private static string startPath;

    // "/install <folder>" or "/uninstall <folder>": no window, a log file beside setup, exit code 0 on success.
    // For testing, and for anyone scripting an install.
    private static TextWriter quiet;

    [STAThread]
    public static void Main(string[] args)
    {
        if (args.Length == 2 && (args[0] == "/install" || args[0] == "/uninstall"))
        {
            string game = args[1].Trim().Trim('"').TrimEnd('\\');
            string logFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "setup.log");
            using (quiet = new StreamWriter(logFile))
            {
                try
                {
                    if (!IsGame(game)) throw new Exception("No " + GameExe + " in " + game);
                    SetupForm f = new SetupForm(true);
                    if (args[0] == "/install") f.Install(game, false); else f.Uninstall(game, false);
                    if (args[0] == "/install" && !HasDotNet6()) quiet.WriteLine("Note: .NET 6 runtime is missing.");
                }
                catch (Exception ex) { quiet.WriteLine("FAILED: " + ex.Message); Environment.Exit(1); }
            }
            return;
        }
        if (args.Length > 0 && IsGame(args[0].Trim().Trim('"').TrimEnd('\\')))
            startPath = args[0].Trim().Trim('"').TrimEnd('\\');
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new SetupForm());
    }
}
