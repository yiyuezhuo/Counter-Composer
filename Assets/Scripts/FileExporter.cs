using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.IO.Compression;
using System.Collections;
using System.Collections.Generic;


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

    public static byte[] Archive(IEnumerable<(string, byte[])> nameDataIter)
    {
        // https://stackoverflow.com/questions/17232414/creating-a-zip-archive-in-memory-using-system-io-compression
        using (var outStream = new MemoryStream())
        {
            using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
            {
                foreach((var fileName, var fileBytes) in nameDataIter)
                {
                    var fileInArchive = archive.CreateEntry(fileName);
                    using (var entryStream = fileInArchive.Open())
                    using (var fileToCompressStream = new MemoryStream(fileBytes))
                    {
                        fileToCompressStream.CopyTo(entryStream);
                    }

                }
            }
            return outStream.ToArray();
        }
    }
}