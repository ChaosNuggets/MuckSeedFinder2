using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MainClass : MonoBehaviour
{
    public TextMeshProUGUI seedsTestedText;
    public TextMeshProUGUI estimatedTimeText;
    public TextMeshProUGUI speedText;

    private const int NUM_THREADS = 2;
    private readonly int[] startSeeds = new int[NUM_THREADS];
    private readonly int[] currentSeeds = new int[NUM_THREADS];
    private readonly int[] endSeeds = new int[NUM_THREADS];

    private readonly SeedCalculator[] seedCalculators = new SeedCalculator[NUM_THREADS];

    private static List<(int, float)> FindSeeds(SeedCalculator seedCalculator, int startSeed, int endSeed, out int currentSeed)
    {
        const float MAX_DISTANCE_TO_LOG = 3000;
        List<(int, float)> seeds = new();

        // currentSeed >= startSeed is for handling the overflow edge case
        for (currentSeed = startSeed; currentSeed >= startSeed && currentSeed < endSeed; currentSeed = seedCalculator.CalculateNextGodSeed())
        {
            HeightMap heightMap = new(currentSeed);

            Vector3 spawn = Spawn.FindSurvivalSpawn(currentSeed, heightMap);
            List<Vector3> chiefsChests = ChiefsChests.FindChiefsChests(currentSeed, heightMap, out List<Vector3> villages);
            List<Vector3> guardians = Guardians.FindGuardians(currentSeed, heightMap);
            Vector3 boat = Boat.FindBoat(currentSeed, heightMap);

            float distance = CalculateDistance.CalculateTotalDistance(spawn, chiefsChests, guardians, villages, boat);
            if (distance <= MAX_DISTANCE_TO_LOG)
            {
                seeds.Add((currentSeed, distance));
                Debug.Log($"Found seed {currentSeed}, distance {distance}");
            }
        }

        return seeds;
    }

    private List<(int, float)> FindSeeds()
    {
        const int SEEDS_PER_THREAD = 10000;
        List<(int, float)> seeds = new();
        var tasks = new Task<List<(int, float)>>[NUM_THREADS];

        for (int i = 0; i < NUM_THREADS; i++)
        {
            int threadEndSeed = currentSeeds[i] + SEEDS_PER_THREAD;
            if (threadEndSeed > endSeeds[i] || threadEndSeed < currentSeeds[i])
            {
                threadEndSeed = endSeeds[i];
            }
            int index = i; // avoid access to modified closure
            tasks[i] = Task.Run(() => FindSeeds(seedCalculators[index], currentSeeds[index], threadEndSeed, out currentSeeds[index]));
        }

        foreach (var task in tasks)
        {
            seeds.AddRange(task.Result);
        }

        return seeds;
    }

    private void UpdateText()
    {
        const int MINUTES_PER_HOUR = 60;
        const int SECONDS_PER_MINUTE = 60;

        int numTestedSeeds = 0;
        for (int i = 0; i < NUM_THREADS; i++)
        {
            numTestedSeeds += currentSeeds[i] - 1 - startSeeds[i];
        }

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
        FileStuff.LogSeeds(FindSeeds());
    }

    private void Awake()
    {
        const uint SEED_CHUNK_SIZE = uint.MaxValue / NUM_THREADS;

        startSeeds[0] = int.MinValue;
        for (int i = 0; i < NUM_THREADS - 1; i++)
        {
            startSeeds[i + 1] = (int)(startSeeds[i] + SEED_CHUNK_SIZE);
            endSeeds[i] = startSeeds[i + 1] - 1;
        }
        endSeeds[NUM_THREADS - 1] = int.MaxValue;

        for (int i = 0; i < NUM_THREADS; i++)
        {
            currentSeeds[i] = startSeeds[i];
            seedCalculators[i] = new SeedCalculator(startSeeds[i]);
        }
    }
}
