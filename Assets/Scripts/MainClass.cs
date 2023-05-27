using System.Collections.Generic;
using UnityEngine;

public class MainClass : MonoBehaviour
{
    private static void RunIndividualTests()
    {
        //const int SEED = 1691052140;
        //const int SEED = 68645856;
        //const int SEED = -296416513;
        const int SEED = -2147483348;
        HeightMap heightMap = new(SEED);

        // Coord to heights
        Debug.Log(heightMap.CoordToHeight(343.5f, -60.2f)); // Should print 1.3927
        Debug.Log(heightMap.CoordToHeight(-478.1f, -1102.4f)); // Should print 0.0000
        Debug.Log(heightMap.CoordToHeight(-427.6f, -981.8f)); // Should print 3.6089
        Debug.Log(heightMap.CoordToHeight(-17.3f, 5.5f)); // Should print 27.8761

        //// Raycasts
        //Debug.Log(Boat.FindBoat(SEED, heightMap).ToString("F5")); // Should print (-428.84430, 10.28744, -940.10740)
        //Debug.Log(heightMap.CoordRaycast(new Vector3(-500, 500, 1235), Vector3.zero, out Vector3 hitPoint));
        //Debug.Log(hitPoint);
        //Debug.Log(heightMap.CoordRaycast(new Vector3(0, 500, 0), new Vector3(23, 500, 0), out Vector3 hitPoint2));
        //Debug.Log(hitPoint2);
        //Debug.Log(heightMap.CoordRaycast(new Vector3(5000, 2, 24322), new Vector3(0, 0, 0), out _));

        //// Spawn position
        //Debug.Log(Spawn.FindSurvivalSpawn(SEED, heightMap).ToString("F5")); // Should print (-17.30739, 28.87648, 5.51006)
        //Debug.Log(Spawn.FindSurvivalSpawn(-2147483017, new HeightMap(-2147483017)).ToString("F5")); // Should print (-110.04690, 15.82334, -652.53750)

        //// Chiefs chests
        //List<Vector3> chiefsChests = ChiefsChests.FindChiefsChests(SEED, heightMap);
        //foreach (var chiefsChest in chiefsChests)
        //{
        //    Debug.Log(chiefsChest);
        //}

        //// Guardian locations
        //List<Vector3> guardians = Guardians.FindGuardians(SEED, heightMap);
        //foreach (var guardian in guardians)
        //{
        //    Debug.Log(guardian);
        //}
    }

    private static void RunCombinedTest()
    {
        int seed = -2147474266;
        Debug.Log($"seed: {seed}");
        HeightMap heightMap = new(seed);

        Vector3 spawn = Spawn.FindSurvivalSpawn(seed, heightMap);
        Debug.Log($"spawn: {spawn}");

        List<Vector3> chiefsChests = ChiefsChests.FindChiefsChests(seed, heightMap, out List<Vector3> villages);
        foreach (var chiefsChest in chiefsChests)
        {
            Debug.Log($"chiefsChest: {chiefsChest}");
        }

        List<Vector3> guardians = Guardians.FindGuardians(seed, heightMap);
        foreach (var guardian in guardians)
        {
            Debug.Log($"guardian: {guardian}");
        }

        Vector3 boat = Boat.FindBoat(seed, heightMap);
        Debug.Log($"boat: {boat}");

        float distance = CalculateDistance.CalculateShortestDistance(spawn, chiefsChests, guardians, villages, boat);
        Debug.Log($"distance: {distance}");
        //if (distance < MAX_DISTANCE_TO_LOG)
        //{
        //    FileStuff.LogSeed(seed, distance);
        //}
    }

    private static void FindSeeds()
    {
        const float MAX_DISTANCE_TO_LOG = 3000;
        SeedCalculator seedCalculator = new(int.MinValue);
        int firstGodSeed = seedCalculator.CalculateNextGodSeed();
        int seed = firstGodSeed;

        do 
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

            seed = seedCalculator.CalculateNextGodSeed();
        } while (seed != firstGodSeed);
    }

    private void Awake()
    {
        FindSeeds();
    }
}
