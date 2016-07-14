using UnityEngine;
using System.Collections;

public static class Dice
{
    public static int Roll(int sides = 6)
    {
        Random.seed = Random.Range(0, 10000).GetHashCode();
        return Random.Range(1, sides + 1);
    }
}
