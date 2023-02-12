using UnityEngine;

public class HeightMap
{
    private readonly float[,] heightMap;
    public const int MAP_CHUNK_SIZE = 241;
    public const int WORLD_SCALE = 12;

    public HeightMap(int seed)
    {
        const float HEIGHT_MULTIPLIER = 100f;
        const float NOISE_SCALE = 20;
        const int OCTAVES = 4;
        const float PERSISTANCE = 0.1f;
        const float LACUNARITY = 6;
        const float BLEND = 0.01f;
        const float BLEND_STRENGTH = 2;
        Vector2 offset = new Vector2(0, 0);

        heightMap = Noise.GenerateNoiseMap(MAP_CHUNK_SIZE, MAP_CHUNK_SIZE, seed, NOISE_SCALE, OCTAVES, PERSISTANCE, LACUNARITY, BLEND, BLEND_STRENGTH, offset);
        float[,] falloffMap = FalloffGenerator.GenerateFalloffMap(MAP_CHUNK_SIZE);
        for (int i = 0; i < MAP_CHUNK_SIZE; i++)
        {
            for (int j = 0; j < MAP_CHUNK_SIZE; j++)
            {
                heightMap[j, i] = Mathf.Clamp(heightMap[j, i] - falloffMap[j, i], 0f, 1f);
                heightMap[j, i] = HeightCurve.Evaluate(heightMap[j, i]) * HEIGHT_MULTIPLIER;
            }
        }
    }

    public HeightMap(float[,] heightMap)
    {
        this.heightMap = heightMap;
    }
    
    private static (float, float) IndexToCoord(float row, float column)
    {
        // THIS IS WHAT THESE VARIABLES ARE CALLED IN MUCK'S SOURCE CODE PLEASE DON'T BULLY ME GO BULLY DANI
        float num1 = (MAP_CHUNK_SIZE - 1) / -2f;
        float num2 = (MAP_CHUNK_SIZE - 1) / 2f;
        return ((num1 + row) * WORLD_SCALE, (num2 - column) * WORLD_SCALE);
    }

    private static (float, float) CoordToIndex(float x, float z)
    {
        float num1 = (MAP_CHUNK_SIZE - 1) / -2f;
        float num2 = (MAP_CHUNK_SIZE - 1) / 2f;
        return (x / WORLD_SCALE - num1, num2 - z / WORLD_SCALE);
    }

    public float CoordToHeightFast(float x, float z)
    {
        var (row, column) = CoordToIndex(x, z);
        return IndexToHeightFast(row, column);
    }

    public float CoordToHeightPrecise(float x, float z)
    {
        var (row, column) = CoordToIndex(x, z);
        return IndexToHeightPrecise(row, column);
    }

    // This treats the heightmap as squares
    public float IndexToHeightFast(float row, float column)
    {
        int topIndex = Mathf.FloorToInt(row);
        int leftIndex = Mathf.FloorToInt(column);
        int bottomIndex = Mathf.CeilToInt(row);
        int rightIndex = Mathf.CeilToInt(column);

        float topLeftHeight = heightMap[topIndex, leftIndex];
        float topRightHeight = heightMap[topIndex, rightIndex];
        float bottomRightHeight = heightMap[bottomIndex, rightIndex];
        float bottomLeftHeight = heightMap[bottomIndex, leftIndex];

        float horizontalT = column - leftIndex;
        float verticalT = row - topIndex;

        float topHeight = Mathf.Lerp(topLeftHeight, topRightHeight, horizontalT);
        float bottomHeight = Mathf.Lerp(bottomLeftHeight, bottomRightHeight, horizontalT);

        return Mathf.Lerp(topHeight, bottomHeight, verticalT);
    }

    // This is a backup that does all the plane math manually
    //public float IndexToHeightPrecise(float row, float column)
    //{
    //    // DON'T GET CONFUSED, ROW GOES DOWN. SO BOTTOM_INDEX > TOP_INDEX
    //    int topIndex = Mathf.FloorToInt(row);
    //    int leftIndex = Mathf.FloorToInt(column);
    //    int bottomIndex = Mathf.CeilToInt(row);
    //    int rightIndex = Mathf.CeilToInt(column);

    //    float topLeftHeight = heightMap[topIndex, leftIndex];
    //    float topRightHeight = heightMap[topIndex, rightIndex];
    //    float bottomRightHeight = heightMap[bottomIndex, rightIndex];
    //    float bottomLeftHeight = heightMap[bottomIndex, leftIndex];

    //    Vector3 topLeft = new Vector3(leftIndex, topLeftHeight, topIndex);
    //    Vector3 bottomRight = new Vector3(rightIndex, bottomRightHeight, bottomIndex);
    //    Vector3 otherPoint = row - topIndex > column - leftIndex // Test which triangle the index is in
    //        ? new Vector3(rightIndex, bottomLeftHeight, topIndex)
    //        : new Vector3(leftIndex, topRightHeight, bottomIndex);

    //    Vector3 vec1 = topLeft - otherPoint;
    //    Vector3 vec2 = bottomRight - otherPoint;
    //    Vector3 normal = Vector3.Cross(vec1, vec2);

    //    float d = Vector3.Dot(normal, topLeft); // d when the plane is in the form ax + by + cz = d
    //    return (d - normal.x * column - normal.z * row) / normal.y;
    //}

    // This treats the heightmap as triangles
    public float IndexToHeightPrecise(float row, float column)
    {
        // DON'T GET CONFUSED, ROW GOES DOWN. SO BOTTOM_INDEX > TOP_INDEX
        int topIndex = Mathf.FloorToInt(row);
        int leftIndex = Mathf.FloorToInt(column);
        int bottomIndex = Mathf.CeilToInt(row);
        int rightIndex = Mathf.CeilToInt(column);

        float topLeftHeight = heightMap[topIndex, leftIndex];
        float topRightHeight = heightMap[topIndex, rightIndex];
        float bottomRightHeight = heightMap[bottomIndex, rightIndex];
        float bottomLeftHeight = heightMap[bottomIndex, leftIndex];

        Vector3 topLeft = new Vector3(leftIndex, topLeftHeight, topIndex);
        Vector3 bottomRight = new Vector3(rightIndex, bottomRightHeight, bottomIndex);
        Vector3 otherPoint = row - topIndex > column - leftIndex // Test which triangle the index is in
            ? new Vector3(leftIndex, bottomLeftHeight, bottomIndex)
            : new Vector3(rightIndex, topRightHeight, topIndex);

        Plane plane = new Plane(topLeft, bottomRight, otherPoint);
        plane.Raycast(new Ray(new Vector3(column, 0, row), Vector3.up), out float distance);
        return distance;
    }
}
