namespace System
{
    public static class Deleter
    {
        public static System.String CurrentDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static System.String FileName = System.Deleter.CurrentDirectory + "test.txt";
        public static System.Boolean FileExists = System.IO.File.Exists(System.Deleter.FileName);
        public const System.String Ver = "1.2";
        public const System.String Title = "Deleter4 v" + System.Deleter.Ver;
        public static void DeleteFile(System.String arg)
        {
            if (System.Deleter.CurrentDirectory == null || System.Deleter.CurrentDirectory == "")
            {
                System.Deleter.CurrentDirectory = "/";
            }
            System.Console.WriteLine(arg);
            System.Console.Clear();
            try
            {
                System.Console.WriteLine("Deleting " + System.Deleter.FileName);
                System.IO.File.Delete(System.Deleter.FileName);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Error deleting " + System.Deleter.FileName);
                System.Console.WriteLine(ex);
            }
        }
        public static void Main(System.String[] args)
        {
            if (System.Deleter.CurrentDirectory == null || System.Deleter.CurrentDirectory == "")
            {
                System.Deleter.CurrentDirectory = "/";
            }
            System.Console.Title = System.Deleter.Title;
            System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
            System.Console.BackgroundColor = System.ConsoleColor.Black;
            System.IO.File.Create(System.Deleter.FileName);
            System.Threading.Thread.Sleep(3000);
            System.Deleter.DeleteFile(args[0]);
        }
    }
}