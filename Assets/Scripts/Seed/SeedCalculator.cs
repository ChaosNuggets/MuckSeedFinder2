public abstract class SeedCalculator
{
    private enum Mode
    {
        One,
        Increment
    }
    private Mode currentMode = Mode.One;

    private int lastPotentialSeed;
    public int currentSeed { get; protected set; }
    private bool hasFoundPotentialSeed = false;
    protected readonly int[] increments;
    private int incrementIndex = -1;

    protected SeedCalculator(int startSeed, int[] increments)
    {
        currentSeed = startSeed;
        this.increments = increments;
    }

    protected abstract (bool, bool) IsGoodSeed(int seed);

    public int[] CalculateNextSeeds(int seedsToFind)
    {
        int[] seeds = new int[seedsToFind];
        for (int i = 0; i < seedsToFind; i++)
        {
            seeds[i] = CalculateNextSeed();
        }
        return seeds;
    }

    public int CalculateNextSeed()
    {
        int seed = 69420;
        bool hasFoundSeed = false;

        while (!hasFoundSeed)
        {
            (bool isPotentialSeed, bool isActualSeed) = IsGoodSeed(currentSeed);
            if (isPotentialSeed)
            {
                lastPotentialSeed = currentSeed;
                hasFoundPotentialSeed = true;
                currentMode = Mode.Increment;
                if (isActualSeed)
                {
                    seed = currentSeed;
                    hasFoundSeed = true;
                }
            }
            else
            {
                hasFoundPotentialSeed = false;
            }
            IncrementSeed();
        }

        return seed;
    }

    private void IncrementSeed()
    {
        if (currentMode == Mode.Increment)
        {
            if (!hasFoundPotentialSeed && incrementIndex >= increments.Length - 1)
            {
                currentSeed = lastPotentialSeed;
                currentMode = Mode.One; // Demote the mode
            }
            else
            {
                incrementIndex = hasFoundPotentialSeed ? 0 : incrementIndex + 1;
                currentSeed = lastPotentialSeed + increments[incrementIndex];
                return;
            }
        }

        // Code should only reach this point if the currentMode = Mode.One
        currentSeed++;
    }
}
