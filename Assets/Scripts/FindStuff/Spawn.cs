using UnityEngine;

public static class Spawn
{
    public static Vector3 FindSurvivalSpawnPosition(int seed, HeightMap heightMap)
    {
        const float MAP_RADIUS = 1100f;
        const int SAND_LAYER_START = 14;

        Vector3 spawnCircleCenter = Vector3.zero;
        Vector3 spawnPosition = new Vector3(0, 50, 0);

        // Calculate spawnCircleCenter
        Random.InitState(seed);
        Vector2 maybeSpawnCircleCenter = Random.insideUnitCircle * MAP_RADIUS;
        float maybeCenterHeight = heightMap.CoordToHeight(maybeSpawnCircleCenter.x, maybeSpawnCircleCenter.y);
        if (maybeCenterHeight >= SAND_LAYER_START)
        {
            spawnCircleCenter = new Vector3(maybeSpawnCircleCenter.x, maybeCenterHeight, maybeSpawnCircleCenter.y);
        }

        // Calculate spawnPosition
        for (int j = 0; j < 100; j++)
        {
            Vector2 maybeSpawnPosition = Random.insideUnitCircle * 50f + VectorConvert.ToVector2(spawnCircleCenter);
            float height = heightMap.CoordToHeight(maybeSpawnPosition.x, maybeSpawnPosition.y);
            if (height >= SAND_LAYER_START)
            {
                spawnPosition = new Vector3(maybeSpawnPosition.x, height + 1, maybeSpawnPosition.y);
                break;
            }
        }

        return spawnPosition;
    }
}