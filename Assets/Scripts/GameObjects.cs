﻿using UnityEngine;

public static class GameObjects
{
    public static GridGeneratorScript GridGenerator
    {
        get { return GameObject.Find("/GridGenerator").GetComponent<GridGeneratorScript>(); }
    }

    public static WallManagerScript WallManager
    {
        get { return GameObject.Find("/WallManager").GetComponent<WallManagerScript>(); }
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
