using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class HexTile : MonoBehaviour {
    // Private members
    private object contentInstance;
    private readonly Color defaultColor = new Color(0.5f, 0.5f, 0.5f);
    private readonly Color highlightColor = new Color(0.6f, 0.6f, 0.6f);
    private readonly Color pathHighlightColor = new Color(0.7f, 0.7f, 0.7f);

    [HideInInspector]
    public GameObject indicator;

    // Public members
    public TileType Type = TileType.Grass;
    public bool Traversible = true;
    public TileRotation Rotation = TileRotation.ZeroDegrees;
    public TileContents Contents = TileContents.Nothing;
    

    [HideInInspector]
    public WallType _upperLeftWall;
    public WallType UpperLeftWall
    {
        get
        {
            return _upperLeftWall;
        }
        set
        {
            var neighbor = GameObjects.GridGenerator.GetNeighborInDirection(Coordinate, WallLocation.UpperLeft);

            if (value == WallType.None || _upperLeftWall != value)
            {
                DestroyImmediate(GameObjects.WallManager.GetWallBetween(Coordinate, neighbor));
            }

            _upperLeftWall = value;

            var neighborTile = GameObjects.GridGenerator.GetTileAtCoordinates(neighbor);
            if (neighborTile != null)
                neighborTile.GetComponent<HexTile>()._lowerRightWall = value;
        }
    }

    [HideInInspector]
    public WallType _upperWall;
    public WallType UpperWall
    {
        get
        {
            return _upperWall;
        }
        set
        {
            var neighbor = GameObjects.GridGenerator.GetNeighborInDirection(Coordinate, WallLocation.Upper);

            if (value == WallType.None || _upperWall != value)
            {
                DestroyImmediate(GameObjects.WallManager.GetWallBetween(Coordinate, neighbor));
            }

            _upperWall = value;
            
            var neighborTile = GameObjects.GridGenerator.GetTileAtCoordinates(neighbor);
            if (neighborTile != null)
                neighborTile.GetComponent<HexTile>()._lowerWall = value;
        }
    }

    [HideInInspector]
    public WallType _upperRightWall;
    public WallType UpperRightWall
    {
        get
        {
            return _upperRightWall;
        }
        set
        {
            var neighbor = GameObjects.GridGenerator.GetNeighborInDirection(Coordinate, WallLocation.UpperRight);

            if (value == WallType.None || _upperRightWall != value)
            {
                DestroyImmediate(GameObjects.WallManager.GetWallBetween(Coordinate, neighbor));
            }

            _upperRightWall = value;

            var neighborTile = GameObjects.GridGenerator.GetTileAtCoordinates(neighbor);
            if (neighborTile != null)
                neighborTile.GetComponent<HexTile>()._lowerLeftWall = value;
        }
    }

    [HideInInspector]
    public WallType _lowerLeftWall;
    public WallType LowerLeftWall
    {
        get
        {
            return _lowerLeftWall;
        }
        set
        {
            var neighbor = GameObjects.GridGenerator.GetNeighborInDirection(Coordinate, WallLocation.LowerLeft);

            if (value == WallType.None || _lowerLeftWall != value)
            {
                DestroyImmediate(GameObjects.WallManager.GetWallBetween(Coordinate, neighbor));
            }

            _lowerLeftWall = value;
            
            var neighborTile = GameObjects.GridGenerator.GetTileAtCoordinates(neighbor);
            if (neighborTile != null)
                neighborTile.GetComponent<HexTile>()._upperRightWall = value;
        }
    }

    [HideInInspector]
    public WallType _lowerWall;
    public WallType LowerWall
    {
        get
        {
            return _lowerWall;
        }
        set
        {
            var neighbor = GameObjects.GridGenerator.GetNeighborInDirection(Coordinate, WallLocation.Lower);

            if (value == WallType.None || _lowerWall != value)
            {
                DestroyImmediate(GameObjects.WallManager.GetWallBetween(Coordinate, neighbor));
            }

            _lowerWall = value;

            var neighborTile = GameObjects.GridGenerator.GetTileAtCoordinates(neighbor);
            if (neighborTile != null)
                neighborTile.GetComponent<HexTile>()._upperWall = value;

        }
    }

    [HideInInspector]
    public WallType _lowerRightWall;
    public WallType LowerRightWall
    {
        get
        {
            return _lowerRightWall;
        }
        set
        {
            var neighbor = GameObjects.GridGenerator.GetNeighborInDirection(Coordinate, WallLocation.LowerRight);

            if (value == WallType.None || _lowerRightWall != value)
            {
                DestroyImmediate(GameObjects.WallManager.GetWallBetween(Coordinate, neighbor));
            }

            _lowerRightWall = value;
       
            var neighborTile = GameObjects.GridGenerator.GetTileAtCoordinates(neighbor);
            if (neighborTile != null)
                neighborTile.GetComponent<HexTile>()._upperLeftWall = value;
        }
    }

    [SerializeField]
    [HideInInspector]
    public Coordinate Coordinate;

    [HideInInspector]
    public bool occupied = false;

    [HideInInspector]
    public bool highlighted = false;

    [HideInInspector]
    public bool pathHighlighted = false;

    public int Weight
    {
        get
        {
            switch (Type)
            {
                default:
                    return 1;
            }
        }
    }

    public bool Passable
    {
        get
        {
            return (Traversible && !occupied);
        }
    }

    public void UpdateRotation()
    {
        switch (Rotation)
        {
            default:
            case TileRotation.ZeroDegrees:
                transform.rotation = Quaternion.Euler(-90, 0, 90);
                break;
            case TileRotation.SixtyDegrees:
                transform.rotation = Quaternion.Euler(-90, 60, 90);
                break;
            case TileRotation.HundredTwentyDegrees:
                transform.rotation = Quaternion.Euler(-90, 120, 90);
                break;
            case TileRotation.OneEightyDegrees:
                transform.rotation = Quaternion.Euler(-90, 180, 90);
                break;
            case TileRotation.TwoFortyDegrees:
                transform.rotation = Quaternion.Euler(-90, 240, 90);
                break;
            case TileRotation.ThreeHundredDegrees:
                transform.rotation = Quaternion.Euler(-90, 300, 90);
                break;
        }
    }

    public void UpdateMaterial(bool inEditor = false)
    {
        var meshRenderer = GetComponent<MeshRenderer>();
        var materials = meshRenderer.sharedMaterials;
        
        switch (Type)
        {
            default:
            case TileType.Grass:
                materials[1] = GameObjects.GridGenerator.GrassMaterial;
                break;
            case TileType.Dirt:
                materials[1] = GameObjects.GridGenerator.DirtMaterial;
                break;
            case TileType.Stone:
                materials[1] = GameObjects.GridGenerator.StoneMaterial;
                break;
            case TileType.Concrete:
                materials[1] = GameObjects.GridGenerator.ConcreteMaterial;
                break;
            case TileType.Wood:
                materials[1] = GameObjects.GridGenerator.WoodMaterial;
                break;

        }

        meshRenderer.materials = materials;

        if (!inEditor)
        {
            if (pathHighlighted)
            {
                meshRenderer.materials[1].SetColor("_Color", pathHighlightColor);
            }
            else if (highlighted)
            {
                meshRenderer.materials[1].SetColor("_Color", highlightColor);
            }
            else
            {
                meshRenderer.materials[1].SetColor("_Color", defaultColor);
            }
        }
    }

    public void SpawnWalls()
    {
        foreach(WallLocation wallLocation in Enum.GetValues(typeof(WallLocation)))
        {
            WallType wallType = WallType.None;
            if (wallLocation == WallLocation.UpperLeft)
                wallType = UpperLeftWall;
            else if (wallLocation == WallLocation.Upper)
                wallType = UpperWall;
            else if (wallLocation == WallLocation.UpperRight)
                wallType = UpperRightWall;
            else if (wallLocation == WallLocation.LowerLeft)
                wallType = LowerLeftWall;
            else if (wallLocation == WallLocation.Lower)
                wallType = LowerWall;
            else if (wallLocation == WallLocation.LowerRight)
                wallType = LowerRightWall;

            if (!GameObjects.WallManager.WallExistsBetween(Coordinate, GameObjects.GridGenerator.GetNeighborInDirection(Coordinate, wallLocation)))
                GameObjects.WallManager.SpawnWall(wallType, wallLocation, gameObject);
        }
    }

    public void ClearWalls()
    {
        foreach (WallLocation wallLocation in Enum.GetValues(typeof(WallLocation)))
        {
            if (wallLocation == WallLocation.UpperLeft)
                UpperLeftWall = WallType.None;
            else if (wallLocation == WallLocation.Upper)
                UpperWall = WallType.None;
            else if (wallLocation == WallLocation.UpperRight)
                UpperRightWall = WallType.None;
            else if (wallLocation == WallLocation.LowerLeft)
                LowerLeftWall = WallType.None;
            else if (wallLocation == WallLocation.Lower)
                LowerWall = WallType.None;
            else if (wallLocation == WallLocation.LowerRight)
                LowerRightWall = WallType.None;
        }
    }

    public void SpawnIndicator()
    {
        if (indicator != null) DestroyImmediate(indicator);
        switch (Contents)
        {
            case TileContents.MainSpawn:
                if (indicator == null) indicator = Instantiate(GameObjects.GridGenerator.MainSpawnIndicator, transform.position + Vector3.up * 0.1f, Quaternion.Euler(90,0,0)) as GameObject;
                break;
            case TileContents.SpawnTwo:
                if (indicator == null) indicator = Instantiate(GameObjects.GridGenerator.SpawnTwoIndicator, transform.position + Vector3.up * 0.1f, Quaternion.Euler(90, 0, 0)) as GameObject;
                break;
            case TileContents.SpawnThree:
                if (indicator == null) indicator = Instantiate(GameObjects.GridGenerator.SpawnThreeIndicator, transform.position + Vector3.up * 0.1f, Quaternion.Euler(90, 0, 0)) as GameObject;
                break;
            case TileContents.SpawnFour:
                if (indicator == null) indicator = Instantiate(GameObjects.GridGenerator.SpawnFourIndicator, transform.position + Vector3.up * 0.1f, Quaternion.Euler(90, 0, 0)) as GameObject;
                break;
            case TileContents.SpawnFive:
                if (indicator == null) indicator = Instantiate(GameObjects.GridGenerator.SpawnFiveIndicator, transform.position + Vector3.up * 0.1f, Quaternion.Euler(90, 0, 0)) as GameObject;
                break;
            case TileContents.Crate:
                if (indicator == null) indicator = Instantiate(GameObjects.GridGenerator.CrateIndicator, transform.position + Vector3.up * 0.1f, Quaternion.Euler(90, 0, 0)) as GameObject;
                break;
        }

        if (indicator != null) indicator.transform.SetParent(transform);
    }

    public void SpawnContents()
    {
        switch (Contents)
        {
            case TileContents.Crate:
                contentInstance = Instantiate(GameObjects.GridGenerator.CratePrefab, transform.position, Quaternion.identity) as GameObject;
                ((GameObject)contentInstance).transform.parent = null;
                break;
        }
    }

    public void DestroyContents()
    {
        if (contentInstance != null)
            Destroy(((GameObject)contentInstance));
    }

    // Use this for initialization
    void Start()
    {
        UpdateRotation();
        SpawnContents();
        SpawnWalls();
    }
}

[Serializable]
public enum TileRotation
{
    ZeroDegrees,
    SixtyDegrees,
    HundredTwentyDegrees,
    OneEightyDegrees,
    TwoFortyDegrees,
    ThreeHundredDegrees
}

[Serializable]
public enum TileType
{
    Grass,
    Dirt,
    Stone,
    Concrete,
    Wood
}

[Serializable]
public enum TileContents
{
    Nothing,
    MainSpawn,
    SpawnTwo,
    SpawnThree,
    SpawnFour,
    SpawnFive,
    Crate
}

[Serializable]
public enum WallType
{
    None,
    BarbedWire,
    DEVSolidWall
}