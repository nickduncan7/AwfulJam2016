using UnityEngine;
using System.Collections;

public class ICharacterScript : MonoBehaviour {

    [HideInInspector]
    public Coordinate currentLocation;
    [HideInInspector]
    public string Name;

    public int Initiative;
    public int AttackStat;
    public int DefenseStat;
    public int MovementStat;
}
