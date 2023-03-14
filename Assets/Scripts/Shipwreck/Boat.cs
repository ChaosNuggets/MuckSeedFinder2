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
        Vector3 origin = new Vector3(x, 0f, z).normalized * HeightMap.WORLD_SCALE * ((float)HeightMap.MAP_CHUNK_SIZE / 2f);
        Vector3 direction = Vector3.zero - origin;
        origin.y = heightMap.CoordToHeightPrecise(x, z) + 1;
        heightMap.CoordRaycast(new Ray(origin, direction), out Vector3 hitPoint);
        return hitPoint;
    }
}
