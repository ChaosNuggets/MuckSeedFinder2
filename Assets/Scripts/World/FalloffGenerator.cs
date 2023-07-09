using UnityEngine;

// Token: 0x02000104 RID: 260
public static class FalloffGenerator
{
	// Token: 0x060007DF RID: 2015 RVA: 0x0002920C File Offset: 0x0002740C
	public static float[,] GenerateFalloffMap(int size)
	{
		float[,] array = new float[size, size];
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				float f = (float)i / size * 2f - 1f;
				float f2 = (float)j / size * 2f - 1f;
				float value = Mathf.Max(Mathf.Abs(f), Mathf.Abs(f2));
				array[i, j] = Evaluate(value);
			}
		}
		return array;
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00029280 File Offset: 0x00027480
	private static float Evaluate(float value)
	{
		float p = 3f;
		float num = 2.2f;
		return Mathf.Pow(value, p) / (Mathf.Pow(value, p) + Mathf.Pow(num - num * value, p));
	}
}

// Written by Dani
