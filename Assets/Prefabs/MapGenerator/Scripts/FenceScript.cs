using UnityEngine;
using System.Collections;

public class FenceScript : MonoBehaviour {

    [SerializeField]
    public Coordinate betweenA;

    [SerializeField]
    public Coordinate betweenB;

    private GridGeneratorScript gridGenerator
    {
        get { return transform.parent.GetComponent<GridGeneratorScript>(); }
    }
}
