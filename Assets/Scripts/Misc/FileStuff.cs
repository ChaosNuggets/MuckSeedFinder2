using System;
using System.IO;
using UnityEngine;

public static class FileStuff
{
    public static void LogSeed(int seed, float distance)
    {
        string path = Environment.GetFolderPath(
            Environment.SpecialFolder.DesktopDirectory) + @"\muck_seeds_2.csv"; // Get the path to the desktop

        if (!File.Exists(path))
        {
            // Create a file to write to
            WriteHeader(path);
        }

        WriteData(path, seed, distance);

        Debug.Log($"Logged seed {seed}, distance {distance}");
    }

    private static void WriteHeader(string path)
    {
        using StreamWriter sw = File.CreateText(path);
        sw.WriteLine("Seed,Distance");
    }

    private static void WriteData(string path, int seed, float distance)
    {
        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine($"{seed},{Mathf.RoundToInt(distance)}");
        }
    }
}
