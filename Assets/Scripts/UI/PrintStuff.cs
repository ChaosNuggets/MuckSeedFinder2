using TMPro;
using UnityEngine;

public class PrintStuff : Singleton<PrintStuff>
{
    // Running text
    [SerializeField]
    private TextMeshProUGUI seedsTestedText;
    [SerializeField]
    private TextMeshProUGUI estimatedTimeText;
    [SerializeField]
    private TextMeshProUGUI speedText;

    // Summary text
    [SerializeField]
    private TextMeshProUGUI totalSeedsText;
    [SerializeField]
    private TextMeshProUGUI totalTimeText;
    [SerializeField]
    private TextMeshProUGUI averageSpeedText;

    public void UpdateText(int numTestedSeeds)
    {
        const int MINUTES_PER_HOUR = 60;

        seedsTestedText.text = $"Seeds Tested:\n{numTestedSeeds} / {SeedFinderRunner.NUM_SEEDS}";

        int minutesLeft = Mathf.RoundToInt((float)Timer.elapsed.TotalMinutes / numTestedSeeds * (SeedFinderRunner.NUM_SEEDS - numTestedSeeds));
        int hoursLeft = minutesLeft / MINUTES_PER_HOUR;
        minutesLeft %= MINUTES_PER_HOUR;

        estimatedTimeText.text = $"Estimated Time Remaining:\n{hoursLeft} hr {minutesLeft} min";

        speedText.text = $"Speed:\n{Mathf.RoundToInt(numTestedSeeds / (float)Timer.elapsed.TotalSeconds)} seeds / sec";
    }

    public void WriteSummaryMessage()
    {
        totalSeedsText.text = $"Number of seeds tested: {SeedFinderRunner.NUM_SEEDS}";

        totalTimeText.text = $"Total time: {Timer.elapsed.Hours} hr {Timer.elapsed.Minutes} min {Timer.elapsed.Seconds} sec";

        averageSpeedText.text = $"Average speed: {Mathf.RoundToInt(SeedFinderRunner.NUM_SEEDS / (float)Timer.elapsed.TotalSeconds)} seeds / sec";
    }
}
