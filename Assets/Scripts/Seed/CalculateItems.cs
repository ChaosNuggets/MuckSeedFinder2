using System;
using System.Collections.Generic;
using System.Linq;

public static class CalculateItems
{
    public const int BOW_INDEX = 9;
    public const int SPEAR_INDEX = 10;

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

    // Returns whether or not it's a potential seed,
    // and then whether or not it's an actual seed
    public static (bool, bool) IsGoodSeed(int seed, HashSet<int> indexes)
    {
        ConsistentRandom rand = new(seed);
        List<int> items = new();

        // Go to next random number 9 times
        for (int i = 0; i < dropChances.Count(); i++)
        {
            if (rand.NextDouble() < dropChances[i])
            {
                items.Add(i);
            }
            else if (indexes.Contains(i))
            {
                return (false, false);
            }
        }
        // Code should only reach this point if there's all of the indexes in the item list

        return (true, IsChestGood(items, rand, indexes));
    }

    private static bool IsChestGood(List<int> items, ConsistentRandom rand, HashSet<int> indexes)
    {
        const int CHEST_SIZE = 21;
        int[] cells = new int[CHEST_SIZE];
        Array.Fill(cells, -1);

        List<int> intList = new();
        for (int index = 0; index < CHEST_SIZE; index++)
        {
            intList.Add(index);
        }

        foreach (int item in items)
        {
            int index = rand.Next(0, intList.Count);
            intList.Remove(index);
            cells[index] = item;
        }

        return indexes.IsSubsetOf(cells);
    }
}
