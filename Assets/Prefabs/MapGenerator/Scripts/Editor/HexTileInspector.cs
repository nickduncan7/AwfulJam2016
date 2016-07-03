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

        var coordinates = string.Format("({0}, {1})", tile.q, tile.r);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Coordinates");
        EditorGUILayout.LabelField(coordinates);
        EditorGUILayout.EndHorizontal();
    }
}