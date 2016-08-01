using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[System.Serializable]
public class GridGeneratorScript : MonoBehaviour {
    // Public members
    public int Width = 8;
    public int Height = 8;
    public GameObject TilePrefab;
    public GameObject CratePrefab;
    public GameObject GuardPrefab;
    public GameObject DocumentsPrefab;
    public GameObject GunPrefab;
    public GameObject ShovelPrefab;
    public GameObject PickaxePrefab;
    public GameObject LumberPrefab;
    public GameObject EliteGuardPrefab;

    public Texture GrassTexture;
    public Texture DirtTexture;
    public Texture StoneTexture;
    public Texture ConcreteTexture;
    public Texture WoodTexture;

    public void ResetWalls()
    {
        GameObjects.WallManager.Walls.ForEach(DestroyImmediate);
        tiles.ForEach(tile => tile.GetComponent<HexTile>().SpawnWalls());

    }

    

    public GameObject MainSpawnIndicator;
    public GameObject SpawnTwoIndicator;
    public GameObject SpawnThreeIndicator;
    public GameObject SpawnFourIndicator;
    public GameObject SpawnFiveIndicator;
    public GameObject CrateIndicator;
    public GameObject GuardIndicator;
    public GameObject DocIndicator;
    public GameObject GunIndicator;
    public GameObject LumberIndicator;
    public GameObject PickaxeIndicator;
    public GameObject ShovelIndicator;

    [HideInInspector]
    public Coordinate badCoordinate = new Coordinate(-999, -999);

    // Class properties
    public List<GameObject> tiles
    {
        get
        {
            return GameObject.FindGameObjectsWithTag("GameTile").ToList();
        }
    }

    public List<Coordinate> graph;

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

    public List<Coordinate> GetNeighbors(Coordinate target, bool traversibleOnly = false)
    {
        var neighbors = GetNeighborsDirections(target, traversibleOnly);

        return (from neighbor in neighbors where !GameObjects.WallManager.WallExistsBetween(neighbor.Value, target) select neighbor.Value).ToList();
    }

    public List<Coordinate> GetAllNeighbors(Coordinate target, bool traversibleOnly = false)
    {
        var neighbors = GetNeighborsDirections(target, traversibleOnly);

        return neighbors.Select(neighbor => neighbor.Value).ToList();
    }

