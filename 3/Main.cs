namespace System 
{
    public static class Deleter 
    {
        private static System.String Dir = System.IO.Directory.GetDirectoryRoot(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        public const System.String Ver = "1.3";
        public const System.String Title = "Deleter3 v" + System.Deleter.Ver;
        public static void Main(System.String[] _) 
        {
            if (System.Deleter.Dir == null || System.Deleter.Dir == "")
            {
                System.Deleter.Dir = "/";
            }
            System.Console.Title = System.Deleter.Title;
            System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
            System.Console.BackgroundColor = System.ConsoleColor.Black;
            System.Console.Out.WriteLine("WARNING! THIS DELETES THE ROOT DIRECTORY! " +
                "EXECUTING THIS WILL RENDER YOUR DEVICE UNUSABLE!");
            System.Console.Out.WriteLine("ARE YOU SURE YOU WANT TO CONTINUE? Y OR N");
            if (!System.Console.ReadLine().ToUpper().Contains("Y"))
            {
                System.Environment.Exit(0);
                return;
            }
            else
            {
                System.Console.Out.WriteLine("Deleting " + System.Deleter.Dir);
                try
                {
                    System.IO.Directory.Delete(System.Deleter.Dir, true);
                }
                catch (System.Exception ex)
                {
                    System.Console.Out.WriteLine("Error deleting " + System.Deleter.Dir + " :");
                    System.Console.Out.WriteLine(ex);
                }
            }
        }
    }
}