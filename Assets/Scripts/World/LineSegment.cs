using UnityEngine;

public struct LineSegment
{
    public readonly Vector2 point1, point2;

    public LineSegment(Vector2 point1, Vector2 point2)
    {
        this.point1 = point1;
        this.point2 = point2;
    }

    private static bool onLine(LineSegment line, Vector2 point)
    {
        // Check whether point is on the line or not
        return (point.x <= Mathf.Max(line.point1.x, line.point2.x)
            && point.x >= Mathf.Min(line.point1.x, line.point2.x))
            && (point.y <= Mathf.Max(line.point1.y, line.point2.y)
            && point.y >= Mathf.Min(line.point1.y, line.point2.y));
    }

    private static int calculateDirection(Vector2 a, Vector2 b, Vector2 c)
    {
        double cross = (b.y - a.y) * (c.x - b.x)
            - (b.x - a.x) * (c.y - b.y); // Computes the cross product between vectors BC and AB

        if (cross == 0)
            return 0; // Colinear

        if (cross < 0)
            return 2; // Counterclockwise direction

        return 1; // Clockwise direction
    }

    private static bool doesIntersect(LineSegment line1, LineSegment line2)
    {
        // Four direction for two lines and points of other line
        int dir1 = calculateDirection(line1.point1, line1.point2, line2.point1);
        int dir2 = calculateDirection(line1.point1, line1.point2, line2.point2);
        int dir3 = calculateDirection(line2.point1, line2.point2, line1.point1);
        int dir4 = calculateDirection(line2.point1, line2.point2, line1.point2);

        // When intersecting
        if (dir1 != dir2 && dir3 != dir4)
            return true;

        // When point2 of line2 are on the line1
        if (dir1 == 0 && onLine(line1, line2.point1))
            return true;

        // When point1 of line2 are on the line1
        if (dir2 == 0 && onLine(line1, line2.point2))
            return true;

        // When point2 of line1 are on the line2
        if (dir3 == 0 && onLine(line2, line1.point1))
            return true;

        // When point1 of line1 are on the line2
        if (dir4 == 0 && onLine(line2, line1.point2))
            return true;

        return false;
    }

    public static bool doesIntersect(LineSegment line, Ray ray)
    {
        Vector2 farPoint = VectorConvert.ToVector2(ray.GetPoint(100));
        return doesIntersect(line, new LineSegment(VectorConvert.ToVector2(ray.origin), farPoint));
    }
}