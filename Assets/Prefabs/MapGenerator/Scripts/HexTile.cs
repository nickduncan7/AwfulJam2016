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
    public GameObject CratePrefab;
    public Texture StandardTexture;
    public Texture ImpassibleTexture;
    public Texture HighlightedTexture;

    [SerializeField]
    [HideInInspector]
    public Coordinate Coordinate;

    [HideInInspector]
    public bool highlighted = false;

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
            return (Weight != -1);
        }
    }

    public void UpdateRotation()
    {
        switch (Rotation)
        {
            default:
            case TileRotation.ZeroDegrees:
                transform.rotation = Quaternion.Euler(90, 0, 0);
                break;
            case TileRotation.SixtyDegrees:
                transform.rotation = Quaternion.Euler(90, 60, 0);
                break;
            case TileRotation.HundredTwentyDegrees:
                transform.rotation = Quaternion.Euler(90, 120, 0);
                break;
            case TileRotation.OneEightyDegrees:
                transform.rotation = Quaternion.Euler(90, 180, 0);
                break;
            case TileRotation.TwoFortyDegrees:
                transform.rotation = Quaternion.Euler(90, 240, 0);
                break;
            case TileRotation.ThreeHundredDegrees:
                transform.rotation = Quaternion.Euler(90, 300, 0);
                break;
        }
    }

    public void UpdateMaterial()
    {
        var meshRenderer = GetComponent<MeshRenderer>();
        if (!highlighted)
        {
            
            switch (Type)
            {
                default:
                case TileType.Standard:
                    meshRenderer.materials[0].mainTexture = StandardTexture;
                    break;
                case TileType.Impassible:
                    meshRenderer.materials[0].mainTexture = ImpassibleTexture;
                    break;

            }
        }
        else
        {
            meshRenderer.materials[0].mainTexture = HighlightedTexture;
        }
    }

    public void SpawnContents()
    {
        switch (Contents)
        {
            case TileContents.Crate:
                contentInstance = Instantiate(CratePrefab, transform.position, Quaternion.identity) as GameObject;
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
    Crate
}