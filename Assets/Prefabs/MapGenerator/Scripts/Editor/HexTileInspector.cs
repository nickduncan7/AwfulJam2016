using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

[CustomEditor(typeof(HexTile)), CanEditMultipleObjects]
public class HexTileInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        string coordinates = String.Empty;
        if (targets.Length > 1)
        {
            EditorGUILayout.HelpBox("Cannot edit walls when more than one tile is selected.", MessageType.Info);
        }
        else
        {
            var tile = target as HexTile;
            coordinates = string.Format("(Q: {0}, R: {1})", tile.Coordinate.q, tile.Coordinate.r);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Coordinates");
            EditorGUILayout.LabelField(coordinates);
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Upper Left Wall");
            tile.UpperLeftWall = (WallType) EditorGUILayout.EnumPopup(tile.UpperLeftWall);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Upper Wall");
            tile.UpperWall = (WallType) EditorGUILayout.EnumPopup(tile.UpperWall);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Upper Right Wall");
            tile.UpperRightWall = (WallType) EditorGUILayout.EnumPopup(tile.UpperRightWall);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Lower Left Wall");
            tile.LowerLeftWall = (WallType) EditorGUILayout.EnumPopup(tile.LowerLeftWall);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Lower Wall");
            tile.LowerWall = (WallType) EditorGUILayout.EnumPopup(tile.LowerWall);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Lower Right Wall");
            tile.LowerRightWall = (WallType) EditorGUILayout.EnumPopup(tile.LowerRightWall);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Clear All Walls"))
                tile.ClearWalls();
        }

        serializedObject.ApplyModifiedProperties();
        foreach (HexTile tileScript in serializedObject.targetObjects)
        {
            tileScript.SpawnIndicator();
            tileScript.UpdateMaterial(true);
        }
    }
}