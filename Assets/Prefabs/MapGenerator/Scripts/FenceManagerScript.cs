using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FenceManagerScript : MonoBehaviour {

    private GridGeneratorScript grid
    {
        get { return GameObject.Find("GridGenerator").GetComponent<GridGeneratorScript>(); }
    }

    // Class properties
    public List<GameObject> fences
    {
        get
        {
            return GameObject.FindGameObjectsWithTag("Fence").ToList();
        }
    }

    public bool FenceExistsBetween(Coordinate A, Coordinate B)
    {
        foreach (var fence in fences)
        {
            var fenceScript = fence.GetComponent<FenceScript>();

            if ((fenceScript.betweenA == A && fenceScript.betweenB == B)
                || (fenceScript.betweenA == B && fenceScript.betweenB == A))
            {
                return true;
            }
        }

        return false;
    }

    public GameObject WireFencePrefab;

    public List<Coordinate> GetBetween(Coordinate origin, FenceLocation location)
    {
        Coordinate other;

        

        switch (location)
        {
            default:
            case FenceLocation.Upper:
                other = grid.GetNeighborInDirection(origin, FenceLocation.Upper);
                break;
            case FenceLocation.Lower:
                other = grid.GetNeighborInDirection(origin, FenceLocation.Lower);
                break;
            case FenceLocation.UpperLeft:
                other = grid.GetNeighborInDirection(origin, FenceLocation.UpperLeft);
                break;
            case FenceLocation.LowerRight:
                other = grid.GetNeighborInDirection(origin, FenceLocation.LowerRight);
                break;
            case FenceLocation.UpperRight:
                other = grid.GetNeighborInDirection(origin, FenceLocation.UpperRight);
                break;
            case FenceLocation.LowerLeft:
                other = grid.GetNeighborInDirection(origin, FenceLocation.LowerLeft);
                break;
        }

        return new List<Coordinate> { origin, other };
    }

    public void SpawnFence(FenceType type, FenceLocation location, GameObject owner)
    {
        GameObject fenceObject;

        switch (type)
        {
            default:
            case FenceType.None:
                return;
            case FenceType.BarbedWire:
                fenceObject = Instantiate(WireFencePrefab) as GameObject;
                break;
        }
        
        var fenceScript = fenceObject.GetComponent<FenceScript>();
        fenceScript.betweenA = owner.GetComponent<HexTile>().Coordinate;
        fenceScript.betweenB = GetBetween(fenceScript.betweenA, location)[1];

        fenceObject.transform.SetParent(owner.transform);

        switch (location)
        {
            default:
            case FenceLocation.Upper:
                fenceObject.transform.localPosition = new Vector3(-0.8683267f, 0f);
                break;
            case FenceLocation.Lower:
                fenceObject.transform.localPosition = new Vector3(0.8683267f, 0f);
                break;
            case FenceLocation.UpperLeft:
                fenceObject.transform.localPosition = new Vector3(-0.4341633f, 0.7519927f);
                fenceObject.transform.localRotation = Quaternion.Euler(0f, 0f, -60f);
                break;
            case FenceLocation.LowerRight:
                fenceObject.transform.localPosition = new Vector3(0.4341633f, -0.7519927f);
                fenceObject.transform.localRotation = Quaternion.Euler(0f, 0f, -60f);
                break;
            case FenceLocation.UpperRight:
                fenceObject.transform.localPosition = new Vector3(-0.4341624f, -0.7519927f);
                fenceObject.transform.localRotation = Quaternion.Euler(0f, 0f, -120f);
                break;
            case FenceLocation.LowerLeft:
                fenceObject.transform.localPosition = new Vector3(0.4341624f, 0.7519927f);
                fenceObject.transform.localRotation = Quaternion.Euler(0f, 0f, -120f);
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
public enum FenceLocation
{
    UpperLeft = 0,
    Upper = 1,
    UpperRight = 2,
    LowerLeft = 3,
    Lower = 4,
    LowerRight = 5
}