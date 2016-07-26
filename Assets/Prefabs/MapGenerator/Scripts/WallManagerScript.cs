using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WallManagerScript : MonoBehaviour {

    // Class properties
    public List<GameObject> Walls
    {
        get
        {
            return GameObject.FindGameObjectsWithTag("Wall").ToList();
        }
    }

    public bool WallExistsBetween(Coordinate A, Coordinate B)
    {
        foreach (var wall in Walls)
        {
            var wallScript = wall.GetComponent<WallScript>();

            if ((wallScript.betweenA == A && wallScript.betweenB == B)
                || (wallScript.betweenA == B && wallScript.betweenB == A))
            {
                return true;
            }
        }

        return false;
    }

    public GameObject GetWallBetween(Coordinate A, Coordinate B)
    {
        if (WallExistsBetween(A, B))
        {
            foreach (var Wall in Walls)
            {
                var WallScript = Wall.GetComponent<WallScript>();

                if ((WallScript.betweenA == A && WallScript.betweenB == B)
                    || (WallScript.betweenA == B && WallScript.betweenB == A))
                {
                    return Wall;
                }
            }
        }
            

        return null;
    }

    public GameObject WireFencePrefab;
    public GameObject WoodWallPrefab;
    public GameObject BrickWallPrefab;

    public List<Coordinate> GetBetween(Coordinate origin, WallLocation location)
    {
        Coordinate other;

        

        switch (location)
        {
            default:
            case WallLocation.Upper:
                other = GameObjects.GridGenerator.GetNeighborInDirection(origin, WallLocation.Upper);
                break;
            case WallLocation.Lower:
                other = GameObjects.GridGenerator.GetNeighborInDirection(origin, WallLocation.Lower);
                break;
            case WallLocation.UpperLeft:
                other = GameObjects.GridGenerator.GetNeighborInDirection(origin, WallLocation.UpperLeft);
                break;
            case WallLocation.LowerRight:
                other = GameObjects.GridGenerator.GetNeighborInDirection(origin, WallLocation.LowerRight);
                break;
            case WallLocation.UpperRight:
                other = GameObjects.GridGenerator.GetNeighborInDirection(origin, WallLocation.UpperRight);
                break;
            case WallLocation.LowerLeft:
                other = GameObjects.GridGenerator.GetNeighborInDirection(origin, WallLocation.LowerLeft);
                break;
        }

        return new List<Coordinate> { origin, other };
    }

    public void SpawnWall(WallType type, WallLocation location, GameObject owner)
    {
        GameObject WallObject;

        switch (type)
        {
            default:
            case WallType.None:
                return;
            case WallType.BarbedWire:
                WallObject = Instantiate(WireFencePrefab) as GameObject;
                break;
            case WallType.BrickWall:
                WallObject = Instantiate(BrickWallPrefab) as GameObject;
                break;
            case WallType.WoodWall:
                WallObject = Instantiate(WoodWallPrefab) as GameObject;
                break;
        }
        
        var WallScript = WallObject.GetComponent<WallScript>();
        WallScript.betweenA = owner.GetComponent<HexTile>().Coordinate;
        WallScript.betweenB = GetBetween(WallScript.betweenA, location)[1];

        WallObject.transform.SetParent(owner.transform);

        switch (location)
        {
            default:
            case WallLocation.Upper:
                WallObject.transform.localPosition = new Vector3(-0.8683267f, 0f);
                break;
            case WallLocation.Lower:
                WallObject.transform.localPosition = new Vector3(0.8683267f, 0f);
                break;
            case WallLocation.UpperLeft:
                WallObject.transform.localPosition = new Vector3(-0.4341633f, 0.7519927f);
                WallObject.transform.localRotation = Quaternion.Euler(0f, 0f, -60f);
                break;
            case WallLocation.LowerRight:
                WallObject.transform.localPosition = new Vector3(0.4341633f, -0.7519927f);
                WallObject.transform.localRotation = Quaternion.Euler(0f, 0f, -60f);
                break;
            case WallLocation.UpperRight:
                WallObject.transform.localPosition = new Vector3(-0.4341624f, -0.7519927f);
                WallObject.transform.localRotation = Quaternion.Euler(0f, 0f, -120f);
                break;
            case WallLocation.LowerLeft:
                WallObject.transform.localPosition = new Vector3(0.4341624f, 0.7519927f);
                WallObject.transform.localRotation = Quaternion.Euler(0f, 0f, -120f);
                break;
        }
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

[System.Serializable]
public enum WallLocation
{
    UpperLeft = 0,
    Upper = 1,
    UpperRight = 2,
    LowerLeft = 3,
    Lower = 4,
    LowerRight = 5
}