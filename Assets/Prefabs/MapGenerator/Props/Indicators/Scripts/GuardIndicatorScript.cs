using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardIndicatorScript : MonoBehaviour
{
    [SerializeField]
    public Rotation InitialDirection = Rotation.ZeroDegrees;

    [SerializeField]
    public bool Stationary = true;

    [SerializeField]
    public bool PatrolRandomly = false;

    [SerializeField]
    public bool PatrolViaRoute = false;

    [SerializeField]
    public List<GameObject> PatrolRoute;
}
