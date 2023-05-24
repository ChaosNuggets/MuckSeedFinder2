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

        int numVillages = 0;
        ConsistentRandom rand = new(seed + RESOURCE_GEN_OFFSET);
        List<Vector3> chiefsChests = new();

        for (int i = 0; i < MAX_VILLAGES * 10; i++)
        {
            if (numVillages >= MAX_VILLAGES || (i >= MAX_VILLAGES * 2 && numVillages >= MIN_VILLAGES))
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
            numVillages++;

            Debug.Log("Village");
            if (GenerateStructures(rand, villageCenter, heightMap, out Vector3 chiefsChest))
            {
                chiefsChests.Add(chiefsChest);
            }
        }

        return chiefsChests;
    }

    // Returns whether or not there's a chiefs chest in that village
    private static bool GenerateStructures(ConsistentRandom rand, Vector3 villageCenter, HeightMap heightMap, out Vector3 chiefsChest)
    {
        rand.Next(); // From CampSpawner.FindObjectToSpawn
        rand.Next(); // From GenerateCamp.GenerateZone

        // From GenerateCamp.GenerateStructures
        // Calculate the total number of structures to spawn
        int numChiefsChests = 1;
        int numHuts = rand.Next(2, 4);
        int numHouses = rand.Next(2, 3);
        int numFireplaces = rand.Next(2, 4);
        int numBarrels = rand.Next(2, 7);
        int numLogs = rand.Next(2, 8);
        int numLogPiles = rand.Next(2, 5);
        int numRockPiles = rand.Next(2, 5);

        bool doesChiefsChestExist = SpawnObjects(numChiefsChests, rand, villageCenter, heightMap, out chiefsChest) >= numChiefsChests;

        int numHutsSpawned = SpawnObjects(numHuts, rand, villageCenter, heightMap, out _);
        for (int i = 0; i < numHutsSpawned; i++)
        {
            SetChests(rand);
        }

        SpawnObjectsSimple(numFireplaces, rand);
        SpawnObjectsSimple(numBarrels, rand);
        SpawnObjectsSimple(numLogs, rand);
        SpawnObjectsSimple(numLogPiles, rand);
        SpawnObjectsSimple(numRockPiles, rand);

        //SpawnObjects(this.houseSpawner, numHouses, rand);

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

    //private List<GameObject> SpawnObjects(StructureSpawner houses, int amount, ConsistentRandom rand)
    //{
    //    List<GameObject> list = new List<GameObject>();
    //    houses.CalculateWeight();
    //    for (int i = 0; i < amount; i++)
    //    {
    //        GameObject original = houses.FindObjectToSpawn(houses.structurePrefabs, houses.totalWeight, rand);
    //        RaycastHit raycastHit = this.FindPos(rand);
    //        if (!(raycastHit.collider == null))
    //        {
    //            GameObject gameObject = Object.Instantiate<GameObject>(original, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
    //            int nextId = ResourceManager.Instance.GetNextId();
    //            gameObject.GetComponent<Hitable>().SetId(nextId);
    //            ResourceManager.Instance.AddObject(nextId, gameObject);
    //            SpawnChestsInLocations componentInChildren = gameObject.GetComponentInChildren<SpawnChestsInLocations>();
    //            if (componentInChildren)
    //            {
    //                componentInChildren.SetChests(rand);
    //            }
    //            SpawnPowerupsInLocations componentInChildren2 = gameObject.GetComponentInChildren<SpawnPowerupsInLocations>();
    //            if (componentInChildren2)
    //            {
    //                componentInChildren2.SetChests(rand);
    //            }
    //            Hitable component = gameObject.GetComponent<Hitable>();
    //            if (component)
    //            {
    //                int nextId2 = ResourceManager.Instance.GetNextId();
    //                component.SetId(nextId2);
    //                ResourceManager.Instance.AddObject(nextId2, gameObject);
    //            }
    //            list.Add(gameObject);
    //        }
    //    }
    //    return list;
    //}

    private static bool FindPos(ConsistentRandom rand, Vector3 villageCenter, HeightMap heightMap, out Vector3 hitPoint)
    {
        const int WATER_HEIGHT = 14;
        const int CAMP_RADIUS = 80;

        Vector3 spherePos = RandomSpherePos(rand) * CAMP_RADIUS;
        Vector3 pos = villageCenter + spherePos;

        hitPoint = heightMap.SphereCastDown(pos.x, pos.z, 1f);
        Debug.Log($"hitPoint: {hitPoint}");
        return hitPoint.y >= WATER_HEIGHT;
    }
    private static Vector3 RandomSpherePos(ConsistentRandom rand)
    {
        float x = (float)rand.NextDouble() * 2 - 1;
        float y = (float)rand.NextDouble() * 2 - 1;
        float z = (float)rand.NextDouble() * 2 - 1;
        return new Vector3(x, y, z).normalized;
    }

    private static void SetChests(ConsistentRandom rand) { }
}
