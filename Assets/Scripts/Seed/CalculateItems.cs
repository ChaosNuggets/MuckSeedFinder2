using System.Collections.Generic;
using System.Linq;

public static class CalculateItems
{
    const int BowIndex = 9;
    const int SpearIndex = 10;
    private const int ChestSize = 21;
    private static int[] cells = new int[ChestSize];

    private static readonly float[] dropChances =
    {
            0.5f,   // 0:  Iron bar 
            0.5f,   // 1:  Gold bar
            0.15f,  // 2:  Gold Axe
            0.15f,  // 3:  Gold Pickaxe
            0.5f,   // 4:  Coin
            0.5f,   // 5:  Bread
            0.15f,  // 6:  Gold Sword
            0.1f,   // 7:  Mithril Sword
            0.15f,  // 8:  Fir bow
            0.025f, // 9:  Ancient Bow
            0.03f,  // 10: Chiefs Spear
            0.1f    // 11: Spear Tip
        };

    // Returns whether or not it's a potential god seed,
    // and then whether or not it's an actual god seed
    public static (bool, bool) IsGodSeed(int seed)
    {
        ConsistentRandom rand = new ConsistentRandom(seed);
        List<int> items = new List<int>();
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = -1;
        }

        // Go to next random number 9 times
        for (int i = 0; i < dropChances.Count(); i++)
        {
            if (rand.NextDouble() < (double)dropChances[i])
            {
                items.Add(i);
            }
            else if (i == BowIndex || i == SpearIndex)
            {
                return (false, false);
            }
        }
        // Code should only reach this point if there's both spear and bow in the item list

        return (true, IsChestGood(items, rand));
    }

    private static bool IsChestGood(List<int> items, ConsistentRandom rand)
    {
        List<int> intList = new List<int>();
        for (int index = 0; index < ChestSize; index++)
        {
            intList.Add(index);
        }
        foreach (int item in items)
        {
            int index = rand.Next(0, intList.Count);
            intList.Remove(index);
            cells[index] = item;
        }

        return cells.Contains(SpearIndex) && cells.Contains(BowIndex);
    }
}
