using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridGeneratorScript : MonoBehaviour {
    // Editor properties

    [SerializeField]
    public int Width = 8;

    [SerializeField]
    public int Height = 8;

    public GameObject TilePrefab;

    private List<GameObject> tiles;

    public void GenerateGrid()
    {
        tiles = new List<GameObject>();
        Debug.Log(string.Format("Generating {0}x{1} grid...", Width, Height));
        ClearGrid();
        Vector3 position = Vector3.zero;

        for (int q = 0; q < Width; q++)
        {
            int qOffset = q >> 1;

            for (int r = -qOffset; r < Height - qOffset; r++)
            {
                var distance = 3.5f;
                position.x = distance * 3.0f / 2.0f * q;
                position.z = distance * Mathf.Sqrt(3.0f) * (r + q / 2.0f);

                var newTile = Instantiate(TilePrefab, position, Quaternion.identity) as GameObject;
                newTile.transform.parent = this.transform;
                tiles.Add(newTile);
            }
        }
        Debug.Log("Generation of grid complete.");
    }

    public void ClearGrid()
    {
        tiles.ForEach(tile => DestroyImmediate(tile));
        tiles.Clear();
    }

	// Use this for initialization
	void Start ()
    { 
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
