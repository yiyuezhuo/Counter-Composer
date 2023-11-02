using UnityEngine;
using System;
using System.IO;


#if UNITY_EDITOR

using UnityEditor;

#endif

public static class FileExporter
{
    public static void Export(byte[] bytes, string desc, string defaultName, string ext)
    {
#if UNITY_EDITOR

        var path = EditorUtility.SaveFilePanel(desc, "", defaultName, ext);

        if(path.Length != 0)
        {
            File.WriteAllBytes(path, bytes);
        }
        return;

#endif

#if UNITY_WEBGL
        WebGLFileSaver.SaveFile(bytes, defaultName, ext);
#endif

        Debug.Log("Not supported platform");
    }
}