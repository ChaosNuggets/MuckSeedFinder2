using UnityEngine;

public static class MapGenerator
{
    private const float heightMultiplier = 100f;

    private const int mapChunkSize = 241;
    private const int worldScale = 12;

    private const float noiseScale = 20;
    private const int octaves = 4;
    private const float persistance = 0.1f;
    private const float lacunarity = 6;
    private const float blend = 0.01f;
    private const float blendStrength = 2;
    private static readonly Vector2 offset = new Vector2(0, 0);

    public static float[,] GenerateHeightMap(int seed)
    {
        float[,] heightMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, blend, blendStrength, offset);
        float[,] falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
        for (int index1 = 0; index1 < mapChunkSize; ++index1)
        {
            for (int index2 = 0; index2 < mapChunkSize; ++index2)
            {
                heightMap[index2, index1] = Mathf.Clamp(heightMap[index2, index1] - falloffMap[index2, index1], 0.0f, 1f);
                heightMap[index2, index1] = HeightCurve.Evaluate(heightMap[index2, index1]) * heightMultiplier; 
            }
        }
        return heightMap;
    }

    private static (float, float) IndexToCoord(float row, float column)
    {
        // THIS IS WHAT THESE VARIABLES ARE CALLED IN MUCK'S SOURCE CODE PLEASE DON'T BULLY ME GO BULLY DANI
        float num1 = (float) (mapChunkSize - 1) / -2f;
        float num2 = (float) (mapChunkSize - 1) / 2f;
        return ((num1 + row) * worldScale, (num2 - column) * worldScale);
    }
    
    private static (float, float) CoordToIndex(float x, float z)
    {
        float num1 = (float) (mapChunkSize - 1) / -2f;
        float num2 = (float) (mapChunkSize - 1) / 2f;
        return (x / worldScale - num1, num2 - z / worldScale);
    }

    public static float GetHeight(float[,] heightMap, float x, float z)
    {
        var (row, column) = CoordToIndex(x, z);
        int rowIndex = Mathf.RoundToInt(row);
        int columnIndex = Mathf.RoundToInt(column);
        return heightMap[rowIndex, columnIndex];
    }
}
