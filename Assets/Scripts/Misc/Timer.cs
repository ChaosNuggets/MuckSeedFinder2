using System;
using System.Diagnostics;

public class Timer : Singleton<Timer>
{
    public static TimeSpan elapsed
    {
        get
        {
            return stopwatch.Elapsed;
        }
    }
    private static readonly Stopwatch stopwatch = new();

    void Start()
    {
        stopwatch.Start();
    }

    void OnDisable()
    {
        stopwatch.Stop();
    }
}
