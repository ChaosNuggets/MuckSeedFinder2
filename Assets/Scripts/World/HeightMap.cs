using System.Collections.Generic;
using UnityEngine;

public class HeightMap
{
    /* The world is divided into a grid of squares.
     * Each square has a diagonal line going from the top left to the bottom right,
     * splitting it into two right triangles. These triangles are
     * the planes that make up the land.
     */

    private readonly float[,] heightMap;
    public const int MAP_CHUNK_SIZE = 241;
    public const int WORLD_SCALE = 12;
    const float SAFETY_OFFSET = 0.01f; // The amount we move the ray origin in by

    public HeightMap(int seed)
    {
        const float HEIGHT_MULTIPLIER = 100f;
        const float NOISE_SCALE = 20f;
        const int OCTAVES = 4;
        const float PERSISTANCE = 0.1f;
        const float LACUNARITY = 6f;
        const float BLEND = 0.01f;
        const float BLEND_STRENGTH = 2f;
        Vector2 offset = new(0f, 0f);

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

    public float CoordToHeight(float x, float z)
    {
        var (row, column) = CoordToIndex(x, z);
        return IndexToHeight(row, column);
    }

    // This treats the heightmap as squares
    private float IndexToHeightFast(float row, float column)
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

    private float IndexToHeight(float row, float column)
    {
        Plane plane = GetIndexPlane(row, column);
        plane.Raycast(new Ray(new Vector3(column, 0f, row), Vector3.up), out float distance);
        return distance;
    }

    private Plane GetIndexPlane(float row, float column)
    {
        return GetIndexPlane(new Triangle(row, column));
    }

    private Plane GetIndexPlane(Triangle triangle)
    {
        var (topRightHeight, topLeftHeight, bottomLeftHeight, bottomRightHeight)
            = GetSquareHeights(triangle.topIndex, triangle.leftIndex, triangle.bottomIndex, triangle.rightIndex);

        Vector3 topLeft = new(triangle.leftIndex, topLeftHeight, triangle.topIndex);
        Vector3 bottomRight = new(triangle.rightIndex, bottomRightHeight, triangle.bottomIndex);
        Vector3 otherPoint = triangle.isTopTriangle
            ? new(triangle.rightIndex, topRightHeight, triangle.topIndex)
            : new(triangle.leftIndex, bottomLeftHeight, triangle.bottomIndex);

        return new Plane(topLeft, bottomRight, otherPoint);
    }

    public float GetAngle(float x, float z)
    {
        Plane plane = GetCoordPlane(x, z);
        float angle = Mathf.Abs(Vector3.Angle(Vector3.up, plane.normal));
        angle = angle > 90 ? 180 - angle : angle;
        return angle;
    }

    public Vector3 SphereCastDown(float x, float z, float radius)
    {
        Plane plane = GetCoordPlane(x, z);
        plane.Raycast(new Ray(new Vector3(x, 0f, z), Vector3.up), out float y);
        float offset = Mathf.Abs(plane.normal.magnitude / plane.normal.y) * radius; // Derived using cos cross product formula and trig

        Vector3 sphereCenter = new(x, y + offset, z);
        Vector3 vectorToHitPoint = plane.normal.y > 0 ? plane.normal.normalized * radius * -1 : plane.normal.normalized * radius;
        return sphereCenter + vectorToHitPoint;
    }

    private Plane GetCoordPlane(float x, float z)
    {
        // Get the heights
        var (row, column) = CoordToIndex(x, z);
        Triangle triangle = new(row, column);
        var (topRightHeight, topLeftHeight, bottomLeftHeight, bottomRightHeight)
            = GetSquareHeights(triangle.topIndex, triangle.leftIndex, triangle.bottomIndex, triangle.rightIndex);

        // Calculate 3 points on the plane
        Vector3 topLeft = IndexToCoord(new Vector3(triangle.leftIndex, topLeftHeight, triangle.topIndex));
        Vector3 bottomRight = IndexToCoord(new Vector3(triangle.rightIndex, bottomRightHeight, triangle.bottomIndex));
        Vector3 otherPoint = IndexToCoord(triangle.isTopTriangle
            ? new Vector3(triangle.rightIndex, topRightHeight, triangle.topIndex)
            : new Vector3(triangle.leftIndex, bottomLeftHeight, triangle.bottomIndex));

        // Create the plane and calculate the angle
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

    // If there is a hit, return true and set hitPoint to be where it hit
    // Otherwise, return false and set hitPoint to IndexToCoord(new Vector3(0, 0, 0))
    // This will return false if origin is outside the height map.
    public bool CoordRaycast(Vector3 origin, Vector3 pointAlongRay, out Vector3 hitPoint)
    {
        // Convert ray to index units
        origin = CoordToIndex(origin);
        pointAlongRay = CoordToIndex(pointAlongRay);

        // Do magic
        bool didHit = IndexRaycast(new Ray(origin, pointAlongRay - origin), out hitPoint);

        // Convert back to coord units
        hitPoint = IndexToCoord(hitPoint);
        return didHit;
    }

    private bool IndexRaycast(Ray ray, out Vector3 hitPoint)
    {
        // Row is z, column is x
        // Start at ray origin
        Triangle currentTriangle = CreateInBoundsTriangle(ref ray);

        SuperiorRay2D ray2D = new(ray);
        //const int NUM_TRIANGLES_INTERSECTED_ON_DIAGONAL = 4;
        //HashSet<Triangle> testedTriangles = new((MAP_CHUNK_SIZE - 1) / 2 * NUM_TRIANGLES_INTERSECTED_ON_DIAGONAL);
        HashSet<Triangle> testedTriangles = new();

        while (IsInBounds(currentTriangle))
        {
            // Get the current plane and see if it intersects
            Plane currentPlane = GetIndexPlane(currentTriangle);
            bool didHit = currentPlane.Raycast(ray, out float distance);
            hitPoint = ray.GetPoint(distance);

            // If it does intersect, see if what plane the point is a part of. If it's part of the same plane, return that point.
            Triangle hitTriangle = new(hitPoint.z, hitPoint.x);
            if (didHit && hitTriangle == currentTriangle)
            {
                return true;
            }

            // If it doesn't intersect or if the intersection point is on a different plane, move to the next plane.
            testedTriangles.Add(currentTriangle);
            currentTriangle = CalculateNextTriangle(currentTriangle, ray2D, testedTriangles);
        }

        hitPoint = Vector3.zero;
        return false;
    }

    private bool IsInBounds(Triangle triangle)
    {
        return 0 <= triangle.topIndex && triangle.bottomIndex < heightMap.GetLength(0)
            && 0 <= triangle.leftIndex && triangle.rightIndex < heightMap.GetLength(1);
    }

    private Triangle CreateInBoundsTriangle(ref Ray ray)
    {
        float distance = 0;
        Triangle triangle = new(ray.origin.z, ray.origin.x);

        if (triangle.topIndex < 0)
        {
            // ray.origin.z + ray.direction.z * distance = SAFETY_OFFSET
            distance = (SAFETY_OFFSET - ray.origin.z) / ray.direction.z;
        }
        else if (triangle.bottomIndex >= heightMap.GetLength(0))
        {
            // ray.origin.z + ray.direction.z * distance = heightMap.GetLength(0) - 1 - SAFETY_OFFSET
            distance = (heightMap.GetLength(0) - 1 - SAFETY_OFFSET - ray.origin.z) / ray.direction.z;
        }
        else if (triangle.leftIndex < 0)
        {
            // ray.origin.x + ray.direction.x * distance = SAFETY_OFFSET
            distance = (SAFETY_OFFSET - ray.origin.x) / ray.direction.x;
        }
        else if (triangle.rightIndex >= heightMap.GetLength(1))
        {
            // ray.origin.x + ray.direction.x * distance = heightMap.GetLength(1) - 1 - SAFETY_OFFSET
            distance = (heightMap.GetLength(1) - 1 - SAFETY_OFFSET - ray.origin.x) / ray.direction.x;
        }

        ray.origin = ray.GetPoint(distance);
        return new Triangle(ray.origin.z, ray.origin.x);
    }

    private Triangle CalculateNextTriangle(Triangle currentTriangle, SuperiorRay2D ray2D, HashSet<Triangle> testedTriangles)
    {
        // Create the lines
        var (topLeft, bottomRight, otherPoint) = currentTriangle.GetVertices();

        // Test each edge of the triangle until we find an edge which the ray intersects with.
        // If the ray intersects with the left, move left. If the ray intersects with the bottom, move down, etc.
        // the testedTriangles set helps prevent it from retesting the same triangle

        LineSegment topOrLeft = new(topLeft, otherPoint);
        if (LineSegment.DoesIntersect(topOrLeft, ray2D))
        {
            // If intersecting with top
            if (currentTriangle.isTopTriangle)
            {
                Triangle potentialNextTriangle = new(
                    currentTriangle.topIndex - 1,
                    currentTriangle.leftIndex,
                    false
                );

                if (!testedTriangles.Contains(potentialNextTriangle))
                {
                    return potentialNextTriangle;
                }
            }
            else
            {
                // If intersecting with left
                Triangle potentialNextTriangle = new(
                    currentTriangle.topIndex,
                    currentTriangle.leftIndex - 1,
                    true
                );

                if (!testedTriangles.Contains(potentialNextTriangle))
                {
                    return potentialNextTriangle;
                }
            }
        }

        LineSegment rightOrBottom = new(bottomRight, otherPoint);
        if (LineSegment.DoesIntersect(rightOrBottom, ray2D))
        {
            // If intersecting with right
            if (currentTriangle.isTopTriangle)
            {
                Triangle potentialNextTriangle = new(
                    currentTriangle.topIndex,
                    currentTriangle.leftIndex + 1,
                    false
                );

                if (!testedTriangles.Contains(potentialNextTriangle))
                {
                    return potentialNextTriangle;
                }
            }
            else
            {
                // If intersecting with bottom
                Triangle potentialNextTriangle = new(
                    currentTriangle.topIndex + 1,
                    currentTriangle.leftIndex,
                    true
                );

                if (!testedTriangles.Contains(potentialNextTriangle))
                {
                    return potentialNextTriangle;
                }
            }
        }

        // If it intersects with the diagonal line
        return new Triangle(
            currentTriangle.topIndex,
            currentTriangle.leftIndex,
            !currentTriangle.isTopTriangle
        );
    }
}
