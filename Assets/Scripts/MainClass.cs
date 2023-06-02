using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainClass : MonoBehaviour
{
    public TextMeshProUGUI seedsTestedText;
    public TextMeshProUGUI estimatedTimeText;
    public TextMeshProUGUI speedText;

    private readonly SeedCalculator seedCalculator = new(int.MinValue);
    private int startSeed;

    private void FindSeeds()
    {
        const float MAX_DISTANCE_TO_LOG = 3000;
        const int SEEDS_BETWEEN_UPDATES = 50000;

        int endSeed = startSeed + SEEDS_BETWEEN_UPDATES;
        if (endSeed < startSeed) // If statement for handling overflow
        {
            endSeed = int.MaxValue;
        }

        // seed >= startSeed is for handling the overflow edge case
        int seed = startSeed;
        for (; seed >= startSeed && seed < endSeed; seed = seedCalculator.CalculateNextGodSeed())
        {
            HeightMap heightMap = new(seed);

            Vector3 spawn = Spawn.FindSurvivalSpawn(seed, heightMap);
            List<Vector3> chiefsChests = ChiefsChests.FindChiefsChests(seed, heightMap, out List<Vector3> villages);
            List<Vector3> guardians = Guardians.FindGuardians(seed, heightMap);
            Vector3 boat = Boat.FindBoat(seed, heightMap);

            float distance = CalculateDistance.CalculateShortestDistance(spawn, chiefsChests, guardians, villages, boat);
            if (distance < MAX_DISTANCE_TO_LOG)
            {
                FileStuff.LogSeed(seed, distance);
            }
        }

        startSeed = seed;
    }

    private void UpdateText()
    {
        const int MINUTES_PER_HOUR = 60;
        const int SECONDS_PER_MINUTE = 60;

        int numTestedSeeds = startSeed - 1 - int.MinValue;
        seedsTestedText.text = $"Seeds Tested:\n{numTestedSeeds} / {uint.MaxValue}";

        float secondsLeft = Time.unscaledTime / numTestedSeeds * (uint.MaxValue - numTestedSeeds);
        int minutesLeft = Mathf.RoundToInt(secondsLeft / SECONDS_PER_MINUTE);
        int hoursLeft = minutesLeft / MINUTES_PER_HOUR;
        minutesLeft %= MINUTES_PER_HOUR;

        estimatedTimeText.text = $"Estimated Time Remaining:\n{hoursLeft} hr {minutesLeft} min";

        speedText.text = $"Speed:\n{Mathf.RoundToInt(numTestedSeeds / Time.unscaledTime)} seeds / sec";
    }

    private void Update()
    {
        UpdateText();
        FindSeeds();
    }

    private void Awake()
    {
        startSeed = seedCalculator.CalculateNextGodSeed();
    }

}
