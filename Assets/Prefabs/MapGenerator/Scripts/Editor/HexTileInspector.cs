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

        var coordinates = string.Format("(Q: {0}, R: {1})", tile.Coordinate.q, tile.Coordinate.r);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Coordinates");
        EditorGUILayout.LabelField(coordinates);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Upper Left Wall");
        tile.UpperLeftWall = (WallType)EditorGUILayout.EnumPopup(tile.UpperLeftWall);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Upper Wall");
        tile.UpperWall = (WallType)EditorGUILayout.EnumPopup(tile.UpperWall);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Upper Right Wall");
        tile.UpperRightWall = (WallType)EditorGUILayout.EnumPopup(tile.UpperRightWall);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Lower Left Wall");
        tile.LowerLeftWall = (WallType)EditorGUILayout.EnumPopup(tile.LowerLeftWall);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Lower Wall");
        tile.LowerWall = (WallType)EditorGUILayout.EnumPopup(tile.LowerWall);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Lower Right Wall");
        tile.LowerRightWall = (WallType)EditorGUILayout.EnumPopup(tile.LowerRightWall);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Clear All Walls"))
            tile.ClearWalls();
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
            tile.UpdateMaterial(true);
            tile.SpawnWalls();
            tile.SpawnIndicator();
        }
    }


    void OnDisable()
    {
        Tools.current = LastTool;
        EditorApplication.update -= EditorUpdate;
    }
}