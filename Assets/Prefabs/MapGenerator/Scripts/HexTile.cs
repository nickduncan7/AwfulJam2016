using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

[ExecuteInEditMode]
public class HexTile : MonoBehaviour {
    // Private members
    private int _q, _r;

    [SerializeField]
    public TileRotation Rotation = TileRotation.ZeroPercent;

    [SerializeField]
    public TileType Type = TileType.Standard;

    [SerializeField]
    public Texture StandardTexture;

    [SerializeField]
    public Texture ImpassibleTexture;

    // Class constructor with optional coordinate parameters
    public HexTile(int q = 0, int r = 0, int s = 0)
    {
        if ((q + r + s) != 0)
        {
            throw new UnityException(String.Format("Coordinates of hex tile must equal zero. Given coordinates were: (q:{0}, r:{1}, s:{2})", q, r, s));
        }
        else
        {
            this.q = q;
            this.r = r;
        }
    }

    // Class properties for coordinates
    public int q
    {
        get
        {
            return _q;
        }
        set
        {
            _q = value;
        }
    }

    public int r
    {
        get
        {
            return _r;
        }
        set
        {
            _r = value;
        }
    }

    public int s
    {
        get
        {
            return ( - q - r);
        }
        set
        {
            throw new UnityException("Cannot set \"s\" coordinate on hex tile directly. Hex grid utilizes axial coordinates.");
        }
    }

    public void UpdateRotation()
    {
        switch (Rotation)
        {
            default:
            case TileRotation.ZeroPercent:
                transform.rotation = Quaternion.Euler(90, 0, 0);
                break;
            case TileRotation.SixtyPercent:
                transform.rotation = Quaternion.Euler(90, 60, 0);
                break;
            case TileRotation.HundredTwentyPercent:
                transform.rotation = Quaternion.Euler(90, 120, 0);
                break;
            case TileRotation.OneEightyPercent:
                transform.rotation = Quaternion.Euler(90, 180, 0);
                break;
            case TileRotation.TwoFortyPercent:
                transform.rotation = Quaternion.Euler(90, 240, 0);
                break;
            case TileRotation.ThreeHundredPercent:
                transform.rotation = Quaternion.Euler(90, 300, 0);
                break;
        }
    }

    public void UpdateMaterial()
    {
        var meshRenderer = GetComponent<MeshRenderer>();
        switch(Type)
        {
            default:
            case TileType.Standard:
                meshRenderer.sharedMaterial.mainTexture = StandardTexture;
                break;
            case TileType.Impassible:
                meshRenderer.sharedMaterial.mainTexture = ImpassibleTexture;
                break;
             
        }
    }

    // Use this for initialization
    void Start ()
    {
        UpdateRotation();
        UpdateMaterial();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    void OnEnable()
    {
        EditorApplication.update += EditorUpdate;
    }

    void EditorUpdate()
    {
        UpdateRotation();
        UpdateMaterial();
    }

    void OnDisable()
    {
        EditorApplication.update -= EditorUpdate;
    }
}

[Serializable]
public enum TileRotation
{
    ZeroPercent,
    SixtyPercent,
    HundredTwentyPercent,
    OneEightyPercent,
    TwoFortyPercent,
    ThreeHundredPercent
}

[Serializable]
public enum TileType
{
    Standard,
    Impassible
}