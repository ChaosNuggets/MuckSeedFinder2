using System.Collections.Generic;

public class GodSeedCalculator : SeedCalculator
{
    public GodSeedCalculator(int startSeed)
        : base(startSeed, new int[] {
            19,
            1204, 1223, 1242, 1261, 1280,
            7745, 7764,
            9025, 9044
        })
    { }

    protected override (bool, bool) IsGoodSeed(int seed)
    {
        return CalculateItems.IsGoodSeed(seed, new HashSet<int>() { CalculateItems.SPEAR_INDEX, CalculateItems.BOW_INDEX } );
    }
}
