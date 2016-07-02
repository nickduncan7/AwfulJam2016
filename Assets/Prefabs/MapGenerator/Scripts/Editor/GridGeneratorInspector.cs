using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(GridGeneratorScript))]
public class GridGeneratorInspector : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GridGeneratorScript grid = target as GridGeneratorScript;

        if (GUILayout.Button("Generate Grid"))
            grid.GenerateGrid();

        if (GUILayout.Button("Clear Grid"))
            grid.ClearGrid();
    }
}