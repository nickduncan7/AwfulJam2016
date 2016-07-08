using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class HexTile : MonoBehaviour {
    // Private members
    private object contentInstance;

    [HideInInspector]
    public GameObject indicator;

    // Public members
    public TileType Type = TileType.Standard;
    public TileRotation Rotation = TileRotation.ZeroDegrees;
    public TileContents Contents = TileContents.Nothing;

    [HideInInspector]
    public FenceType _upperLeftFence;
    public FenceType UpperLeftFence
    {
        get
        {
            return _upperLeftFence;
        }
        set
        {
            var neighbor = gridGenerator.GetNeighborInDirection(Coordinate, FenceLocation.UpperLeft);

            if (value == FenceType.None && _upperLeftFence != value)
            {
                DestroyImmediate(fenceManager.GetFenceBetween(Coordinate, neighbor));
            }

            _upperLeftFence = value;

            var neighborTile = gridGenerator.GetTileAtCoordinates(neighbor);
            if (neighborTile != null)
                neighborTile.GetComponent<HexTile>()._lowerRightFence = value;
        }
    }

    [HideInInspector]
    public FenceType _upperFence;
    public FenceType UpperFence
    {
        get
        {
            return _upperFence;
        }
        set
        {
            var neighbor = gridGenerator.GetNeighborInDirection(Coordinate, FenceLocation.Upper);

            if (value == FenceType.None && _upperFence != value)
            {
                DestroyImmediate(fenceManager.GetFenceBetween(Coordinate, neighbor));
            }

            _upperFence = value;
            
            var neighborTile = gridGenerator.GetTileAtCoordinates(neighbor);
            if (neighborTile != null)
                neighborTile.GetComponent<HexTile>()._lowerFence = value;
        }
    }

    [HideInInspector]
    public FenceType _upperRightFence;
    public FenceType UpperRightFence
    {
        get
        {
            return _upperRightFence;
        }
        set
        {
            var neighbor = gridGenerator.GetNeighborInDirection(Coordinate, FenceLocation.UpperRight);

            if (value == FenceType.None && _upperRightFence != value)
            {
                DestroyImmediate(fenceManager.GetFenceBetween(Coordinate, neighbor));
            }

            _upperRightFence = value;

            var neighborTile = gridGenerator.GetTileAtCoordinates(neighbor);
            if (neighborTile != null)
                neighborTile.GetComponent<HexTile>()._lowerLeftFence = value;
        }
    }

    [HideInInspector]
    public FenceType _lowerLeftFence;
    public FenceType LowerLeftFence
    {
        get
        {
            return _lowerLeftFence;
        }
        set
        {
            var neighbor = gridGenerator.GetNeighborInDirection(Coordinate, FenceLocation.LowerLeft);

            if (value == FenceType.None && _lowerLeftFence != value)
            {
                DestroyImmediate(fenceManager.GetFenceBetween(Coordinate, neighbor));
            }

            _lowerLeftFence = value;
            
            var neighborTile = gridGenerator.GetTileAtCoordinates(neighbor);
            if (neighborTile != null)
                neighborTile.GetComponent<HexTile>()._upperRightFence = value;
        }
    }

    [HideInInspector]
    public FenceType _lowerFence;
    public FenceType LowerFence
    {
        get
        {
            return _lowerFence;
        }
        set
        {
            var neighbor = gridGenerator.GetNeighborInDirection(Coordinate, FenceLocation.Lower);

            if (value == FenceType.None && _lowerFence != value)
            {
                DestroyImmediate(fenceManager.GetFenceBetween(Coordinate, neighbor));
            }

            _lowerFence = value;

            var neighborTile = gridGenerator.GetTileAtCoordinates(neighbor);
            if (neighborTile != null)
                neighborTile.GetComponent<HexTile>()._upperFence = value;

        }
    }

    [HideInInspector]
    public FenceType _lowerRightFence;
    public FenceType LowerRightFence
    {
        get
        {
            return _lowerRightFence;
        }
        set
        {
            var neighbor = gridGenerator.GetNeighborInDirection(Coordinate, FenceLocation.LowerRight);

            if (value == FenceType.None && _lowerRightFence != value)
            {
                DestroyImmediate(fenceManager.GetFenceBetween(Coordinate, neighbor));
            }

            _lowerRightFence = value;
       
            var neighborTile = gridGenerator.GetTileAtCoordinates(neighbor);
            if (neighborTile != null)
                neighborTile.GetComponent<HexTile>()._upperLeftFence = value;
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

    private GridGeneratorScript gridGenerator
    {
        get { return transform.parent.GetComponent<GridGeneratorScript>(); }
    }

    private FenceManagerScript fenceManager
    {
        get { return GameObject.Find("FenceManager").GetComponent<FenceManagerScript>(); }
    }

    public int Weight
    {
        get
        {
            switch (Type)
            {
                default:
                case TileType.Standard:
                    return 1;
                case TileType.Impassible:
                    return -1;
            }
        }
    }

    public bool Passable
    {
        get
        {
            return (Weight != -1 && !occupied);
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

    public void UpdateMaterial()
    {
        var meshRenderer = GetComponent<MeshRenderer>();
        var materials = meshRenderer.sharedMaterials;
        if (pathHighlighted)
        {
            materials[1] = gridGenerator.PathMaterial;
        }
        else if (highlighted)
        {
            materials[1] = gridGenerator.HighlightedMaterial;
        }
        else
        {
            
            switch (Type)
            {
                default:
                case TileType.Standard:
                    materials[1] = gridGenerator.StandardMaterial;
                    break;
                case TileType.Impassible:
                    materials[1] = gridGenerator.ImpassibleMaterial;
                    break;

            }
        }

        meshRenderer.materials = materials;
    }

    public void SpawnFences()
    {
        foreach(FenceLocation fenceLocation in Enum.GetValues(typeof(FenceLocation)))
        {
            FenceType fenceType = FenceType.None;
            if (fenceLocation == FenceLocation.UpperLeft)
                fenceType = UpperLeftFence;
            else if (fenceLocation == FenceLocation.Upper)
                fenceType = UpperFence;
            else if (fenceLocation == FenceLocation.UpperRight)
                fenceType = UpperRightFence;
            else if (fenceLocation == FenceLocation.LowerLeft)
                fenceType = LowerLeftFence;
            else if (fenceLocation == FenceLocation.Lower)
                fenceType = LowerFence;
            else if (fenceLocation == FenceLocation.LowerRight)
                fenceType = LowerRightFence;

            if (!fenceManager.FenceExistsBetween(Coordinate, gridGenerator.GetNeighborInDirection(Coordinate, fenceLocation)))
                fenceManager.SpawnFence(fenceType, fenceLocation, gameObject);
        }
    }

    public void ClearFences()
    {
        foreach (FenceLocation fenceLocation in Enum.GetValues(typeof(FenceLocation)))
        {
            if (fenceLocation == FenceLocation.UpperLeft)
                UpperLeftFence = FenceType.None;
            else if (fenceLocation == FenceLocation.Upper)
                UpperFence = FenceType.None;
            else if (fenceLocation == FenceLocation.UpperRight)
                UpperRightFence = FenceType.None;
            else if (fenceLocation == FenceLocation.LowerLeft)
                LowerLeftFence = FenceType.None;
            else if (fenceLocation == FenceLocation.Lower)
                LowerFence = FenceType.None;
            else if (fenceLocation == FenceLocation.LowerRight)
                LowerRightFence = FenceType.None;
        }
    }

    public void SpawnIndicator()
    {
        if (indicator != null) DestroyImmediate(indicator);
        switch (Contents)
        {
            case TileContents.MainSpawn:
                if (indicator == null) indicator = Instantiate(gridGenerator.MainSpawnIndicator, transform.position + Vector3.up * 0.1f, Quaternion.Euler(90,0,0)) as GameObject;
                break;
            case TileContents.SpawnTwo:
                if (indicator == null) indicator = Instantiate(gridGenerator.SpawnTwoIndicator, transform.position + Vector3.up * 0.1f, Quaternion.Euler(90, 0, 0)) as GameObject;
                break;
            case TileContents.SpawnThree:
                if (indicator == null) indicator = Instantiate(gridGenerator.SpawnThreeIndicator, transform.position + Vector3.up * 0.1f, Quaternion.Euler(90, 0, 0)) as GameObject;
                break;
            case TileContents.SpawnFour:
                if (indicator == null) indicator = Instantiate(gridGenerator.SpawnFourIndicator, transform.position + Vector3.up * 0.1f, Quaternion.Euler(90, 0, 0)) as GameObject;
                break;
            case TileContents.SpawnFive:
                if (indicator == null) indicator = Instantiate(gridGenerator.SpawnFiveIndicator, transform.position + Vector3.up * 0.1f, Quaternion.Euler(90, 0, 0)) as GameObject;
                break;
            case TileContents.Crate:
                if (indicator == null) indicator = Instantiate(gridGenerator.CrateIndicator, transform.position + Vector3.up * 0.1f, Quaternion.Euler(90, 0, 0)) as GameObject;
                break;
        }

        if (indicator != null) indicator.transform.SetParent(transform);
    }

    public void SpawnContents()
    {
        switch (Contents)
        {
            case TileContents.Crate:
                contentInstance = Instantiate(gridGenerator.CratePrefab, transform.position, Quaternion.identity) as GameObject;
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
        SpawnFences();
    }
	
	// Update is called once per frame
	void Update()
    {
        UpdateMaterial();
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
    Standard,
    Impassible
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
public enum FenceType
{
    None,
    BarbedWire
}