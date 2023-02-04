using System;
using System.IO;

public static class FileStuff
{
    public static void Log(string text)
    {
        string path = Environment.GetFolderPath(
            Environment.SpecialFolder.DesktopDirectory) + @"\MuckSeedFinder.log.txt"; // Get the path to the desktop

        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(text);
        }
    }
    public static void LogSeed(int[] seeds)
    {
        string path = Environment.GetFolderPath(
            Environment.SpecialFolder.DesktopDirectory) + @"\god_seeds.txt"; // Get the path to the desktop

        WriteData(path, seeds);
    }

    private static void WriteData(string path, int[] seeds)
    {
        using (StreamWriter sw = File.AppendText(path))
        {
            foreach (int seed in seeds)
            {
                sw.WriteLine(seed);
            }
        }
    }
}
