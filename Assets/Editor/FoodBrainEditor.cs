using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FoodBrain))]
public class FoodBrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FoodBrain brain = (FoodBrain)target;
        if (GUILayout.Button("Die"))
        {
            brain.Die();
        }
    }
}
