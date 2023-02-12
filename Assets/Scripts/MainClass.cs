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

    private void Awake()
    {
        const int SEED = 1691052140;
        HeightMap heightMap = new HeightMap(SEED);
        Debug.Log(heightMap.CoordToHeightPrecise(343.5f, -60.2f)); // Should print 5.6 - 3.6
        Debug.Log(heightMap.CoordToHeightPrecise(-478.1f, -1102.4f)); // Should print 0
        Debug.Log(heightMap.CoordToHeightPrecise(-427.6f, -981.8f)); // Should print 7.2 - 3.6
        Debug.Log(heightMap.CoordToHeightPrecise(-17.3f, 5.5f)); // Should print 31.5 - 3.6
    }

    //private void Awake()
    //{
    //    const int SEED = 1691052140;
    //    HeightMap heightMap = new HeightMap(SEED);
    //    Boat.CalculateBoatPosition(SEED, heightMap);
    //}
}
