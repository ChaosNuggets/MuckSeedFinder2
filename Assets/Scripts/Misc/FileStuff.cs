using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileStuff
{
    public static void LogSeeds(IList<(int, float)> seeds)
    {
        foreach(var (seed, distance) in seeds)
        {
            LogSeed(seed, distance);
            Console.WriteLine($"Logged seed {seed}, distance {distance}");
        }
    }

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
    }

    private static void WriteHeader(string path)
    {
        using StreamWriter sw = File.CreateText(path);
        sw.WriteLine("Seed,Distance");
    }

    private static void WriteData(string path, int seed, float distance)
    {
        using StreamWriter sw = File.AppendText(path);
        sw.WriteLine($"{seed},{Mathf.RoundToInt(distance)}");
    }
}
