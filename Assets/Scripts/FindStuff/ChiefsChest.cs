using System.Collections.Generic;
using UnityEngine;

public static class ChiefsChest
{
    public static List<Vector3> FindChiefsChests(int seed, HeightMap heightMap)
    {
        const int RESOURCE_GEN_OFFSET = 3;
        const int MAX_VILLAGES = 3;
        const int MIN_VILLAGES = 1;
        const float WORLD_EDGE_BUFFER = 0.6f;
        const int GRASS_HEIGHT = 17;

        List<Vector3> chiefsChests = new();
        ConsistentRandom rand = new(seed + RESOURCE_GEN_OFFSET);
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
            float x = (float)(rand.NextDouble() * 2 - 1) * HeightMap.MAP_CHUNK_SIZE / 2f * WORLD_SCALE;
            float z = (float)(rand.NextDouble() * 2 - 1) * HeightMap.MAP_CHUNK_SIZE / 2f * WORLD_SCALE;
            Vector3 villageCenter = new(x, heightMap.CoordToHeight(x, z), z);
            if (villageCenter.y < GRASS_HEIGHT || heightMap.GetAngle(x, z) > 15f)
            {
                continue;
            }

            // If the village should be placed
            totalVillages++;
            rand.Next(); // From CampSpawner.FindObjectToSpawn
            CalculateChiefsChestPos(villageCenter, rand);
        }

        return chiefsChests;
    }

    private static void CalculateChiefsChestPos(Vector3 villageCenter, ConsistentRandom rand)
    {
        const int CAMP_RADIUS = 80;
        for (int i = 0; i < 8; i++)
        {
            // One from GenerateCamp.GenerateZone and 7 from GenerateCamp.GenerateStructures
            rand.Next(); 
        }

        Vector3 distance = RandomSpherePos(rand) * CAMP_RADIUS;
        Debug.Log($"Village placed at {villageCenter}");
    }

    private static Vector3 RandomSpherePos(ConsistentRandom rand)
    {
        float x = (float)rand.NextDouble() * 2 - 1;
        float y = (float)rand.NextDouble() * 2 - 1;
        float z = (float)rand.NextDouble() * 2 - 1;
        return new Vector3(x, y, z).normalized;
    }
}
