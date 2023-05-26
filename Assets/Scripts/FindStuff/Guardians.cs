using System.Collections.Generic;
using UnityEngine;

public static class Guardians
{
    public static List<Vector3> FindGuardians(int seed, HeightMap heightMap)
    {
        const int RESOURCE_GEN_OFFSET = 2;
        const int NUM_GUARDIANS = 5;
        const float WORLD_EDGE_BUFFER = 0.6f;
        const int GRASS_HEIGHT = 17;
        const float WORLD_SCALE = HeightMap.WORLD_SCALE * WORLD_EDGE_BUFFER;

        int numIterations = 0;
        ConsistentRandom rand = new(seed + RESOURCE_GEN_OFFSET);
        List<Vector3> guardians = new();

        while (guardians.Count < NUM_GUARDIANS)
        {
            numIterations++;

            float x = (float)(rand.NextDouble() * 2 - 1) * HeightMap.MAP_CHUNK_SIZE / 2f * WORLD_SCALE;
            float z = (float)(rand.NextDouble() * 2 - 1) * HeightMap.MAP_CHUNK_SIZE / 2f * WORLD_SCALE;
            Vector3 guardian = new(x, heightMap.CoordToHeight(x, z), z);

            if (guardian.y < GRASS_HEIGHT || heightMap.GetAngle(x, z) > 15f)
            {
                continue;
            }

            rand.Next(); // From GuardianSpawner.FindObjectToSpawn
            guardians.Add(guardian);

            if (numIterations > NUM_GUARDIANS * 100)
            {
                break;
            }
        }

        return guardians;
    }
}
