public class SpearFinderStarter : ButtonTemplate
{
    protected override void OnButtonClicked()
    {
        SeedFinderRunner.InitSeedFinder(startSeed => new SpearSeedCalculator(startSeed));
        CanvasManager.SwitchCanvas(CanvasType.RunningScreen);
    }
}
