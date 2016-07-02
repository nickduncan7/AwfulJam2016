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
        Quaternion rotation = Quaternion.Euler(0, 0, 0);
        switch (Rotation)
        {
            case TileRotation.ZeroPercent:
                rotation = Quaternion.Euler(0, 0, 0);
                break;
            case TileRotation.SixtyPercent:
                rotation = Quaternion.Euler(0, 60, 0);
                break;
            case TileRotation.HundredTwentyPercent:
                rotation = Quaternion.Euler(0, 120, 0);
                break;
            case TileRotation.OneEightyPercent:
                rotation = Quaternion.Euler(0, 180, 0);
                break;
            case TileRotation.TwoFortyPercent:
                rotation = Quaternion.Euler(0, 240, 0);
                break;
            case TileRotation.ThreeHundredPercent:
                rotation = Quaternion.Euler(0, 300, 0);
                break;
            default:
                rotation = Quaternion.Euler(0, 0, 0);
                break;
        }

        transform.rotation = rotation;
    }

    // Use this for initialization
    void Start ()
    {
        UpdateRotation();
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