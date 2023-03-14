using UnityEngine;

public struct Triangle
{
    public readonly int topIndex, leftIndex, bottomIndex, rightIndex;
    public readonly bool isTopTriangle;

    // Mesh of land is boxes with a diagonal line going down and right separating each box into 2 triangles
    public Triangle(float row, float column)
    {
        (topIndex, leftIndex, bottomIndex, rightIndex)
            = GetSquare(row, column);
        isTopTriangle = row - topIndex < column - leftIndex; // Basically if y is greater than x (remember y goes down), it's the left triangle, and vice versa
    }

    // ya like boilerplate code?
    public Triangle(int topIndex, int leftIndex, int bottomIndex, int rightIndex, bool isTopTriangle)
    {
        this.topIndex = topIndex;
        this.leftIndex = leftIndex;
        this.bottomIndex = bottomIndex;
        this.rightIndex = rightIndex;
        this.isTopTriangle = isTopTriangle;
    }

    public static (int, int, int, int) GetSquare(float row, float column)
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

    public (Vector2, Vector2, Vector2) GetVertices()
    {
        Vector2 topLeft = new Vector2(leftIndex, topIndex);
        Vector2 bottomRight = new Vector2(rightIndex, bottomIndex);
        Vector2 otherPoint = isTopTriangle
            ? new Vector2(rightIndex, topIndex)
            : new Vector2(leftIndex, bottomIndex);
        return (topLeft, bottomRight, otherPoint);
    }

    public static bool operator ==(Triangle t1, Triangle t2)
    {
        return t1.topIndex == t2.topIndex
            && t1.leftIndex == t2.leftIndex
            && t1.bottomIndex == t2.bottomIndex
            && t1.rightIndex == t2.rightIndex
            && t1.isTopTriangle == t2.isTopTriangle;
    }
    
    public static bool operator !=(Triangle t1, Triangle t2)
    {
        return !(t1 == t2);
    }
}
