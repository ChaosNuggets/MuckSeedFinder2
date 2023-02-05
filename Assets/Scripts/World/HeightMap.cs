using UnityEngine;

public class HeightMap
{
    private readonly float[,] heightMap;

    public HeightMap(float[,] heightMap)
    {
        this.heightMap = heightMap;
    }

    private static (float, float) IndexToCoord(float row, float column)
    {
        // THIS IS WHAT THESE VARIABLES ARE CALLED IN MUCK'S SOURCE CODE PLEASE DON'T BULLY ME GO BULLY DANI
        float num1 = (float) (MapGenerator.mapChunkSize - 1) / -2f;
        float num2 = (float) (MapGenerator.mapChunkSize - 1) / 2f;
        return ((num1 + row) * MapGenerator.worldScale, (num2 - column) * MapGenerator.worldScale);
    }
    
    private static (float, float) CoordToIndex(float x, float z)
    {
        float num1 = (float) (MapGenerator.mapChunkSize - 1) / -2f;
        float num2 = (float) (MapGenerator.mapChunkSize - 1) / 2f;
        return (x / MapGenerator.worldScale - num1, num2 - z / MapGenerator.worldScale);
    }

    public float CoordToHeight(float x, float z)
    {
        var (row, column) = CoordToIndex(x, z);
        return IndexToHeight(row, column);
    }

    public float IndexToHeight(float row, float column)
    {
        int rowIndex = Mathf.RoundToInt(row);
        int columnIndex = Mathf.RoundToInt(column);
        return heightMap[rowIndex, columnIndex];
    }
}
