using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainClass : MonoBehaviour
{
    public TextMeshProUGUI seedsFoundText;

    private readonly SeedCalculator seedCalculator = new(int.MinValue);
    private int startSeed;

    private void Update()
    {
        const float MAX_DISTANCE_TO_LOG = 3000;
        const int SEEDS_BETWEEN_UPDATES = 10000;

        seedsFoundText.text = $"Seeds Tested:\n{startSeed - int.MinValue} / {uint.MaxValue}";

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

    private void Awake()
    {
        startSeed = seedCalculator.CalculateNextGodSeed();
    }

}
