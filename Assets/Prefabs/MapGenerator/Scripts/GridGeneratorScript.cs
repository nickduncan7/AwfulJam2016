using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class GridGeneratorScript : MonoBehaviour {
    // Public members
    public int Width = 8;
    public int Height = 8;
    public GameObject TilePrefab;

    private List<GameObject> tiles
    {
        get
        {
            return GameObject.FindGameObjectsWithTag("GameTile").ToList();
        }
    }

    public void GenerateGrid()
    {
        Debug.Log(string.Format("Generating {0}x{1} grid...", Width, Height));
        ClearGrid();

        Vector3 position = Vector3.zero;

        
        for (int q = 0; q < Width; q++)
        {
            int logicalRow = 0;
            int qOffset = q >> 1;

            for (int r = -qOffset; r < Height - qOffset; r++)
            {
                var distance = 1f;
                position.x = distance * 3.0f / 2.0f * q;
                position.z = distance * Mathf.Sqrt(3.0f) * (r + q / 2.0f);

                var newTile = Instantiate(TilePrefab, position, Quaternion.identity) as GameObject;
                newTile.GetComponent<HexTile>().q = q;
                newTile.GetComponent<HexTile>().r = logicalRow++;
                newTile.transform.parent = this.transform;
                newTile.tag = "GameTile";
            } 
        }
        Debug.Log("Generation of grid complete.");
    }

    public GameObject GetTileAtCoordinates(int q, int r)
    {
        foreach (var tile in tiles)
        {
            HexTile tileScript = tile.GetComponent<HexTile>();

            if (tileScript.q == q && tileScript.r == r)
                return tile;
        }
        return null;
    }

    public void ClearGrid()
    {
        Debug.Log("Clearing tiles...");

        var objectsToDetroy = new List<GameObject>();

        foreach(GameObject tile in tiles)
            objectsToDetroy.Add(tile);

        foreach (GameObject objectToDestroy in objectsToDetroy)
            DestroyImmediate(objectToDestroy);

        Debug.Log("Tiles cleared.");
    }
}
