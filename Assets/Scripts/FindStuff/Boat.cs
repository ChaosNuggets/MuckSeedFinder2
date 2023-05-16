using UnityEngine;

public static class Boat
{
    public static Vector3 CalculateBoatPosition(int seed, HeightMap heightMap)
    {
        const float WATER_HEIGHT = 9.2874360f; 
        ConsistentRandom randomGen = new(seed);
        float x = (float)(randomGen.NextDouble() - 0.5);
        float z = (float)(randomGen.NextDouble() - 0.5);
        Vector3 origin = (HeightMap.MAP_CHUNK_SIZE / 2f) * HeightMap.WORLD_SCALE * new Vector3(x, 0, z).normalized;
        origin.y = WATER_HEIGHT + 1;
        heightMap.CoordRaycast(origin, new Vector3(0, origin.y, 0), out Vector3 hitPoint);
        return hitPoint;
    }
}
