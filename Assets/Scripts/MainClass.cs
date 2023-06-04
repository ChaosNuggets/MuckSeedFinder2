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

    private const int NUM_THREADS = 5;
    private const int NUM_SEEDS_PER_FRAME = 10;
    private readonly int[] startSeeds = new int[NUM_THREADS];
    private readonly int[] endSeeds = new int[NUM_THREADS];

    private readonly SeedCalculator[] seedCalculators = new SeedCalculator[NUM_THREADS];

    private static (int[], HeightMap[]) GetHeightMaps(SeedCalculator seedCalculator, int startSeed, int endSeed)
    {
        HeightMap[] heightMaps = new HeightMap[NUM_SEEDS_PER_FRAME];
        int[] seeds = seedCalculator.CalculateNextGodSeeds(NUM_SEEDS_PER_FRAME);

        // seeds[i] >= startSeed is for handling the overflow edge case
        int i = 0;
        for (; i < NUM_SEEDS_PER_FRAME && seeds[i] <= endSeed && seeds[i] >= startSeed; i++)
        {
            heightMaps[i] = new HeightMap(seeds[i]);
        }

        // For the seeds that are out of the bounds of startSeed and endSeed, set the heightMap to null
        for (; i < NUM_SEEDS_PER_FRAME; i++)
        {
            heightMaps[i] = null;
        }

        return (seeds, heightMaps);
    }

    private static List<(int, float)> FindSeeds(int[] seeds, HeightMap[] heightMaps, Vector3[] spawns)
    {
        const float MAX_DISTANCE_TO_LOG = 3000;
        List<(int, float)> godDistanceSeeds = new();

        for (int i = 0; i < NUM_SEEDS_PER_FRAME && heightMaps[i] != null; i++)
        {
            List<Vector3> chiefsChests = ChiefsChests.FindChiefsChests(seeds[i], heightMaps[i], out List<Vector3> villages);
            List<Vector3> guardians = Guardians.FindGuardians(seeds[i], heightMaps[i]);
            Vector3 boat = Boat.FindBoat(seeds[i], heightMaps[i]);

            float distance = CalculateDistance.CalculateTotalDistance(spawns[i], chiefsChests, guardians, villages, boat);
            if (distance <= MAX_DISTANCE_TO_LOG)
            {
                godDistanceSeeds.Add((seeds[i], distance));
                //Debug.Log($"Found seed {seeds[i]}, distance {distance}");
            }
        }

        return godDistanceSeeds;
    }

    private List<(int, float)> FindSeeds()
    {
        List<(int, float)> godDistanceSeeds = new();
        var heightMapTasks = new Task<(int[], HeightMap[])>[NUM_THREADS];

        for (int i = 0; i < NUM_THREADS; i++)
        {
            int index = i; // avoid access to modified closure
            heightMapTasks[i] = Task.Run(() => GetHeightMaps(seedCalculators[index], startSeeds[index], endSeeds[index]));
        }

        var findSeedTasks = new Task<List<(int, float)>>[NUM_THREADS];
        for (int i = 0; i < NUM_THREADS; i++)
        {
            var spawns = new Vector3[NUM_SEEDS_PER_FRAME];
            var (seeds, heightMaps) = heightMapTasks[i].Result;
            for (int j = 0; j < NUM_SEEDS_PER_FRAME; j++)
            {
                if (seeds[j] == -2147320059
                    || seeds[j] == -2147259148
                    || seeds[j] == -2147115284
                    || seeds[j] == -2147001245
                    || seeds[j] == -2146970159
                    || seeds[j] == -2146939073
                    || seeds[j] == -2146826333
                    || seeds[j] == -2146725197
                    || seeds[j] == -2146664324
                    || seeds[j] == -2146652682
                    || seeds[j] == -2146569729
                    || seeds[j] == -2146530841
                    || seeds[j] == -2146499736
                    )
                {
                    Debug.Log(seeds[j]);
                }
                if (heightMaps[j] == null)
                {
                    break;
                }
                spawns[j] = Spawn.FindSurvivalSpawn(seeds[j], heightMaps[j]);
                if (seeds[j] == -2147320059
                    || seeds[j] == -2147259148
                    || seeds[j] == -2147115284
                    || seeds[j] == -2147001245
                    || seeds[j] == -2146970159
                    || seeds[j] == -2146939073
                    || seeds[j] == -2146826333
                    || seeds[j] == -2146725197
                    || seeds[j] == -2146664324
                    || seeds[j] == -2146652682
                    || seeds[j] == -2146569729
                    || seeds[j] == -2146530841
                    || seeds[j] == -2146499736
                    )
                {
                    Debug.Log(seeds[j]);
                }
            }

            findSeedTasks[i] = Task.Run(() => FindSeeds(seeds, heightMaps, spawns));
        }

        foreach (var findSeedTask in findSeedTasks)
        {
            godDistanceSeeds.AddRange(findSeedTask.Result);
        }

        return godDistanceSeeds;
    }

    private void UpdateText()
    {
        const int MINUTES_PER_HOUR = 60;
        const int SECONDS_PER_MINUTE = 60;

        int numTestedSeeds = 0;
        for (int i = 0; i < NUM_THREADS; i++)
        {
            numTestedSeeds += seedCalculators[i].currentSeed - 1 - startSeeds[i];
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
            seedCalculators[i] = new SeedCalculator(startSeeds[i]);
        }
    }
}
