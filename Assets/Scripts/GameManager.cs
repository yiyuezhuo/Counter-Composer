using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public TMP_InputField oobSource;
    public SheetContainer sheetContainer;
    public TMP_InputField widthSource;
    public TMP_InputField heightSource;

    public Transform screenshotBench;
    public Camera screenshotCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Generate()
    {
        var oobData = OobData.Load(oobSource.text);
        var width = int.Parse(widthSource.text);
        var height = int.Parse(heightSource.text);

        sheetContainer.Generate(oobData, width, height);
    }

    public void ExportFirst()
    {
        Transform firstSheetTransform = null;
        foreach(Transform sheetTransform in sheetContainer.transform)
        {
            firstSheetTransform = sheetTransform;
            break;
        }

        if(firstSheetTransform == null)
        {
            Debug.Log("There should be at least one sheet to download");
            return;
        }

        var cellSize = firstSheetTransform.GetComponent<GridLayoutGroup>().cellSize;
        
        var width = (int)MathF.Ceiling(cellSize.x);
        var height = (int)MathF.Ceiling(cellSize.y);
        
        /*
        width = 1920;
        height = 1080;
        */

        /*
        width = 1720;
        height = 880;
        */

        /*
        width = 1220;
        height = 800;
        */

        // RenderTexture rt = new(width, height, 24);
        RenderTexture rt = new(Screen.width, Screen.height, 24);
        screenshotCamera.targetTexture = rt;

        var screenWidthHalf = Screen.width / 2;
        var widthHalf = width / 2;
        var screenHeightHalf = Screen.height / 2;
        var heightHalf = height / 2;

        foreach(Transform sheetTransform in sheetContainer.transform)
        {
            foreach(Transform counterTransform in sheetTransform)
            {
                //ScreenshotCounter(sheetTransform, counterTransform, width, height);

                var idxOri = counterTransform.GetSiblingIndex();
                counterTransform.SetParent(screenshotBench);
                counterTransform.position = new Vector3(0, 0, counterTransform.position.z);
                counterTransform.localScale = Vector3.one;

                /*
                // https://stackoverflow.com/questions/63677023/convert-gameobject-to-image
                var rectTransform = counterTransform.GetComponent<RectTransform>();
                var delta = rectTransform.sizeDelta;
                var position = rectTransform.position;

                var offset = new RectOffset((int) (delta.x / 2), (int) (delta.x / 2), (int) (delta.y / 2), (int) (delta.y / 2));
                var rect   = new Rect(position, Vector2.zero);
                var add    = offset.Add(rect);
                */

                screenshotCamera.Render();
                Texture2D screenshot = new(width, height, TextureFormat.RGB24, false);
                // Texture2D screenshot = new(1920, 1080, TextureFormat.RGB24, false);
                RenderTexture.active = rt;

                // screenshot.ReadPixels(screenshotCamera.pixelRect, 0, 0);
                // screenshot.ReadPixels(add, 0, 0);
                Rect rect = new(screenWidthHalf - widthHalf, screenHeightHalf - heightHalf, width, height);
                screenshot.ReadPixels(rect, 0, 0);

                screenshot.Apply();
                byte[] bytes = screenshot.EncodeToPNG();
                
                // System.IO.File.WriteAllBytes($"test.png", bytes);
                FileExporter.Export(bytes, "desc", "test.png", "png");
                // break;

                counterTransform.SetParent(sheetTransform);
                counterTransform.localScale = Vector3.one;
                counterTransform.SetSiblingIndex(idxOri);

                break;
            }
            break;
        }

        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        rt.Release();
    }

    void ScreenshotCounter(Transform sheetTransform, Transform counterTransform, int width, int height)
    {
        var idxOri = counterTransform.GetSiblingIndex();
        counterTransform.SetParent(screenshotBench);

        

        counterTransform.SetParent(sheetTransform);
        counterTransform.SetSiblingIndex(idxOri);
    }
}
