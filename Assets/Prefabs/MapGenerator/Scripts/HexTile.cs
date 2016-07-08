using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

public class HexTile : MonoBehaviour {
    // Private members
    private object contentInstance;

    // Public members
    public TileType Type = TileType.Standard;
    public TileRotation Rotation = TileRotation.ZeroDegrees;
    public TileContents Contents = TileContents.Nothing;

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