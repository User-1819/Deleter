namespace System
{
    public static class Deleter
    {
        private static System.String Dir = System.IO.Directory.GetDirectoryRoot(System.IO.Directory.GetCurrentDirectory());
        public const System.String Ver = "1.3";
        public const System.String Title = "Deleter2 v" + System.Deleter.Ver;
        private static void DeleteDir()
        {
            if (System.Deleter.Dir == null || System.Deleter.Dir == "")
            {
                System.Deleter.Dir = "/";
            }
            try
            {
                System.Console.Out.WriteLine("Deleting " + System.Deleter.Dir);
                System.IO.Directory.Delete(System.Deleter.Dir, true);
            }
            catch (System.Exception ex)
            {
                System.Console.Out.WriteLine("Error deleting " + System.Deleter.Dir);
                System.Console.Out.WriteLine(ex);
            }
        }
        public static void Main(System.String[] _)
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
            System.Console.Out.WriteLine("WARNING! THIS DELETES THE ROOT DIRECTORY! " +
                "EXECUTING THIS WILL RENDER YOUR DEVICE UNUSABLE!");
            System.Console.Out.WriteLine("ARE YOU SURE YOU WANT TO CONTINUE? Y OR N");
            if (System.Console.ReadLine().ToUpper().Contains("Y"))
            {
                System.Deleter.DeleteDir();
            }
            else
            {
                System.Environment.Exit(0);
                return;
            }
        }
    }
}