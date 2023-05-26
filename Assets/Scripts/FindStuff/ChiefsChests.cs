using System.Collections.Generic;
using UnityEngine;

public static class ChiefsChests
{
    public static List<Vector3> FindChiefsChests(int seed, HeightMap heightMap)
    {
        const int RESOURCE_GEN_OFFSET = 3;
        const int MAX_VILLAGES = 3;
        const int MIN_VILLAGES = 1;
        const float WORLD_EDGE_BUFFER = 0.6f;
        const int GRASS_HEIGHT = 17;
        const float WORLD_SCALE = HeightMap.WORLD_SCALE * WORLD_EDGE_BUFFER;

        int numIterations = 0;
        int numVillages = 0;
        ConsistentRandom rand = new(seed + RESOURCE_GEN_OFFSET);
        List<Vector3> chiefsChests = new();

        while (numVillages < MAX_VILLAGES)
        {
            numIterations++;
            // Calculate the center of the village
            float x = (float)(rand.NextDouble() * 2 - 1) * HeightMap.MAP_CHUNK_SIZE / 2f * WORLD_SCALE;
            float z = (float)(rand.NextDouble() * 2 - 1) * HeightMap.MAP_CHUNK_SIZE / 2f * WORLD_SCALE;
            Vector3 villageCenter = new(x, heightMap.CoordToHeight(x, z), z);
            if (villageCenter.y < GRASS_HEIGHT || heightMap.GetAngle(x, z) > 15f)
            {
                continue;
            }

            // If the village should be placed
            numVillages++;

            bool shouldBreak = numVillages >= MAX_VILLAGES || (numIterations > MAX_VILLAGES * 2 && numVillages >= MIN_VILLAGES) || numIterations > MAX_VILLAGES * 10;
            if (GenerateStructures(rand, villageCenter, heightMap, shouldBreak, out Vector3 chiefsChest))
            {
                chiefsChests.Add(chiefsChest);
            }

            if (shouldBreak)
            {
                break;
            }
        }

        return chiefsChests;
    }

    // Returns whether or not there's a chiefs chest in that village
    private static bool GenerateStructures(ConsistentRandom rand, Vector3 villageCenter, HeightMap heightMap, bool shouldSkipLaterCalcs, out Vector3 chiefsChest)
    {
        rand.Next(); // From CampSpawner.FindObjectToSpawn
        rand.Next(); // From GenerateCamp.GenerateZone

        float[] walllessWeirdThingDropChances =
        {
            0.3f,
            0.3f,
            0.4f,
            0.4f,
            0.3f,
            0.1f,
            0.3f,
            0.15f,
            0.25f,
            0.2f,
            0.15f,
            0.25f
        };

        // From GenerateCamp.GenerateStructures
        // Calculate the total number of structures to spawn
        const int NUM_CHIEFS_CHESTS = 1;
        int numWalllessWeirdThings = rand.Next(2, 4);
        int numHouseSpawners = rand.Next(2, 3);
        int numFireplaces = rand.Next(2, 4);
        int numBarrels = rand.Next(2, 7);
        int numLogs = rand.Next(2, 8);
        int numLogPiles = rand.Next(2, 5);
        int numRockPiles = rand.Next(2, 5);

        bool doesChiefsChestExist = SpawnObjects(NUM_CHIEFS_CHESTS, rand, villageCenter, heightMap, out chiefsChest) >= NUM_CHIEFS_CHESTS;

        if (shouldSkipLaterCalcs)
        {
            return doesChiefsChestExist;
        }

        int numHutsSpawned = SpawnObjects(numWalllessWeirdThings, rand, villageCenter, heightMap, out _);
        for (int i = 0; i < numHutsSpawned; i++)
        {
            SetRegularChests(rand, walllessWeirdThingDropChances);
        }

        SpawnObjectsSimple(numFireplaces, rand);
        SpawnObjectsSimple(numBarrels, rand);
        SpawnObjectsSimple(numLogs, rand);
        SpawnObjectsSimple(numLogPiles, rand);
        SpawnObjectsSimple(numRockPiles, rand);

        SpawnHouseSpawners(numHouseSpawners, rand, villageCenter, heightMap);

        return doesChiefsChestExist;
    }
    
    // Returns the number of objects spawned
    private static int SpawnObjects(int amount, ConsistentRandom rand, Vector3 villageCenter, HeightMap heightMap, out Vector3 hitPoint)
    {
        int objectsSpawned = 0;
        hitPoint = new();
        for (int i = 0; i < amount; i++)
        {
            if (FindPos(rand, villageCenter, heightMap, out hitPoint))
            {
                objectsSpawned++;
            }
        }

        return objectsSpawned;
    }

