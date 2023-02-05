using UnityEngine;

public static class FalloffGenerator
{
    public static float[,] GenerateFalloffMap(int size)
    {
        float[,] falloffMap = new float[size, size];
        for (int index1 = 0; index1 < size; ++index1)
        {
            for (int index2 = 0; index2 < size; ++index2)
            {
                double f1 = (double) index1 / (double) size * 2.0 - 1.0;
                float f2 = (float) ((double) index2 / (double) size * 2.0 - 1.0);
                float num = Mathf.Max(Mathf.Abs((float) f1), Mathf.Abs(f2));
                falloffMap[index1, index2] = FalloffGenerator.Evaluate(num);
            }
        }
        return falloffMap;
    }

    private static float Evaluate(float value)
    {
        float p = 3f;
        float num = 2.2f;
        return Mathf.Pow(value, p) / (Mathf.Pow(value, p) + Mathf.Pow(num - num * value, p));
    }
}

