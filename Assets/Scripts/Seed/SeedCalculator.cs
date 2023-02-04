public class SeedCalculator
{
    private enum Mode
    {
        One,
        God
    }
    private Mode currentMode = Mode.One;

    private int lastPotentialGodSeed;
    private int currentSeed;
    private bool hasFoundPotentialGod = false;
    private readonly int[] increments =
    {
            19,
            1204, 1223, 1242, 1261, 1280,
            7745, 7764,
            9025, 9044
        };
    private int incrementIndex = -1;

    public SeedCalculator(int startSeed)
    {
        currentSeed = startSeed;
    }

    public int[] CalculateNextGodSeeds(int seedsToFind)
    {
        int[] godSeeds = new int[seedsToFind];
        for (int i = 0; i < seedsToFind; i++)
        {
            godSeeds[i] = CalculateNextGodSeed();
        }
        return godSeeds;
    }

    public int CalculateNextGodSeed()
    {
        int godSeed = 69420;
        bool hasFoundGod = false;

        while (!hasFoundGod)
        {
            (bool isPotentialGodSeed, bool isGodSeed) = CalculateItems.IsGodSeed(currentSeed);
            if (isPotentialGodSeed)
            {
                lastPotentialGodSeed = currentSeed;
                hasFoundPotentialGod = true;
                currentMode = Mode.God;
                if (isGodSeed)
                {
                    godSeed = currentSeed;
                    hasFoundGod = true;
                }
            }
            else
            {
                hasFoundPotentialGod = false;
            }
            IncrementSeed();
        }

        return godSeed;
    }
    private void IncrementSeed()
    {
        if (currentMode == Mode.God)
        {
            if (!hasFoundPotentialGod && incrementIndex >= increments.Length - 1)
            {
                currentSeed = lastPotentialGodSeed;
                currentMode = Mode.One; // Demote the mode
            }
            else
            {
                incrementIndex = hasFoundPotentialGod ? 0 : incrementIndex + 1;
                currentSeed = lastPotentialGodSeed + increments[incrementIndex];
                return;
            }
        }
        // Code should only reach this point if the currentMode = Mode.One

        currentSeed++;
    }
}
