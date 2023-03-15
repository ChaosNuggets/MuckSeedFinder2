using UnityEngine;

public class MainClass : MonoBehaviour
{
    //private void Awake()
    //{
    //    const int StartSeed = int.MinValue;
    //    const int SeedsToLog = 100000;

    //    SeedCalculator seedCalculator = new SeedCalculator(StartSeed);
    //    var watch = System.Diagnostics.Stopwatch.StartNew();
    //    int[] godSeeds = seedCalculator.CalculateNextGodSeeds(SeedsToLog);
    //    watch.Stop();
    //    Debug.Log($"Execution Time: {watch.ElapsedMilliseconds} ms");

    //    FileStuff.LogSeed(godSeeds);
    //}

    // Tests
    private void Awake()
    {
        const int SEED = 1691052140;
        HeightMap heightMap = new HeightMap(SEED);

        // Coord to heights
        Debug.Log(heightMap.CoordToHeight(343.5f, -60.2f)); // Should print 1.3927
        Debug.Log(heightMap.CoordToHeight(-478.1f, -1102.4f)); // Should print 0.0000
        Debug.Log(heightMap.CoordToHeight(-427.6f, -981.8f)); // Should print 3.6089
        Debug.Log(heightMap.CoordToHeight(-17.3f, 5.5f)); // Should print 27.8761

        // Raycasts
        Debug.Log(Boat.CalculateBoatPosition(SEED, heightMap).ToString("F5")); // Should print (-428.84430, 10.28744, -940.10740)
        Debug.Log(heightMap.CoordRaycast(new Vector3(-500, 500, 1235), Vector3.zero, out Vector3 hitPoint));
        Debug.Log(hitPoint);
        Debug.Log(heightMap.CoordRaycast(new Vector3(0, 500, 0), new Vector3(23, 500, 0), out Vector3 hitPoint2));
        Debug.Log(hitPoint2);
        Debug.Log(heightMap.CoordRaycast(new Vector3(5000, 2, 24322), new Vector3(0, 0, 0), out Vector3 hitPoint3));
        
        // Spawn position
        Debug.Log(Spawn.FindSurvivalSpawnPosition(SEED, heightMap).ToString("F5")); // Should print (-17.30739, 28.87648, 5.51006)
        Debug.Log(Spawn.FindSurvivalSpawnPosition(-2147483017, new HeightMap(-2147483017)).ToString("F5")); // Should print (-110.04690, 15.82334, -652.53750)
    }

    //private void Awake()
    //{
    //    float lowestDistance = float.MaxValue;
    //    for (int seed = int.MinValue; seed <= int.MaxValue; seed++)
    //    {
    //        HeightMap heightMap = new HeightMap(seed);
    //        float distance = Boat.CalculateBoatPosition(seed, heightMap).magnitude;
    //        if (distance < lowestDistance)
    //        {
    //            lowestDistance = distance;
    //            FileStuff.Log($"{seed}: {distance}");
    //        }
    //    }
    //}
}
