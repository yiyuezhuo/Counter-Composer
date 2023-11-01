using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotTest : MonoBehaviour
{
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // [ContextMenu("TakePics")]
    public void TakePics()
    {
        RenderTexture rt = new(cam.pixelWidth, cam.pixelHeight, 24);
        cam.targetTexture = rt;
        cam.Render();
        Texture2D screenShot = new Texture2D(cam.pixelWidth, cam.pixelHeight, TextureFormat.RGB24, false);
        RenderTexture.active = rt;
        screenShot.ReadPixels(cam.pixelRect, 0, 0);
        screenShot.Apply();
        byte[] bytes = screenShot.EncodeToPNG();
        var filename = "test.png";
        System.IO.File.WriteAllBytes(filename, bytes);
        cam.targetTexture = null;
        RenderTexture.active = null;
        rt.Release();
    }
}
