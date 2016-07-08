using UnityEngine;
using System.Collections;

public class FenceScript : MonoBehaviour {

    public Coordinate betweenA;
    public Coordinate betweenB;

    private GridGeneratorScript gridGenerator
    {
        get { return transform.parent.GetComponent<GridGeneratorScript>(); }
    }

    void Start()
    {
        betweenA = new Coordinate(-1, -1);
        betweenB = new Coordinate(-1, -1);
    }
}
