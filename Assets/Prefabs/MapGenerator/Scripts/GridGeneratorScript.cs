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

    public List<Coordinate> GetNeighbors(Coordinate target)
    {
        var neighbors = new List<Coordinate>();

        var above = GetTileAtCoordinates(target.q, target.r + 1);
        if (above != null && above.GetComponent<HexTile>().Passable)
            neighbors.Add(above.GetComponent<HexTile>().coordinate);

        var below = GetTileAtCoordinates(target.q, target.r - 1);
        if (below != null && below.GetComponent<HexTile>().Passable)
            neighbors.Add(below.GetComponent<HexTile>().coordinate);

        if (target.q % 2 == 0)
        {
            var leftAbove = GetTileAtCoordinates(target.q - 1, target.r);
            if (leftAbove != null && leftAbove.GetComponent<HexTile>().Passable)
                neighbors.Add(leftAbove.GetComponent<HexTile>().coordinate);

            var leftBelow = GetTileAtCoordinates(target.q - 1, target.r - 1);
            if (leftBelow != null && leftBelow.GetComponent<HexTile>().Passable)
                neighbors.Add(leftBelow.GetComponent<HexTile>().coordinate);

            var rightAbove = GetTileAtCoordinates(target.q + 1, target.r);
            if (rightAbove != null && rightAbove.GetComponent<HexTile>().Passable)
                neighbors.Add(rightAbove.GetComponent<HexTile>().coordinate);

            var rightBelow = GetTileAtCoordinates(target.q + 1, target.r - 1);
            if (rightBelow != null && rightBelow.GetComponent<HexTile>().Passable)
                neighbors.Add(rightBelow.GetComponent<HexTile>().coordinate);
        }
        else
        {
            var leftAbove = GetTileAtCoordinates(target.q - 1, target.r + 1);
            if (leftAbove != null && leftAbove.GetComponent<HexTile>().Passable)
                neighbors.Add(leftAbove.GetComponent<HexTile>().coordinate);

            var leftBelow = GetTileAtCoordinates(target.q - 1, target.r);
            if (leftBelow != null && leftBelow.GetComponent<HexTile>().Passable)
                neighbors.Add(leftBelow.GetComponent<HexTile>().coordinate);

            var rightAbove = GetTileAtCoordinates(target.q + 1, target.r + 1);
            if (rightAbove != null && rightAbove.GetComponent<HexTile>().Passable)
                neighbors.Add(rightAbove.GetComponent<HexTile>().coordinate);

            var rightBelow = GetTileAtCoordinates(target.q + 1, target.r);
            if (rightBelow != null && rightBelow.GetComponent<HexTile>().Passable)
                neighbors.Add(rightBelow.GetComponent<HexTile>().coordinate);
        }

        return neighbors;
    }

    public List<Coordinate> CalculateRoute(Coordinate start, Coordinate end)
    {
        var cameFrom = new Dictionary<Coordinate, Coordinate>();
        var costSoFar = new Dictionary<Coordinate, double>();
        var searchGraph = new PriorityQueue<Coordinate>();
        searchGraph.Enqueue(start, 0);

        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (searchGraph.Count > 0)
        {
            var current = searchGraph.Dequeue();

            if (current.Equals(end))
                return BuildPath(cameFrom, current, start);

            foreach (var next in GetNeighbors(current))
            {
                double newCost = costSoFar[current] + 1;
                if (!costSoFar.ContainsKey(next)
                    || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    double priority = newCost + 1;
                    searchGraph.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }

        return null;
    }

    private List<Coordinate> BuildPath(Dictionary<Coordinate, Coordinate> cameFrom, Coordinate current, Coordinate start)
    {
        var path = new List<Coordinate>();
        path.Add(current);

        while (cameFrom.Keys.Contains(current) && current != start)
        {
            current = cameFrom[current];
            path.Add(current);
        }

        path.Reverse();
        path.Remove(start);

        return path;
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
                newTile.GetComponent<HexTile>().coordinate.q = q;
                newTile.GetComponent<HexTile>().coordinate.r = logicalRow++;
                newTile.transform.parent = this.transform;
                newTile.tag = "GameTile";
            } 
        }
        Debug.Log("Generation of grid complete.");
    }

    public GameObject GetTileAtCoordinates(Coordinate coordinates)
    {
        return GetTileAtCoordinates(coordinates.q, coordinates.r);
    }

    public GameObject GetTileAtCoordinates(int q, int r)
    {
        foreach (var tile in tiles)
        {
            HexTile tileScript = tile.GetComponent<HexTile>();

            if (tileScript.coordinate.q == q && tileScript.coordinate.r == r)
                return tile;
        }
        return null;
    }

    public void ClearGrid()
    {
        Debug.Log("Clearing tiles...");

        foreach(GameObject tile in tiles)
            DestroyImmediate(tile);

        Debug.Log("Tiles cleared.");
    }
}
