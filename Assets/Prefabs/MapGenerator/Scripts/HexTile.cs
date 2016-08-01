using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class HexTile : MonoBehaviour {
    // Private members
    private object contentInstance;
    private readonly Color defaultEdgeColor = new Color(0f, 0f, 0f);
    private readonly Color highlightedEdgeColor = new Color(1f, 1f, 1f);
    private readonly Color defaultColor = new Color(0.5f, 0.5f, 0.5f);
    private readonly Color highlightColor = new Color(0.7f, 0.7f, 0.7f);
    private readonly Color pathHighlightColor = new Color(0.8f, 0.8f, 0.8f);

    [HideInInspector]
    public GameObject indicator;

    // Public members
    [SerializeField]
    public TileType Type = TileType.Grass;

    [SerializeField]
    public bool Traversible = true;

    [SerializeField]
    public bool Safe = false;

    [SerializeField]
    public bool EscapeZone = false;

    [SerializeField]
    public Rotation Rotation = Rotation.ZeroDegrees;

    [SerializeField]
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

            SpawnWalls();
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

            SpawnWalls();
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

            SpawnWalls();
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

            SpawnWalls();
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

            SpawnWalls();
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

            SpawnWalls();
        }
    }

    [SerializeField]
    [HideInInspector]
    public Coordinate Coordinate;

    [SerializeField]
    [HideInInspector]
    public bool occupied = false;

    [SerializeField]
    [HideInInspector]
    public UnitType OccupierType = UnitType.None;

    [SerializeField]
    [HideInInspector]
    public GameObject Occupier;

    [HideInInspector]
    public bool highlighted = false;

    [HideInInspector]
    public bool pathHighlighted = false;

    public int Weight
    {
        get
        {
            return 1;
            //switch (Type)
            //{
            //    default:
            //    case TileType.Grass:
            //        return 25;
            //    case TileType.Dirt:
            //        return 30;
            //    case TileType.Stone:
            //        return 25;
            //    case TileType.Concrete:
            //        return 20;
            //    case TileType.Wood:
            //        return 20;
            //}
        }
    }

    public bool Passable
    {
        get
        {
            var isPlayer = GameObjects.GameManager.currentUnit.GetComponent<PlayerCharacterScript>() != null;
            var isEnemy = GameObjects.GameManager.currentUnit.GetComponent<GuardScript>() != null;

            if (isPlayer)
            {
                return ((Traversible && !occupied) || (Traversible && GameObjects.GameManager.currentUnit.GetComponent<PlayerCharacterScript>() != null && GameObjects.GameManager.currentUnit.GetComponent<PlayerCharacterScript>().hasGun));
            }
            else if (isEnemy)
            {
                return (Traversible && !occupied);
            }

            return false;
        }
    }

    public void UpdateRotation()
    {
        switch (Rotation)
        {
            default:
            case Rotation.ZeroDegrees:
                transform.rotation = Quaternion.Euler(-90, 0, 90);
                break;
            case Rotation.SixtyDegrees:
                transform.rotation = Quaternion.Euler(-90, 60, 90);
                break;
            case Rotation.HundredTwentyDegrees:
                transform.rotation = Quaternion.Euler(-90, 120, 90);
                break;
            case Rotation.OneEightyDegrees:
                transform.rotation = Quaternion.Euler(-90, 180, 90);
                break;
            case Rotation.TwoFortyDegrees:
                transform.rotation = Quaternion.Euler(-90, 240, 90);
                break;
            case Rotation.ThreeHundredDegrees:
                transform.rotation = Quaternion.Euler(-90, 300, 90);
                break;
        }
    }

    public void UpdateMaterial(bool inEditor = false)
    {
        var meshRenderer = GetComponent<MeshRenderer>();
        Texture desiredTexture = null;

        switch (Type)
        {
            default:
            case TileType.Grass:
                desiredTexture = GameObjects.GridGenerator.GrassTexture;
                break;
            case TileType.Dirt:
                desiredTexture = GameObjects.GridGenerator.DirtTexture;
                break;
            case TileType.Stone:
                desiredTexture = GameObjects.GridGenerator.StoneTexture;
                break;
            case TileType.Concrete:
                desiredTexture = GameObjects.GridGenerator.ConcreteTexture;
                break;
            case TileType.Wood:
                desiredTexture = GameObjects.GridGenerator.WoodTexture;
                break;
        }

        if (meshRenderer.sharedMaterials[1].mainTexture != desiredTexture)
        {
            meshRenderer.sharedMaterials[1].mainTexture = desiredTexture;
        }

        if (!inEditor)
        {
            if (pathHighlighted)
            {
                meshRenderer.materials[0].EnableKeyword("_EMISSION");
                meshRenderer.materials[0].SetColor("_EmissionColor", highlightedEdgeColor);
                meshRenderer.materials[1].SetColor("_Color", pathHighlightColor);
            }
            else if (highlighted)
            {
                meshRenderer.materials[0].DisableKeyword("_EMISSION");
                meshRenderer.materials[0].SetColor("_EmissionColor", defaultEdgeColor);
                meshRenderer.materials[1].SetColor("_Color", highlightColor);
            }
            else
            {
                meshRenderer.materials[0].DisableKeyword("_EMISSION");
                meshRenderer.materials[0].SetColor("_EmissionColor", defaultEdgeColor);
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
        if (Contents == TileContents.Nothing) DestroyImmediate(indicator);
        if (indicator != null) return;

        GameObject objectToInstantiate = null;

        // Set the object to instantiate
        switch (Contents)
        {
            case TileContents.MainSpawn:
                objectToInstantiate = GameObjects.GridGenerator.MainSpawnIndicator;
                break;
            case TileContents.SpawnTwo:
                objectToInstantiate = GameObjects.GridGenerator.SpawnTwoIndicator;
                break;
            case TileContents.SpawnThree:
                objectToInstantiate = GameObjects.GridGenerator.SpawnThreeIndicator;
                break;
            case TileContents.SpawnFour:
                objectToInstantiate = GameObjects.GridGenerator.SpawnFourIndicator;
                break;
            case TileContents.SpawnFive:
                objectToInstantiate = GameObjects.GridGenerator.SpawnFiveIndicator;
                break;
            case TileContents.Crate:
                objectToInstantiate = GameObjects.GridGenerator.CrateIndicator;
                break;
            case TileContents.Guard:
                objectToInstantiate = GameObjects.GridGenerator.GuardIndicator;
                break;
            case TileContents.Documents:
                objectToInstantiate = GameObjects.GridGenerator.DocIndicator;
                break;
            case TileContents.Gun:
                objectToInstantiate = GameObjects.GridGenerator.GunIndicator;
                break;
            case TileContents.Lumber:
                objectToInstantiate = GameObjects.GridGenerator.LumberIndicator;
                break;
            case TileContents.Pickaxe:
                objectToInstantiate = GameObjects.GridGenerator.PickaxeIndicator;
                break;
            case TileContents.Shovel:
                objectToInstantiate = GameObjects.GridGenerator.ShovelIndicator;
                break;
        }

        // Finally, spawn the indicator
        if (objectToInstantiate != null)
        {
            if (indicator == null)
                indicator = Instantiate(objectToInstantiate, transform.position + Vector3.up*0.1f, Quaternion.Euler(90, 0, 0)) as GameObject;
            if (indicator != null) indicator.transform.SetParent(transform);
        }
    }

    public void SpawnContents()
    {
        if (contentInstance != null) return;
        switch (Contents)
        {
            case TileContents.Crate:
                contentInstance = Instantiate(GameObjects.GridGenerator.CratePrefab, transform.position, Quaternion.identity);
                ((GameObject)contentInstance).transform.parent = null;
                break;
            case TileContents.Documents:
                contentInstance = Instantiate(GameObjects.GridGenerator.DocumentsPrefab, transform.position, Quaternion.identity);
                ((GameObject)contentInstance).transform.parent = null;
                break;
            case TileContents.Gun:
                contentInstance = Instantiate(GameObjects.GridGenerator.GunPrefab, transform.position, Quaternion.identity);
                ((GameObject)contentInstance).transform.parent = null;
                break;
            case TileContents.Shovel:
                contentInstance = Instantiate(GameObjects.GridGenerator.ShovelPrefab, transform.position, Quaternion.identity);
                ((GameObject)contentInstance).transform.parent = null;
                break;
            case TileContents.Lumber:
                contentInstance = Instantiate(GameObjects.GridGenerator.LumberPrefab, transform.position, Quaternion.identity);
                ((GameObject)contentInstance).transform.parent = null;
                break;
            case TileContents.Pickaxe:
                contentInstance = Instantiate(GameObjects.GridGenerator.PickaxePrefab, transform.position, Quaternion.identity);
                ((GameObject)contentInstance).transform.parent = null;
                break;
            case TileContents.Guard:
                var guardInstance = Instantiate(GameObjects.GridGenerator.GuardPrefab, transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
                var guardScript = guardInstance.GetComponent<GuardScript>();
                guardScript.currentLocation = Coordinate;
                guardScript.indicator = indicator;
                guardInstance.transform.parent = null;
                occupied = true;
                Occupier = guardInstance;
                OccupierType = UnitType.Enemy;
                guardScript.PostSpawn();
                break;
        }
    }

    public TileContents DestroyContents()
    {
        if (contentInstance != null)
            Destroy(((GameObject) contentInstance));

        return Contents;
    }

    // Use this for initialization
    void Start()
    {
        //UpdateRotation();
        SpawnContents();
    }
}

[Serializable]
public enum Rotation
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
    Crate,
    Guard,
    Documents,
    Gun,
    Pickaxe,
    Shovel,
    Lumber
}

[Serializable]
public enum WallType
{
    None,
    BarbedWire,
    WoodWall,
    BrickWall
}