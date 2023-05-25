using System;
using UnityEngine;

public readonly struct Triangle : IEquatable<Triangle>
{
    public readonly int topIndex, leftIndex;
    public int bottomIndex
    { 
        get
        {
            return topIndex + 1;
        }
    }
    public int rightIndex
    {
        get
        {
            return leftIndex + 1;
        }
    }

    public readonly bool isTopTriangle;

    // Mesh of land is boxes with a diagonal line going down and right separating each box into 2 triangles
    public Triangle(float row, float column)
    {
        (topIndex, leftIndex, _, _)
            = GetSquare(row, column);
        isTopTriangle = row - topIndex < column - leftIndex; // Basically if y is less than x (remember y goes down), it's the top triangle, and vice versa
    }

    // ya like boilerplate code?
    public Triangle(int topIndex, int leftIndex, bool isTopTriangle)
    {
        this.topIndex = topIndex;
        this.leftIndex = leftIndex;
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
        Vector2 topLeft = new(leftIndex, topIndex);
        Vector2 bottomRight = new(rightIndex, bottomIndex);
        Vector2 otherPoint = isTopTriangle
            ? new(rightIndex, topIndex)
            : new(leftIndex, bottomIndex);
        return (topLeft, bottomRight, otherPoint);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(topIndex, leftIndex, bottomIndex, rightIndex, isTopTriangle);
    }

    public override bool Equals(object obj)
    {
        return obj is Triangle triangle && Equals(triangle);
    }

    public bool Equals(Triangle other)
    {
        return topIndex == other.topIndex &&
               leftIndex == other.leftIndex &&
               bottomIndex == other.bottomIndex &&
               rightIndex == other.rightIndex &&
               isTopTriangle == other.isTopTriangle;
    }

    public static bool operator ==(Triangle left, Triangle right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Triangle left, Triangle right)
    {
        return !(left == right);
    }
}
