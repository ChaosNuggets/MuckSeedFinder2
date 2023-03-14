using UnityEngine;

public class HeightMap
{
    private readonly float[,] heightMap;
    public const int MAP_CHUNK_SIZE = 241;
    public const int WORLD_SCALE = 12;

    public HeightMap(int seed)
    {
        const float HEIGHT_MULTIPLIER = 100f;
        const float NOISE_SCALE = 20f;
        const int OCTAVES = 4;
        const float PERSISTANCE = 0.1f;
        const float LACUNARITY = 6f;
        const float BLEND = 0.01f;
        const float BLEND_STRENGTH = 2f;
        Vector2 offset = new Vector2(0f, 0f);

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

    private static Vector3 IndexToCoord(Vector3 index)
    {
        var (x, z) = IndexToCoord(index.z, index.x);
        return new Vector3(x, index.y, z);
    }

    private static (float, float) CoordToIndex(float x, float z)
    {
        float num1 = (MAP_CHUNK_SIZE - 1) / -2f;
        float num2 = (MAP_CHUNK_SIZE - 1) / 2f;
        return (x / WORLD_SCALE - num1, num2 - z / WORLD_SCALE);
    }

    private static Vector3 CoordToIndex(Vector3 coord)
    {
        var (row, column) = CoordToIndex(coord.x, coord.z);
        return new Vector3(column, coord.y, row);
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
        var (topIndex, leftIndex, bottomIndex, rightIndex)
            = GetSquare(row, column);
        var (topRightHeight, topLeftHeight, bottomLeftHeight, bottomRightHeight)
            = GetSquareHeights(topIndex, leftIndex, bottomIndex, rightIndex);

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
        Plane plane = GetPlane(row, column);
        plane.Raycast(new Ray(new Vector3(column, 0f, row), Vector3.up), out float distance);
        return distance;
    }

    private Plane GetPlane(float row, float column)
    {
        Triangle triangle = GetTriangle(row, column);
        return GetPlane(row, column, triangle);
    }
    
    private Plane GetPlane(float row, float column, Triangle triangle)
    {
        var (topRightHeight, topLeftHeight, bottomLeftHeight, bottomRightHeight)
            = GetSquareHeights(triangle.topIndex, triangle.leftIndex, triangle.bottomIndex, triangle.rightIndex);
        
        Vector3 topLeft = new Vector3(triangle.leftIndex, topLeftHeight, triangle.topIndex);
        Vector3 bottomRight = new Vector3(triangle.rightIndex, bottomRightHeight, triangle.bottomIndex);
        Vector3 otherPoint = triangle.isTopTriangle
            ? new Vector3(triangle.rightIndex, topRightHeight, triangle.topIndex)
            : new Vector3(triangle.leftIndex, bottomLeftHeight, triangle.bottomIndex);

        return new Plane(topLeft, bottomRight, otherPoint);
    }

    private static Triangle GetTriangle(float row, float column)
    {
        Triangle triangle;
        (triangle.topIndex, triangle.leftIndex, triangle.bottomIndex, triangle.rightIndex)
            = GetSquare(row, column);
        triangle.isTopTriangle = row - triangle.topIndex < column - triangle.leftIndex;

        return triangle;
    }

    private static (int, int, int, int) GetSquare(float row, float column)
    {
        // DON'T GET CONFUSED, ROW GOES DOWN. SO BOTTOM_INDEX > TOP_INDEX
        int topIndex = Mathf.FloorToInt(row);
        int leftIndex = Mathf.FloorToInt(column);
        int bottomIndex = Mathf.CeilToInt(row);
        int rightIndex = Mathf.CeilToInt(column);

        if (bottomIndex == topIndex) bottomIndex++;
        if (rightIndex == leftIndex) rightIndex++;
        
        return (topIndex, leftIndex, bottomIndex, rightIndex);
    }

    private (float, float, float, float) GetSquareHeights(int topIndex, int leftIndex, int bottomIndex, int rightIndex)
    {
        float topRightHeight = heightMap[topIndex, rightIndex];
        float topLeftHeight = heightMap[topIndex, leftIndex];
        float bottomLeftHeight = heightMap[bottomIndex, leftIndex];
        float bottomRightHeight = heightMap[bottomIndex, rightIndex];

        return (topRightHeight, topLeftHeight, bottomLeftHeight, bottomRightHeight);
    }

    // If ray enters the surface, return point where it hits
    // Otherwise, return null (if it goes out of the bounds of the array)
    public Vector3 CoordRaycast(Ray ray)
    {
        // Convert ray to index units
        ray.origin = CoordToIndex(ray.origin);
        ray.direction = CoordToIndex(ray.direction);

        return IndexRaycast(ray);
    }

    public Vector3 IndexRaycast(Ray ray)
    {
        // Row is z, column is x
        // Start at ray origin. Get the current plane and see if it intersects
        Triangle currentTriangle = GetTriangle(ray.origin.z, ray.origin.x);
        Plane currentPlane = GetPlane(ray.origin.z, ray.origin.x, currentTriangle);
        currentPlane.Raycast(ray, out float distance);
        Vector3 hitPoint = ray.GetPoint(distance);
        // If it does intersect, see if what plane the point is a part of. If it's part of the same plane, return that point.
        // If it doesn't intersect or if the intersection point is on a different plane, move to the next plane.

        return Vector3.zero;
    }
}
