using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Boat
{
    public static Vector3 CalculateBoatPosition(int seed, HeightMap heightMap)
    {
        ConsistentRandom randomGen = new ConsistentRandom(seed);
        float x = (float)(randomGen.NextDouble() - 0.5);
        float z = (float)(randomGen.NextDouble() - 0.5);
        Vector3 origin = new Vector3(x, 0f, z).normalized * HeightMap.WORLD_SCALE * (HeightMap.MAP_CHUNK_SIZE / 2f);
        origin.y = heightMap.CoordToHeightPrecise(origin.x, origin.z) + 1;
        heightMap.CoordRaycast(origin, new Vector3(0, origin.y, 0), out Vector3 hitPoint);
        return hitPoint;
    }
}
