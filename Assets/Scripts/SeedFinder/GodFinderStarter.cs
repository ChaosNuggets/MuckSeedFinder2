public class GodFinderStarter : ButtonTemplate
{
    protected override void OnButtonClicked()
    {
        SeedFinderRunner.InitSeedFinder(startSeed => new GodSeedCalculator(startSeed));
        CanvasManager.SwitchCanvas(CanvasType.RunningScreen);
    }
}
