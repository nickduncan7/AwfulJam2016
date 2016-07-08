﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class GridGeneratorScript : MonoBehaviour {
    // Public members
    public int Width = 8;
    public int Height = 8;
    public GameObject TilePrefab;
    public GameObject CratePrefab;

    public Material StandardMaterial;
    public Material ImpassibleMaterial;
    public Material HighlightedMaterial;
    public Material PathMaterial;

    public GameObject MainSpawnIndicator;
    public GameObject SpawnTwoIndicator;
    public GameObject SpawnThreeIndicator;
    public GameObject SpawnFourIndicator;
    public GameObject SpawnFiveIndicator;
    public GameObject CrateIndicator;

    [HideInInspector]
    public Coordinate badCoordinate = new Coordinate(-999, -999);

    // Class properties
    private List<GameObject> tiles
    {
        get
        {
            return GameObject.FindGameObjectsWithTag("GameTile").ToList();
        }
    }

    private FenceManagerScript fenceManager
    {
        get { return GameObject.Find("FenceManager").GetComponent<FenceManagerScript>(); }
    }

    #region Spawn Tile Properties

    [SerializeField]
    [HideInInspector]
    private Coordinate _mainSpawn;
    private bool _mainSpawnSet = false;
    public Coordinate MainSpawn
    {
        get
        {
            if (!_mainSpawnSet)
            {
                _mainSpawn = tiles.First(x => x.GetComponent<HexTile>().Contents == TileContents.MainSpawn).GetComponent<HexTile>().Coordinate;
                _mainSpawnSet = true;
            }

            return _mainSpawn;
        }
    }

    [SerializeField]
    [HideInInspector]
    private Coordinate _spawnTwo;
    private bool _spawnTwoSet = false;
    public Coordinate SpawnTwo
    {
        get
        {
            if (tiles.All(x => x.GetComponent<HexTile>().Contents != TileContents.SpawnTwo))
                return new Coordinate(-1, -1);

            if (!_spawnTwoSet)
            {
                _spawnTwo = tiles.First(x => x.GetComponent<HexTile>().Contents == TileContents.SpawnTwo).GetComponent<HexTile>().Coordinate;
                _spawnTwoSet = true;
            }

            return _spawnTwo;
        }
    }

    [SerializeField]
    [HideInInspector]
    private Coordinate _spawnThree;
    private bool _spawnThreeSet = false;
    public Coordinate SpawnThree
    {
        get
        {
            if (tiles.All(x => x.GetComponent<HexTile>().Contents != TileContents.SpawnThree))
                return new Coordinate(-1, -1);

            if (!_spawnThreeSet)
            {
                _spawnThree = tiles.First(x => x.GetComponent<HexTile>().Contents == TileContents.SpawnThree).GetComponent<HexTile>().Coordinate;
                _spawnThreeSet = true;
            }

            return _spawnThree;
        }
    }

    [SerializeField]
    [HideInInspector]
    private Coordinate _spawnFour;
    private bool _spawnFourSet = false;
    public Coordinate SpawnFour
    {
        get
        {
            if (tiles.All(x => x.GetComponent<HexTile>().Contents != TileContents.SpawnFour))
                return new Coordinate(-1, -1);

            if (!_spawnFourSet)
            {
                _spawnFour = tiles.First(x => x.GetComponent<HexTile>().Contents == TileContents.SpawnFour).GetComponent<HexTile>().Coordinate;
                _spawnFourSet = true;
            }

            return _spawnFour;
        }
    }

    [SerializeField]
    [HideInInspector]
    private Coordinate _spawnFive;
    private bool _spawnFiveSet = false;
    public Coordinate SpawnFive
    {
        get
        {
            if (tiles.All(x => x.GetComponent<HexTile>().Contents != TileContents.SpawnFive))
                return new Coordinate(-1, -1);

            if (!_spawnFiveSet)
            {
                _spawnFive = tiles.First(x => x.GetComponent<HexTile>().Contents == TileContents.SpawnFive).GetComponent<HexTile>().Coordinate;
                _spawnFiveSet = true;
            }

            return _spawnFive;
        }
    }

    #endregion

    public List<Coordinate> GetNeighbors(Coordinate target)
    {
        var neighbors = GetNeighborsDirections(target);
        var neighborList = new List<Coordinate>();

        foreach(var neighbor in neighbors)
        {
            if (!fenceManager.FenceExistsBetween(neighbor.Value, target))
                neighborList.Add(neighbor.Value);
        }

        return neighborList;
    }

    public List<KeyValuePair<FenceLocation, Coordinate>> GetNeighborsDirections(Coordinate target)
    {
        var neighbors = new List<KeyValuePair<FenceLocation, Coordinate>>();

        var upper = GetTileAtCoordinates(target.q, target.r + 1);
        if (upper != null && upper.GetComponent<HexTile>().Passable)
            neighbors.Add(new KeyValuePair<FenceLocation, Coordinate>(FenceLocation.Upper, upper.GetComponent<HexTile>().Coordinate));

        var lower = GetTileAtCoordinates(target.q, target.r - 1);
        if (lower != null && lower.GetComponent<HexTile>().Passable)
            neighbors.Add(new KeyValuePair<FenceLocation, Coordinate>(FenceLocation.Lower, lower.GetComponent<HexTile>().Coordinate));

        if (target.q % 2 == 0)
        {
            var leftUpper = GetTileAtCoordinates(target.q - 1, target.r);
            if (leftUpper != null && leftUpper.GetComponent<HexTile>().Passable)
                neighbors.Add(new KeyValuePair<FenceLocation, Coordinate>(FenceLocation.UpperLeft, leftUpper.GetComponent<HexTile>().Coordinate));

            var leftLower = GetTileAtCoordinates(target.q - 1, target.r - 1);
            if (leftLower != null && leftLower.GetComponent<HexTile>().Passable)
                neighbors.Add(new KeyValuePair<FenceLocation, Coordinate>(FenceLocation.LowerLeft, leftLower.GetComponent<HexTile>().Coordinate));

            var rightUpper = GetTileAtCoordinates(target.q + 1, target.r);
            if (rightUpper != null && rightUpper.GetComponent<HexTile>().Passable)
                neighbors.Add(new KeyValuePair<FenceLocation, Coordinate>(FenceLocation.UpperRight, rightUpper.GetComponent<HexTile>().Coordinate));

            var rightLower = GetTileAtCoordinates(target.q + 1, target.r - 1);
            if (rightLower != null && rightLower.GetComponent<HexTile>().Passable)
                neighbors.Add(new KeyValuePair<FenceLocation, Coordinate>(FenceLocation.LowerRight, rightLower.GetComponent<HexTile>().Coordinate));
        }
        else
        {
            var leftUpper = GetTileAtCoordinates(target.q - 1, target.r + 1);
            if (leftUpper != null && leftUpper.GetComponent<HexTile>().Passable)
                neighbors.Add(new KeyValuePair<FenceLocation, Coordinate>(FenceLocation.UpperLeft, leftUpper.GetComponent<HexTile>().Coordinate));

            var leftLower = GetTileAtCoordinates(target.q - 1, target.r);
            if (leftLower != null && leftLower.GetComponent<HexTile>().Passable)
                neighbors.Add(new KeyValuePair<FenceLocation, Coordinate>(FenceLocation.LowerLeft, leftLower.GetComponent<HexTile>().Coordinate));

            var rightUpper = GetTileAtCoordinates(target.q + 1, target.r + 1);
            if (rightUpper != null && rightUpper.GetComponent<HexTile>().Passable)
                neighbors.Add(new KeyValuePair<FenceLocation, Coordinate>(FenceLocation.UpperRight, rightUpper.GetComponent<HexTile>().Coordinate));

            var rightLower = GetTileAtCoordinates(target.q + 1, target.r);
            if (rightLower != null && rightLower.GetComponent<HexTile>().Passable)
                neighbors.Add(new KeyValuePair<FenceLocation, Coordinate>(FenceLocation.LowerRight, rightLower.GetComponent<HexTile>().Coordinate));
        }

        return neighbors;
    }

    public Coordinate GetNeighborInDirection(Coordinate origin, FenceLocation direction)
    {
        var neighbors = GetNeighborsDirections(origin);

        var neighbor = neighbors.Where(n => n.Key == direction).ToList();

        if (neighbor.Count == 0 || neighbor.Count > 1)
        {
            Coordinate coordinate = origin;

            if (direction == FenceLocation.Upper)
            {
                coordinate.r += 1;
            }
            else if (direction == FenceLocation.Lower)
            {
                
                coordinate.r -= 1;
            }
            else
            {
                if (origin.q%2 == 0)
                {
                    switch (direction)
                    {
                        default:
                        case FenceLocation.UpperLeft:
                            coordinate.q -= 1;
                            break;
                        case FenceLocation.LowerRight:
                            coordinate.q += 1;
                            coordinate.r -= 1;
                            break;
                        case FenceLocation.UpperRight:
                            coordinate.q += 1;
                            break;
                        case FenceLocation.LowerLeft:
                            coordinate.q -= 1;
                            coordinate.r -= 1;
                            break;
                    }
                }
                else
                {
                    switch (direction)
                    {
                        default:
                        case FenceLocation.UpperLeft:
                            coordinate.q -= 1;
                            coordinate.r += 1;
                            break;
                        case FenceLocation.LowerRight:
                            coordinate.q += 1;
                            break;
                        case FenceLocation.UpperRight:
                            coordinate.q += 1;
                            coordinate.r += 1;
                            break;
                        case FenceLocation.LowerLeft:
                            coordinate.q -= 1;
                            break;
                    }
                }
            }

            return coordinate;
        }

        return neighbor[0].Value;
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
                var newTileScript = newTile.GetComponent<HexTile>();

                newTileScript.Coordinate.q = q;
                newTileScript.Coordinate.r = logicalRow++;
                newTile.transform.parent = this.transform;
                newTile.tag = "GameTile";

                newTileScript.UpdateMaterial();
                newTileScript.UpdateRotation();
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

            if (tileScript.Coordinate.q == q && tileScript.Coordinate.r == r)
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
