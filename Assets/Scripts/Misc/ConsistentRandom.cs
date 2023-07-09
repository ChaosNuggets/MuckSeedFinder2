using System;

// Token: 0x020000E2 RID: 226
public class ConsistentRandom : Random
{
	// Token: 0x060006E9 RID: 1769 RVA: 0x0002491F File Offset: 0x00022B1F
	public ConsistentRandom() : this(Environment.TickCount)
	{
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x0002492C File Offset: 0x00022B2C
	public ConsistentRandom(int seed)
	{
		int num = (seed == int.MinValue) ? int.MaxValue : Math.Abs(seed);
		int num2 = 161803398 - num;
		SeedArray[55] = num2;
		int num3 = 1;
		for (int i = 1; i < 55; i++)
		{
			int num4 = 21 * i % 55;
			SeedArray[num4] = num3;
			num3 = num2 - num3;
			if (num3 < 0)
			{
				num3 += int.MaxValue;
			}
			num2 = SeedArray[num4];
		}
		for (int j = 1; j < 5; j++)
		{
			for (int k = 1; k < 56; k++)
			{
				SeedArray[k] -= SeedArray[1 + (k + 30) % 55];
				if (SeedArray[k] < 0)
				{
					SeedArray[k] += int.MaxValue;
				}
			}
		}
		inext = 0;
		inextp = 21;
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x00024A26 File Offset: 0x00022C26
	protected override double Sample()
	{
		return InternalSample() * 4.656612875245797E-10;
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x00024A3C File Offset: 0x00022C3C
	private int InternalSample()
	{
		int num = inext;
		int num2 = inextp;
		if (++num >= 56)
		{
			num = 1;
		}
		if (++num2 >= 56)
		{
			num2 = 1;
		}
		int num3 = SeedArray[num] - SeedArray[num2];
		if (num3 == 2147483647)
		{
			num3--;
		}
		if (num3 < 0)
		{
			num3 += int.MaxValue;
		}
		SeedArray[num] = num3;
		inext = num;
		inextp = num2;
		return num3;
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x00024AAF File Offset: 0x00022CAF
	public override int Next()
	{
		return InternalSample();
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x00024AB8 File Offset: 0x00022CB8
	private double GetSampleForLargeRange()
	{
		int num = InternalSample();
		if (InternalSample() % 2 == 0)
		{
			num = -num;
		}
		return (num + 2147483646.0) / 4294967293.0;
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x00024AF8 File Offset: 0x00022CF8
	public override int Next(int minValue, int maxValue)
	{
		if (minValue > maxValue)
		{
			throw new ArgumentOutOfRangeException("minValue");
		}
		long num = (long)maxValue - minValue;
		if (num <= 2147483647L)
		{
			return (int)(Sample() * num) + minValue;
		}
		return (int)((long)(GetSampleForLargeRange() * num) + minValue);
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x00024B40 File Offset: 0x00022D40
	public override void NextBytes(byte[] buffer)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		for (int i = 0; i < buffer.Length; i++)
		{
			buffer[i] = (byte)(InternalSample() % 256);
		}
	}

	// Token: 0x04000629 RID: 1577
	private int inext;

	// Token: 0x0400062A RID: 1578
	private int inextp;

	// Token: 0x0400062B RID: 1579
	private readonly int[] SeedArray = new int[56];
}

// Written by Dani
