using UnityEngine;

public struct Ray2D
{
    public readonly Vector2 origin;
    public readonly Vector2 direction;

    public Ray2D(Ray ray)
    {
        origin = VectorConvert.ToVector2(ray.origin);
        direction = VectorConvert.ToVector2(ray.direction).normalized;
    }

    public Vector2 GetPoint(float distance)
    {
        return origin + direction * distance;
    }
}