    private static void SpawnObjectsSimple(int amount, ConsistentRandom rand)
    {
        for (int i = 0; i < amount; i++)
        {
            // From GenerateCamp.RandomSpherePos
            rand.Next();
            rand.Next();
            rand.Next();
        }
    }

    private static void SpawnHouseSpawners(int amount, ConsistentRandom rand, Vector3 villageCenter, HeightMap heightMap)
    {
        float[][] dropChances =
        {
            new[] { 0.7f, 0.3f, 0.2f, 0.05f, 0.3f, 0.05f, 0.5f, 0.5f, 0.5f, 0.2f, 0.2f, 0.2f },
            new[] { 0.3f, 0.5f, 0.3f, 0.3f, 0.4f, 0.2f, 0.2f, 0.1f, 0.05f, 0.5f, 0.05f },
            new[] { 0.5f, 0.2f, 0.1f, 0.1f, 0.2f, 0.4f, 0.3f, 0.1f, 0.3f, 0.2f, 0.2f, 0.05f, 0.005f, 0.5f, 0.1f },
            new[] { 0.3f, 0.5f, 0.5f, 0.1f, 0.01f, 0.3f, 0.1f, 0.01f, 0.5f, 0.1f, 0.3f, 0.3f },
            new[] { 0.5f, 0.5f, 0.1f, 0.01f, 0.5f, 0.5f, 0.3f, 0.1f, 0.2f, 0.5f, 0.1f, 0.05f, 0.1f, 0.1f, 0.1f, 0.1f, 0.01f, 0.05f }
        };

        for (int i = 0; i < amount; i++)
        {
            int houseIdx = FindObjectToSpawn(rand);

            if (FindPos(rand, villageCenter, heightMap, out Vector3 hitPoint))
            {
                if (houseIdx < 5)
                {
                    SetRegularChests(rand, dropChances[houseIdx]);
                }
                else
                {
                    SetPowerupChests(rand);
                }
            }
        }
    }

    private static int FindObjectToSpawn(ConsistentRandom rand)
    {
        const float TOTAL_WEIGHT = 1.15f;
        float[] weights =
        {
            0.5f,  // 0
            0.25f, // 1
            0.15f, // 2
            0.13f, // 3
            0.08f, // 4
            0.04f  // 5 - the only one with spawnPowerups
        };

        float randNum = (float)rand.NextDouble();
        float cumulativeWeight = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randNum < cumulativeWeight / TOTAL_WEIGHT)
            {
                return i;
            }
        }

        return 0;
    }

    private static bool FindPos(ConsistentRandom rand, Vector3 villageCenter, HeightMap heightMap, out Vector3 hitPoint)
    {
        const int WATER_HEIGHT = 14;
        const int CAMP_RADIUS = 80;

        Vector3 spherePos = RandomSpherePos(rand) * CAMP_RADIUS;
        Vector3 pos = villageCenter + spherePos;

        hitPoint = heightMap.SphereCastDown(pos.x, pos.z, 1f);
        return hitPoint.y >= WATER_HEIGHT;
    }

    private static Vector3 RandomSpherePos(ConsistentRandom rand)
    {
        float x = (float)rand.NextDouble() * 2 - 1;
        float y = (float)rand.NextDouble() * 2 - 1;
        float z = (float)rand.NextDouble() * 2 - 1;
        return new Vector3(x, y, z).normalized;
    }

    private static void SetRegularChests(ConsistentRandom rand, float[] dropChances)
    {
        const int POSITIONS_LENGTH = 2;

        int numChests = rand.Next(0, POSITIONS_LENGTH) + 1;
        for (int i = 0; i < numChests; i++)
        {
            rand.Next();
            rand.Next();
            rand.Next(); // From SpawnChestsInLocations.FindLootTable

            int numLootItems = GetLoot(rand, dropChances);
            InitChest(numLootItems, rand);
        }
    }

    private static int GetLoot(ConsistentRandom rand, float[] dropChances)
    {
        int numLootItems = 0;
        foreach (float dropChance in dropChances)
        {
            if (rand.NextDouble() < dropChance)
            {
                numLootItems++;
            }
        }

        return numLootItems;
    }

    private static void InitChest(int numLootItems, ConsistentRandom rand)
    {
        for (int i = 0; i < numLootItems; i++)
        {
            rand.Next();
        }
    }

    private static void SetPowerupChests(ConsistentRandom rand)
    {
        const int POSITIONS_LENGTH = 5;

        int numChests = rand.Next(0, POSITIONS_LENGTH) + 1;
        for (int i = 0; i < numChests; i++)
        {
            rand.Next();
            rand.Next(); // From SpawnPowerupsInLocations.FindObjectToSpawn
        }
    }
}
