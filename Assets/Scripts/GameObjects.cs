using UnityEngine;

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

    public static GameManagerScript GameManager
    {
        get { return GameObject.Find("/GameManager").GetComponent<GameManagerScript>(); }
    }

    public static CameraControllerScript CameraController
    {
        get { return GameObject.Find("/CameraController").GetComponent<CameraControllerScript>(); }
    }
}

