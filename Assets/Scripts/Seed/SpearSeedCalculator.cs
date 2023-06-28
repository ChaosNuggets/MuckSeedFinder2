using System.Collections.Generic;

public class SpearSeedCalculator : SeedCalculator
{
    public SpearSeedCalculator(int startSeed)
        : base(startSeed, new int[] {
            19,
            205, 224
        })
    { }

    protected override (bool, bool) IsGoodSeed(int seed)
    {
        return CalculateItems.IsGoodSeed(seed, new HashSet<int>() { CalculateItems.SPEAR_INDEX } );
    }
}
