using UnityEngine;

public static class GameObjects
{
    public static GridGeneratorScript GridGenerator
    {
        get { return GameObject.Find("/GridGenerator").GetComponent<GridGeneratorScript>(); }
    }

    public static FenceManagerScript FenceManager
    {
        get { return GameObject.Find("/FenceManager").GetComponent<FenceManagerScript>(); }
    }

    public static TimeManagerScript TimeManager
    {
        get { return GameObject.Find("/TimeManager").GetComponent<TimeManagerScript>(); }
    }

    public static PlayerCharacterManager PlayerCharacterManager
    {
        get { return GameObject.Find("/PlayerManager").GetComponent<PlayerCharacterManager>(); }
    }
}

