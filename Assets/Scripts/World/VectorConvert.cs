using UnityEngine;

public static class VectorConvert
{
    public static Vector2 ToVector2(Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }

    public static Vector3 ToVector3(Vector2 vec)
    {
        return new Vector3(vec.x, 0, vec.y);
    }
}
