// Copied and pasted from muck source code
using System;

public class ConsistentRandom : Random
{
    private const int MBIG = 2147483647;
    private const int MSEED = 161803398;
    private const int MZ = 0;
    private int inext;
    private int inextp;
    private int[] seedArray = new int[56];

    public ConsistentRandom()
        : this(Environment.TickCount)
    {
    }

    public ConsistentRandom(int seed)
    {
        int num1 = 161803398 - (seed == int.MinValue ? int.MaxValue : Math.Abs(seed));
        this.seedArray[55] = num1;
        int num2 = 1;
        for (int index1 = 1; index1 < 55; ++index1)
        {
            int index2 = 21 * index1 % 55;
            this.seedArray[index2] = num2;
            num2 = num1 - num2;
            if (num2 < 0)
                num2 += int.MaxValue;
            num1 = this.seedArray[index2];
        }
        for (int index3 = 1; index3 < 5; ++index3)
        {
            for (int index4 = 1; index4 < 56; ++index4)
            {
                this.seedArray[index4] -= this.seedArray[1 + (index4 + 30) % 55];
                if (this.seedArray[index4] < 0)
                    this.seedArray[index4] += int.MaxValue;
            }
        }
        this.inext = 0;
        this.inextp = 21;
    }

    protected override double Sample() => (double)this.InternalSample() * 4.6566128752457969E-10;

    private int InternalSample()
    {
        int inext = this.inext;
        int inextp = this.inextp;
        int index1;
        if ((index1 = inext + 1) >= 56)
            index1 = 1;
        int index2;
        if ((index2 = inextp + 1) >= 56)
            index2 = 1;
        int num = this.seedArray[index1] - this.seedArray[index2];
        if (num == int.MaxValue)
            --num;
        if (num < 0)
            num += int.MaxValue;
        this.seedArray[index1] = num;
        this.inext = index1;
        this.inextp = index2;
        return num;
    }

    public override int Next() => this.InternalSample();

    private double GetSampleForLargeRange()
    {
        int num = this.InternalSample();
        if ((this.InternalSample() % 2 == 0 ? 1 : 0) != 0)
            num = -num;
        return ((double)num + 2147483646.0) / 4294967293.0;
    }

    public override int Next(int minValue, int maxValue)
    {
        if (minValue > maxValue)
            throw new ArgumentOutOfRangeException(nameof(minValue));
        long num = (long)maxValue - (long)minValue;
        return num <= (long)int.MaxValue ? (int)(this.Sample() * (double)num) + minValue : (int)((long)(this.GetSampleForLargeRange() * (double)num) + (long)minValue);
    }

    public override void NextBytes(byte[] buffer)
    {
        if (buffer == null)
            throw new ArgumentNullException(nameof(buffer));
        for (int index = 0; index < buffer.Length; ++index)
            buffer[index] = (byte)(this.InternalSample() % 256);
    }
}