    public List<KeyValuePair<WallLocation, Coordinate>> GetNeighborsDirections(Coordinate target, bool traversibleOnly = false)
    {
        var neighbors = new List<KeyValuePair<WallLocation, Coordinate>>();

        if (graph.Contains(new Coordinate(target.q, target.r + 1)))
        {
            var upper = GetTileAtCoordinates(target.q, target.r + 1);
            if (upper != null && (upper.GetComponent<HexTile>().Passable || (upper.GetComponent<HexTile>().Passable && traversibleOnly)))
                neighbors.Add(new KeyValuePair<WallLocation, Coordinate>(WallLocation.Upper, upper.GetComponent<HexTile>().Coordinate));
        }

        if (graph.Contains(new Coordinate(target.q, target.r - 1)))
        {
            var lower = GetTileAtCoordinates(target.q, target.r - 1);
            if (lower != null && (lower.GetComponent<HexTile>().Passable || (lower.GetComponent<HexTile>().Passable && traversibleOnly)))
                neighbors.Add(new KeyValuePair<WallLocation, Coordinate>(WallLocation.Lower, lower.GetComponent<HexTile>().Coordinate));
        }

        if (target.q % 2 == 0)
        {
            if (graph.Contains(new Coordinate(target.q - 1, target.r)))
            {
                var leftUpper = GetTileAtCoordinates(target.q - 1, target.r);
                if (leftUpper != null && (leftUpper.GetComponent<HexTile>().Passable || (leftUpper.GetComponent<HexTile>().Passable && traversibleOnly)))
                    neighbors.Add(new KeyValuePair<WallLocation, Coordinate>(WallLocation.UpperLeft, leftUpper.GetComponent<HexTile>().Coordinate));
            }

            if (graph.Contains(new Coordinate(target.q - 1, target.r - 1)))
            {
                var leftLower = GetTileAtCoordinates(target.q - 1, target.r - 1);
                if (leftLower != null && (leftLower.GetComponent<HexTile>().Passable || (leftLower.GetComponent<HexTile>().Passable && traversibleOnly)))
                    neighbors.Add(new KeyValuePair<WallLocation, Coordinate>(WallLocation.LowerLeft, leftLower.GetComponent<HexTile>().Coordinate));
            }

            if (graph.Contains(new Coordinate(target.q + 1, target.r)))
            {
                var rightUpper = GetTileAtCoordinates(target.q + 1, target.r);
                if (rightUpper != null && (rightUpper.GetComponent<HexTile>().Passable || (rightUpper.GetComponent<HexTile>().Passable && traversibleOnly)))
                    neighbors.Add(new KeyValuePair<WallLocation, Coordinate>(WallLocation.UpperRight, rightUpper.GetComponent<HexTile>().Coordinate));
            }

            if (graph.Contains(new Coordinate(target.q + 1, target.r - 1)))
            {
                var rightLower = GetTileAtCoordinates(target.q + 1, target.r - 1);
                if (rightLower != null && (rightLower.GetComponent<HexTile>().Passable || (rightLower.GetComponent<HexTile>().Passable && traversibleOnly)))
                    neighbors.Add(new KeyValuePair<WallLocation, Coordinate>(WallLocation.LowerRight, rightLower.GetComponent<HexTile>().Coordinate));
            }
        }
        else
        {
            if (graph.Contains(new Coordinate(target.q - 1, target.r + 1)))
            {
                var leftUpper = GetTileAtCoordinates(target.q - 1, target.r + 1);
                if (leftUpper != null && (leftUpper.GetComponent<HexTile>().Passable || (leftUpper.GetComponent<HexTile>().Passable && traversibleOnly)))
                    neighbors.Add(new KeyValuePair<WallLocation, Coordinate>(WallLocation.UpperLeft, leftUpper.GetComponent<HexTile>().Coordinate));
            }

            if (graph.Contains(new Coordinate(target.q - 1, target.r)))
            {
                var leftLower = GetTileAtCoordinates(target.q - 1, target.r);
                if (leftLower != null && (leftLower.GetComponent<HexTile>().Passable || (leftLower.GetComponent<HexTile>().Passable && traversibleOnly)))
                    neighbors.Add(new KeyValuePair<WallLocation, Coordinate>(WallLocation.LowerLeft, leftLower.GetComponent<HexTile>().Coordinate));
            }

            if (graph.Contains(new Coordinate(target.q + 1, target.r + 1)))
            {
                var rightUpper = GetTileAtCoordinates(target.q + 1, target.r + 1);
                if (rightUpper != null && (rightUpper.GetComponent<HexTile>().Passable || (rightUpper.GetComponent<HexTile>().Passable && traversibleOnly)))
                    neighbors.Add(new KeyValuePair<WallLocation, Coordinate>(WallLocation.UpperRight, rightUpper.GetComponent<HexTile>().Coordinate));
            }

            if (graph.Contains(new Coordinate(target.q + 1, target.r)))
            {
                var rightLower = GetTileAtCoordinates(target.q + 1, target.r);
                if (rightLower != null && (rightLower.GetComponent<HexTile>().Passable || (rightLower.GetComponent<HexTile>().Passable && traversibleOnly)))
                    neighbors.Add(new KeyValuePair<WallLocation, Coordinate>(WallLocation.LowerRight, rightLower.GetComponent<HexTile>().Coordinate));
            }
        }

        return neighbors;
    }

