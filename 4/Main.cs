namespace System
{
    public static class Deleter
    {
        private static System.String CurrentDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static readonly System.String FileName = System.Deleter.CurrentDirectory + "test.txt";
        public const System.String Ver = "1.3";
        public const System.String Title = "Deleter4 v" + System.Deleter.Ver;
        private static void DeleteFile()
        {
            if (System.Deleter.CurrentDirectory == null || System.Deleter.CurrentDirectory == "")
            {
                System.Deleter.CurrentDirectory = "/";
            }
            try
            {
                System.Console.Out.WriteLine("Deleting " + System.Deleter.FileName);
                System.IO.File.Delete(System.Deleter.FileName);
            }
            catch (System.Exception ex)
            {
                System.Console.Out.WriteLine("Error deleting " + System.Deleter.FileName);
                System.Console.Out.WriteLine(ex);
            }
        }
        public static void Main(System.String[] _)
        {
            if (System.Deleter.CurrentDirectory == null || System.Deleter.CurrentDirectory == "")
            {
                System.Deleter.CurrentDirectory = "/";
            }
            System.Console.Title = System.Deleter.Title;
            System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
            System.Console.BackgroundColor = System.ConsoleColor.Black;
            System.IO.File.Create(System.Deleter.FileName).Dispose();
            System.Threading.Thread.Sleep(3000);
            System.Deleter.DeleteFile();
        }
    }
}