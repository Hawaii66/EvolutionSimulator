using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DrynessMap))]
public class DrynessMapRegenerate : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrynessMap brain = (DrynessMap)target;
        if (GUILayout.Button("Redraw"))
        {
            brain.ReDraw();
        }
    }
}
