using UnityEngine;

public static class MapGenerator
{

    public const int MAP_CHUNK_SIZE = 241;
    public const int WORLD_SCALE = 12;

    public static HeightMap GenerateHeightMap(int seed)
    {
        const float HEIGHT_MULTIPLIER = 100f;
        const float NOISE_SCALE = 20;
        const int OCTAVES = 4;
        const float PERSISTANCE = 0.1f;
        const float LACUNARITY = 6;
        const float BLEND = 0.01f;
        const float BLEND_STRENGTH = 2;
        Vector2 offset = new Vector2(0, 0);

        float[,] heightMap = Noise.GenerateNoiseMap(MAP_CHUNK_SIZE, MAP_CHUNK_SIZE, seed, NOISE_SCALE, OCTAVES, PERSISTANCE, LACUNARITY, BLEND, BLEND_STRENGTH, offset);
        float[,] falloffMap = FalloffGenerator.GenerateFalloffMap(MAP_CHUNK_SIZE);
            for (int index1 = 0; index1<MAP_CHUNK_SIZE; ++index1)
        {
            for (int index2 = 0; index2<MAP_CHUNK_SIZE; ++index2)
            {
                heightMap[index2, index1] = Mathf.Clamp(heightMap[index2, index1] - falloffMap[index2, index1], 0.0f, 1f);
                heightMap[index2, index1] = HeightCurve.Evaluate(heightMap[index2, index1])* HEIGHT_MULTIPLIER;
}
        }
        return new HeightMap(heightMap);
    }

    
}
