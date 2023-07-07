using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CanvasType
{
    StartScreen,
    RunningScreen,
    SummaryScreen
}

public class CanvasManager : Singleton<CanvasManager>
{
    private List<CanvasController> canvasControllerList;
    private CanvasController lastActiveCanvas;

    protected override void Awake()
    {
        base.Awake();
        canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();
        canvasControllerList.ForEach(x => x.gameObject.SetActive(false));
        SwitchCanvas(CanvasType.StartScreen);
    }

    public void SwitchCanvas(CanvasType _type)
    {
        if (lastActiveCanvas != null)
        {
            lastActiveCanvas.gameObject.SetActive(false);
        }

        CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == _type);
        if (desiredCanvas != null)
        {
            desiredCanvas.gameObject.SetActive(true);
            lastActiveCanvas = desiredCanvas;
        }
        else
        {
            Debug.LogWarning("The desired canvas was not found!");
        }
    }
}

// Written by EngiGames https://youtu.be/vmKxLibGrMo
