using UnityEngine;

public class ItemScript : MonoBehaviour
{

    [SerializeField]
    [HideInInspector]
    public Coordinate Coordinate;

    [HideInInspector]
    public TileContents Type;
}
