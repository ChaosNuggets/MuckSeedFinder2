using System;
using TMPro;
using UnityEngine;

public class PrintStuff : MonoBehaviour
{
    public static PrintStuff instance;

    [SerializeField]
    private TextMeshProUGUI seedsTestedText;
    [SerializeField]
    private TextMeshProUGUI estimatedTimeText;
    [SerializeField]
    private TextMeshProUGUI speedText;

    const int MINUTES_PER_HOUR = 60;
    const int SECONDS_PER_MINUTE = 60;

    public void UpdateText(int numTestedSeeds)
    {
        seedsTestedText.text = $"Seeds Tested:\n{numTestedSeeds} / {MainClass.NUM_SEEDS}";
        Console.WriteLine(seedsTestedText.text);

        float secondsLeft = Time.realtimeSinceStartup / numTestedSeeds * (MainClass.NUM_SEEDS - numTestedSeeds);
        int minutesLeft = Mathf.RoundToInt(secondsLeft / SECONDS_PER_MINUTE);
        int hoursLeft = minutesLeft / MINUTES_PER_HOUR;
        minutesLeft %= MINUTES_PER_HOUR;

        estimatedTimeText.text = $"Estimated Time Remaining:\n{hoursLeft} hr {minutesLeft} min";
        Console.WriteLine(estimatedTimeText.text);

        speedText.text = $"Speed:\n{Mathf.RoundToInt(numTestedSeeds / Time.realtimeSinceStartup)} seeds / sec";
        Console.WriteLine(speedText.text);
    }

    public void WriteSummaryMessage()
    {
        Console.WriteLine("\nSeed Finding Completed Successfully.\n");

        Console.WriteLine("---------------SUMMARY---------------");
        Console.WriteLine($"Number of Seeds Tested: {MainClass.NUM_SEEDS}");

        int elapsedSeconds = Mathf.RoundToInt(Time.unscaledTime);
        int elapsedHours = elapsedSeconds / (SECONDS_PER_MINUTE * MINUTES_PER_HOUR);
        elapsedSeconds %= SECONDS_PER_MINUTE * MINUTES_PER_HOUR;
        int elapsedMinutes = elapsedSeconds / SECONDS_PER_MINUTE;
        elapsedSeconds %= SECONDS_PER_MINUTE;
        Console.WriteLine($"Total time: {elapsedHours} hr {elapsedMinutes} min {elapsedSeconds} sec");

        Console.WriteLine($"Average speed: {Mathf.RoundToInt(MainClass.NUM_SEEDS / Time.unscaledTime)} seeds / sec");
        Console.WriteLine("-------------------------------------");

        Console.WriteLine("\nYou may close the seed finder now");
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
}
