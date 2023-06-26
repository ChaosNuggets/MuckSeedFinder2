using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class MainClass : MonoBehaviour
{
    public const int START_SEED = int.MinValue;
    public const int END_SEED = int.MaxValue;
    public const uint NUM_SEEDS = (uint)((long)END_SEED - START_SEED);

    private const int NUM_THREADS = 10;
    private const int NUM_SEEDS_PER_FRAME = 10;
    private readonly int[] startSeeds = new int[NUM_THREADS];
    private readonly int[] endSeeds = new int[NUM_THREADS];

    private readonly SeedCalculator[] seedCalculators = new SeedCalculator[NUM_THREADS];
    private readonly bool[] isSeedChunkDone = new bool[NUM_THREADS];

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
        const float MAX_DISTANCE_TO_LOG = 2000;
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
            if (isSeedChunkDone[i])
            {
                continue;
            }

            int index = i; // avoid access to modified closure
            heightMapTasks[i] = Task.Run(() => GetHeightMaps(seedCalculators[index], startSeeds[index], endSeeds[index]));
        }

        var findSeedTasks = new Task<List<(int, float)>>[NUM_THREADS];
        for (int i = 0; i < NUM_THREADS; i++)
        {
            if (isSeedChunkDone[i])
            {
                continue;
            }

            var (seeds, heightMaps) = heightMapTasks[i].Result;

            var spawns = new Vector3[NUM_SEEDS_PER_FRAME];
            for (int j = 0; j < NUM_SEEDS_PER_FRAME; j++)
            {
                if (heightMaps[j] == null)
                {
                    isSeedChunkDone[i] = true;
                    break;
                }
                spawns[j] = Spawn.FindSurvivalSpawn(seeds[j], heightMaps[j]);
            }

            findSeedTasks[i] = Task.Run(() => FindSeeds(seeds, heightMaps, spawns));
        }

        for (int i = 0; i < NUM_THREADS; i++)
        {
            if (isSeedChunkDone[i])
            {
                continue;
            }

            godDistanceSeeds.AddRange(findSeedTasks[i].Result);
        }

        return godDistanceSeeds;
    }

    private void UpdateText()
    {
        int numTestedSeeds = 0;
        for (int i = 0; i < NUM_THREADS; i++)
        {
            numTestedSeeds += seedCalculators[i].currentSeed - 1 - startSeeds[i];
        }

        PrintStuff.instance.UpdateText(numTestedSeeds);
    }

    private bool HasTestedAllSeeds()
    {
        for (int i = 0; i < NUM_THREADS; i++)
        {
            if (!isSeedChunkDone[i])
            {
                return false;
            }
        }

        return true;
    }

    //private void Update()
    //{
    //    if (!HasTestedAllSeeds())
    //    {
    //        FileStuff.LogSeeds(FindSeeds());
    //        UpdateText();
    //    }
    //    else
    //    {
    //        enabled = false; // Stop the script from running
    //        PrintStuff.instance.WriteSummaryMessage();
    //    }
    //}

    //private void Awake()
    //{
    //    const uint SEED_CHUNK_SIZE = NUM_SEEDS / NUM_THREADS;

    //    startSeeds[0] = START_SEED;
    //    for (int i = 0; i < NUM_THREADS - 1; i++)
    //    {
    //        startSeeds[i + 1] = (int)(startSeeds[i] + SEED_CHUNK_SIZE);
    //        endSeeds[i] = startSeeds[i + 1] - 1;
    //    }
    //    endSeeds[NUM_THREADS - 1] = END_SEED;

    //    for (int i = 0; i < NUM_THREADS; i++)
    //    {
    //        seedCalculators[i] = new SeedCalculator(startSeeds[i]);
    //    }

    //    Array.Fill(isSeedChunkDone, false);
    //}

    private void Awake()
    {
        List<int> potentialSpearSeeds = new();
        for (int i = int.MinValue; i < int.MinValue + 1000000; i++)
        {
            var (isPotential, _) = CalculateItems.IsSpearSeed(i);
            if (isPotential)
            {
                potentialSpearSeeds.Add(i);
            }
        }

        HashSet<int> differences = new();
        for (int i = 1; i < potentialSpearSeeds.Count; i++)
        {
            int difference = potentialSpearSeeds[i] - potentialSpearSeeds[i - 1];
            differences.Add(difference);
        }

        foreach (int difference in differences)
        {
            UnityEngine.Debug.Log(difference);
        }
    }
}
