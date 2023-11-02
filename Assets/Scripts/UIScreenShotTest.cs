using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScreenShotTest : MonoBehaviour
{
    public Camera screenshotCamera;
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScreenShotAndForward()
    {
        // https://forum.unity.com/threads/multiple-camera-screenshots.144597/
        var resWidth = screenshotCamera.pixelWidth;
        var resHeight = screenshotCamera.pixelHeight;

        RenderTexture rt = new(resWidth, resHeight, 24);
        screenshotCamera.targetTexture = rt;

        for(var i=0; i<3; i++)
        {
            text.text = $"{i}";

            screenshotCamera.Render();
            Texture2D screenshot = new(resWidth, resHeight, TextureFormat.RGB24, false);
            RenderTexture.active = rt;
            screenshot.ReadPixels(screenshotCamera.pixelRect, 0, 0);
            screenshot.Apply();
            byte[] bytes = screenshot.EncodeToPNG();

            System.IO.File.WriteAllBytes($"test_{i}.png", bytes);
        }

        
        screenshotCamera.targetTexture=null;
        RenderTexture.active = null;
        rt.Release();
    }
}
