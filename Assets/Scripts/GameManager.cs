using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public TMP_InputField oobSource;
    public SheetContainer sheetContainer;
    public TMP_InputField widthSource;
    public TMP_InputField heightSource;
    public TMP_InputField runtimeScriptSource;

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
        var runtimeScript = runtimeScriptSource.text;
        var width = int.Parse(widthSource.text);
        var height = int.Parse(heightSource.text);

        sheetContainer.Generate(oobData, runtimeScript, width, height);
    }

    public void ExportFirst()
    {
        foreach(var counterByte in IterExport(true)) // TODO: refactor as  `using`
        {
            FileExporter.Export(counterByte, "desc", "counter.png", "png");
        }
    }

    public void ExportArchive()
    {
        var nameDataIter = IterExport(false).Select((bytes, i) => ($"{i}.png", bytes));
        var archiveBytes = FileExporter.Archive(nameDataIter);
        FileExporter.Export(archiveBytes, "desc", "counters.zip", "zip");
    }

    public IEnumerable<byte[]> IterExport(bool firstCounter)
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
            yield break;
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
                // FileExporter.Export(bytes, "desc", "test.png", "png");
                // break;
                yield return bytes;

                counterTransform.SetParent(sheetTransform);
                counterTransform.localScale = Vector3.one;
                counterTransform.SetSiblingIndex(idxOri);

                if(firstCounter)
                    break;
            }

            if(firstCounter)
                break;
        }

        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        rt.Release();
    }
}
