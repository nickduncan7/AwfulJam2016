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
        EditorGUILayout.PrefixLabel("Upper Left Fence");
        tile.UpperLeftFence = (FenceType)EditorGUILayout.EnumPopup(tile.UpperLeftFence);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Upper Fence");
        tile.UpperFence = (FenceType)EditorGUILayout.EnumPopup(tile.UpperFence);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Upper Right Fence");
        tile.UpperRightFence = (FenceType)EditorGUILayout.EnumPopup(tile.UpperRightFence);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Lower Left Fence");
        tile.LowerLeftFence = (FenceType)EditorGUILayout.EnumPopup(tile.LowerLeftFence);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Lower Fence");
        tile.LowerFence = (FenceType)EditorGUILayout.EnumPopup(tile.LowerFence);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Lower Right Fence");
        tile.LowerRightFence = (FenceType)EditorGUILayout.EnumPopup(tile.LowerRightFence);
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
            tile.SpawnFences();
            tile.SpawnIndicator();
        }
    }


    void OnDisable()
    {
        Tools.current = LastTool;
        EditorApplication.update -= EditorUpdate;
    }
}