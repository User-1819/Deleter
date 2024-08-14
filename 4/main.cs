using System;
using System.IO;
using System.Reflection;
using System.Threading;
    class Deleter {
      public static string CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      public static string FileName = CurrentDirectory + "test.txt";
      public static bool FileExists = File.Exists(FileName);
        public const string Ver = "1.0";
        public const string Title = "Deleter v" + Ver;
      public static void DeleteFile() {
          try { 
        Console.WriteLine("Deleting " + FileName);
          File.Delete(FileName);
            }
          catch (Exception ex) {
            Console.WriteLine("Error deleting " + FileName);
            Console.WriteLine(ex);  
          }
          }
        static void Main(string[] args) {
        Console.Title = Title;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.BackgroundColor = ConsoleColor.Black;
          File.Create(FileName);
          Thread.Sleep(3000);
        DeleteFile();
          }
    }