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
            = Triangle.GetSquare(row, column);
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
        return GetPlane(new Triangle(row, column));
    }

    private Plane GetPlane(Triangle triangle)
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
    public bool CoordRaycast(Ray ray, out Vector3 hitPoint)
    {
        // Convert ray to index units
        ray.origin = CoordToIndex(ray.origin);
        ray.direction = CoordToIndex(ray.direction);

        return IndexRaycast(ray, out hitPoint);
    }

    public bool IndexRaycast(Ray ray, out Vector3 hitPoint)
    {
        // Row is z, column is x
        // Start at ray origin
        Triangle currentTriangle = new Triangle(ray.origin.z, ray.origin.x);

        while (true) // TODO: handle out of bounds of array stuff 
        {
            // Get the current plane and see if it intersects
            Plane currentPlane = GetPlane(currentTriangle);
            bool didHit = currentPlane.Raycast(ray, out float distance);
            hitPoint = ray.GetPoint(distance);

            // If it does intersect, see if what plane the point is a part of. If it's part of the same plane, return that point.
            Triangle hitTriangle = new Triangle(hitPoint.z, hitPoint.x);
            if (didHit && hitTriangle == currentTriangle)
            {
                return true;
            }
            // If it doesn't intersect or if the intersection point is on a different plane, move to the next plane.
            currentTriangle = CalculateNextTriangle(currentTriangle, ray);
        }

        return false;
    }

    private Triangle CalculateNextTriangle(Triangle currentTriangle, Ray ray)
    {
        // Create the lines
        var (topLeft, bottomRight, otherPoint) = currentTriangle.GetVertices();

        // Test each edge of the triangle until we find an edge which the ray intersects with.
        // If the ray intersects with the left, move left. If the ray intersects with the bottom, move down, etc.
        LineSegment topOrLeft = new LineSegment(topLeft, otherPoint);
        if (LineSegment.doesIntersect(topOrLeft, ray))
        {
            if (currentTriangle.isTopTriangle) // This would be testing top
            {
                return new Triangle(
                    currentTriangle.topIndex - 1,
                    currentTriangle.leftIndex,
                    currentTriangle.bottomIndex - 1,
                    currentTriangle.rightIndex,
                    false
                );
            }
            // This would be testing left
            return new Triangle(
                currentTriangle.topIndex,
                currentTriangle.leftIndex - 1,
                currentTriangle.bottomIndex,
                currentTriangle.rightIndex - 1,
                true
            );
        }

        LineSegment rightOrBottom = new LineSegment(bottomRight, otherPoint);
        if (LineSegment.doesIntersect(rightOrBottom, ray))
        {
            if (currentTriangle.isTopTriangle) // This would be testing right
            {
                return new Triangle(
                    currentTriangle.topIndex,
                    currentTriangle.leftIndex + 1,
                    currentTriangle.bottomIndex,
                    currentTriangle.rightIndex + 1,
                    false
                );
            }
            // This would be testing bottom
            return new Triangle(
                currentTriangle.topIndex + 1,
                currentTriangle.leftIndex,
                currentTriangle.bottomIndex + 1,
                currentTriangle.rightIndex,
                true
            );
        }

        // If it intersects with the diagonal line
        return new Triangle(
            currentTriangle.topIndex,
            currentTriangle.leftIndex,
            currentTriangle.bottomIndex,
            currentTriangle.rightIndex,
            !currentTriangle.isTopTriangle
        );
    }
}
