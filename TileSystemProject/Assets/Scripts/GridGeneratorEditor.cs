using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NodeGridGenerator))]
public class GridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NodeGridGenerator generator = (NodeGridGenerator)target;

        if(GUILayout.Button("Generate Grid"))
        {
            generator.GenerateNodeGrid();

            Debug.Log("Grid generated!");

        }
        if(GUILayout.Button("Clear Grid"))
        {
            generator.ClearNodes();

            Debug.Log("Grid cleared!");
        }
    }
}
