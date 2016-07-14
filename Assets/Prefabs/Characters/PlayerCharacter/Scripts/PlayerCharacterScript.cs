using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;

public class PlayerCharacterScript : ICharacterScript
{
    private Color notActiveColor = new Color(0.9f, 0.9f, 0.9f);
    private Color activeColor = new Color(1f, 1f, 1f);

    public GameObject NameCanvas;
    

    public string FullName
    {
        get { return "Grandpa " + Name; }
    }

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

	// Use this for initialization
    void Awake()
    {
        Name = GameObjects.GameManager.GetGrandpaName();

        var nameCanvas = Instantiate(GameObjects.GameManager.NameCanvasPrefab, transform.position + (2f * Vector3.up), Quaternion.identity) as GameObject;
        NameCanvas = nameCanvas;
        NameCanvas.transform.SetParent(transform);

        NameCanvas.transform.FindChild("NamePlate").GetComponent<Text>().text = FullName;

        var substanceMaterial =
            transform.FindChild("Base_Character").FindChild("Cylinder_001").GetComponent<SkinnedMeshRenderer>().material as ProceduralMaterial;

        substanceMaterial.SetProceduralEnum("ShoeSize", Random.Range(0,3));
        substanceMaterial.SetProceduralEnum("ShoeColor", Random.Range(0, 3));
        substanceMaterial.SetProceduralEnum("PantsColor", Random.Range(2, 5));
        substanceMaterial.SetProceduralEnum("ShirtColor", Random.Range(2, 5));
        substanceMaterial.SetProceduralEnum("Hair", Random.Range(0, 5));
        substanceMaterial.SetProceduralEnum("FacialHairStyle", Random.Range(0, 5));

        bool coat = (Random.Range(0, 2) == 1);
        substanceMaterial.SetProceduralBoolean("Coat", coat);
        if (coat)
        {
            substanceMaterial.SetProceduralEnum("Coattype", Random.Range(0, 2));
            substanceMaterial.SetProceduralEnum("CoatColor", Random.Range(2, 5));
        }

        UnityEngine.Random.seed = Random.Range(0, 10000).GetHashCode();

        substanceMaterial.RebuildTextures();

        Active = false;
    }

    void LateUpdate()
    {
        NameCanvas.transform.rotation = Camera.main.transform.rotation;
    }
}
