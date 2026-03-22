using System;
using UnityEngine;

public class BackgroundCapture : MonoBehaviour
{
    [ContextMenu("Screenshot")]
    public void Screenshot()
    {
        string fileName = "GameScreenshot_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        ScreenCapture.CaptureScreenshot(fileName + ".png");
        Debug.Log("Screenshot saved: " + fileName);
    }
}