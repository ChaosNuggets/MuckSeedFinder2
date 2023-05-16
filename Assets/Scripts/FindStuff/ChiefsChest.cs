using System.Collections.Generic;
using UnityEngine;

public static class ChiefsChest
{
    public static List<Vector3> FindChiefsChests(int seed, HeightMap heightMap)
    {
        const int RESOURCE_GEN_OFFSET = 3;
        const int MAX_VILLAGES = 50;
        const int MIN_VILLAGES = 3;
        const float WORLD_EDGE_BUFFER = 0.6f;
        const int GRASS_HEIGHT = 17;

        List<Vector3> chiefsChests = new();
        ConsistentRandom randomGen = new(seed + RESOURCE_GEN_OFFSET);
        const float WORLD_SCALE = HeightMap.WORLD_SCALE * WORLD_EDGE_BUFFER;

        int totalVillages = 0;
        int i;
        for (i = 0; i < MAX_VILLAGES * 10; i++)
        {
            if (totalVillages >= MAX_VILLAGES || (i >= MAX_VILLAGES * 2 && totalVillages >= MIN_VILLAGES))
            {
                break;
            }
            
            // Calculate the center of the village
            float x = (float)(randomGen.NextDouble() * 2 - 1) * HeightMap.MAP_CHUNK_SIZE / 2f * WORLD_SCALE;
            float z = (float)(randomGen.NextDouble() * 2 - 1) * HeightMap.MAP_CHUNK_SIZE / 2f * WORLD_SCALE;
            Vector3 villageCenter = new(x, heightMap.CoordToHeight(x, z), z);
            //Debug.Log($"villageCenter: {villageCenter}, angle: {heightMap.GetAngle(x, z)}");
            if (villageCenter.y < GRASS_HEIGHT || heightMap.GetAngle(x, z) > 15f)
            {
                continue;
            }

            // If the village should be placed
            totalVillages++;
            CalculateChiefsChestPos(villageCenter);
        }
        Debug.Log($"Loop ran {i} times");

        return chiefsChests;
    }

    private static void CalculateChiefsChestPos(Vector3 villageCenter)
    {
        Debug.Log($"Village placed at {villageCenter}");
    }
}
