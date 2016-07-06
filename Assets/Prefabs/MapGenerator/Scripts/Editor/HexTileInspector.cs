using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(HexTile))]
public class HexTileInspector : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        HexTile tile = target as HexTile;

        var coordinates = string.Format("({0}, {1})", tile.Coordinate.q, tile.Coordinate.r);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Coordinates");
        EditorGUILayout.LabelField(coordinates);
        EditorGUILayout.EndHorizontal();
    }

    Tool LastTool = Tool.None;

    void OnEnable()
    {
        LastTool = Tools.current;
        Tools.current = Tool.None;
        EditorApplication.update += EditorUpdate;
    }

    void EditorUpdate()
    {
        HexTile tile = target as HexTile;
        if (tile != null)
        {
            tile.UpdateRotation();
            tile.UpdateMaterial();
        }
    }


    void OnDisable()
    {
        Tools.current = LastTool;
        EditorApplication.update -= EditorUpdate;
    }
}