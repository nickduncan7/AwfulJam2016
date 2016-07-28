using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ICharacterScript : MonoBehaviour {

    private Color notActiveColor = new Color(0.9f, 0.9f, 0.9f);
    private Color activeColor = new Color(1f, 1f, 1f);

    public GameObject NameCanvas;

    [HideInInspector]
    public Coordinate currentLocation;
    [HideInInspector]
    public string Name;

    public int Initiative;
    public int AttackStat = 10;
    public int DefenseStat = 20;
    public int Health = 100;
    public int MovementStat = 100;

    public bool IsPlayer = false;

    public UnitType Type;

    private bool _active;
    public bool Active
    {
        set
        {
            _active = value;

            if (!_active)
            {
                NameCanvas.transform.FindChild("NamePlate").GetComponent<Text>().color = notActiveColor;
                NameCanvas.transform.FindChild("NamePlate").GetComponent<Shadow>().enabled = false;
            }
            else
            {
                NameCanvas.transform.FindChild("NamePlate").GetComponent<Text>().color = activeColor;
                NameCanvas.transform.FindChild("NamePlate").GetComponent<Shadow>().enabled = true;
            }
        }
        get
        {
            return _active;
        }
    }

    public void UnitReady()
    {
        var nameCanvas = Instantiate(GameObjects.GameManager.NameCanvasPrefab, transform.position + (2f * Vector3.up), Quaternion.identity) as GameObject;
        NameCanvas = nameCanvas;
        NameCanvas.transform.SetParent(transform);

        Active = false;
    }

    public void UpdateNamePlate()
    {
        NameCanvas.transform.rotation = Camera.main.transform.rotation;
    }
}


[System.Serializable]
public enum UnitType
{
    None,
    Friendly,
    Enemy
}