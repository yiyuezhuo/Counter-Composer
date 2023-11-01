
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(ScreenShotTest))]
public class ScreenShotTestEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Takeshot"))
        {
            var sst = (ScreenShotTest)target;
            sst.TakePics();
        }
    }
}
