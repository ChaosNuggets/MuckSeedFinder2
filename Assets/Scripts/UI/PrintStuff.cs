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

    const int MINUTES_PER_HOUR = 60;
    const int SECONDS_PER_MINUTE = 60;

    public void UpdateText(int numTestedSeeds)
    {
        seedsTestedText.text = $"Seeds Tested:\n{numTestedSeeds} / {SeedFinderRunner.NUM_SEEDS}";

        float secondsLeft = Time.realtimeSinceStartup / numTestedSeeds * (SeedFinderRunner.NUM_SEEDS - numTestedSeeds);
        int minutesLeft = Mathf.RoundToInt(secondsLeft / SECONDS_PER_MINUTE);
        int hoursLeft = minutesLeft / MINUTES_PER_HOUR;
        minutesLeft %= MINUTES_PER_HOUR;

        estimatedTimeText.text = $"Estimated Time Remaining:\n{hoursLeft} hr {minutesLeft} min";

        speedText.text = $"Speed:\n{Mathf.RoundToInt(numTestedSeeds / Time.realtimeSinceStartup)} seeds / sec";
    }

    public void WriteSummaryMessage()
    {
        totalSeedsText.text = $"Number of seeds tested: {SeedFinderRunner.NUM_SEEDS}";

        int elapsedSeconds = Mathf.RoundToInt(Time.unscaledTime);
        int elapsedHours = elapsedSeconds / (SECONDS_PER_MINUTE * MINUTES_PER_HOUR);
        elapsedSeconds %= SECONDS_PER_MINUTE * MINUTES_PER_HOUR;
        int elapsedMinutes = elapsedSeconds / SECONDS_PER_MINUTE;
        elapsedSeconds %= SECONDS_PER_MINUTE;
        totalTimeText.text = $"Total time: {elapsedHours} hr {elapsedMinutes} min {elapsedSeconds} sec";

        averageSpeedText.text = $"Average speed: {Mathf.RoundToInt(SeedFinderRunner.NUM_SEEDS / Time.unscaledTime)} seeds / sec";
    }
}