    public Coordinate GetNeighborInDirection(Coordinate origin, WallLocation direction)
    {
        var neighbors = GetNeighborsDirections(origin);

        var neighbor = neighbors.Where(n => n.Key == direction).ToList();

        if (neighbor.Count == 0 || neighbor.Count > 1)
        {
            Coordinate coordinate = origin;

            if (direction == WallLocation.Upper)
            {
                coordinate.r += 1;
            }
            else if (direction == WallLocation.Lower)
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
                        case WallLocation.UpperLeft:
                            coordinate.q -= 1;
                            break;
                        case WallLocation.LowerRight:
                            coordinate.q += 1;
                            coordinate.r -= 1;
                            break;
                        case WallLocation.UpperRight:
                            coordinate.q += 1;
                            break;
                        case WallLocation.LowerLeft:
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
                        case WallLocation.UpperLeft:
                            coordinate.q -= 1;
                            coordinate.r += 1;
                            break;
                        case WallLocation.LowerRight:
                            coordinate.q += 1;
                            break;
                        case WallLocation.UpperRight:
                            coordinate.q += 1;
                            coordinate.r += 1;
                            break;
                        case WallLocation.LowerLeft:
                            coordinate.q -= 1;
                            break;
                    }
                }
            }

            return coordinate;
        }

        return neighbor[0].Value;
    }

    public void GetAccessibleTiles(Coordinate start, bool traversibleOnly = false, int moves = -1)
    {
        graph = tiles.Select(tile => tile.GetComponent<HexTile>().Coordinate).ToList();

        var visited = new HashSet<Coordinate> {start};
        var fringes = new Dictionary<int, List<Coordinate>> {{0, new List<Coordinate> {start}}};

        if (moves <= 0)
            moves = 30;

        for (var i = 1; i <= moves; i++)
        {
            fringes.Add(i, new List<Coordinate>());
            foreach (var coordinate in fringes[i - 1])
            {
                foreach (var neighbor in GetNeighbors(coordinate, traversibleOnly))
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        fringes[i].Add(neighbor);
                    }
                }
            }
        }

        graph = visited.ToList();
    }

    public List<Coordinate> CalculateRoute(Coordinate start, Coordinate end, bool traversibleOnly = false, int moves = -1)
    {
        var frontier = new Queue<Coordinate>();
        frontier.Enqueue(start);
        var cameFrom = new Dictionary<Coordinate, Coordinate?>();
        cameFrom[start] = null;

        Coordinate current;
        while (frontier.Any())
        {
            current = frontier.Dequeue();

            if (current == end)
                break;

            foreach (var neighbor in GetNeighbors(current, traversibleOnly))
            {
                if (!cameFrom.ContainsKey(neighbor))
                {
                    frontier.Enqueue(neighbor);
                    cameFrom[neighbor] = current;
                }
            }
        }

        current = end;
        var path = new List<Coordinate> { current };
        while (current != start)
        {
            if (!cameFrom.ContainsKey(current))
                break;

            current = cameFrom[current].Value;
            path.Add(current);
        }

        path.Reverse();

        if (path.Count == 1 && path[0] == end)
            return null;

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

    public void UpdateWalls()
    {
        tiles.ForEach(tile =>
        {
            var type = WallType.WoodWall;
            var destinationType = WallType.BrickWall;

            var tileScript = tile.GetComponent<HexTile>();
            if (tileScript.UpperLeftWall == type) tileScript.UpperLeftWall = destinationType;
            if (tileScript.UpperRightWall == type) tileScript.UpperRightWall = destinationType;
            if (tileScript.UpperWall == type) tileScript.UpperWall = destinationType;
            if (tileScript.LowerLeftWall == type) tileScript.LowerLeftWall = destinationType;
            if (tileScript.LowerRightWall == type) tileScript.LowerRightWall = destinationType;
            if (tileScript.LowerWall == type) tileScript.LowerWall = destinationType;
        });

        ResetWalls();
    }
}
