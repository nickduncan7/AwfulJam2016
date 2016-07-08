using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;

public class PlayerCharacterScript : MonoBehaviour
{
    private Vector3 labelPosition;
    private Quaternion labelRotation;

    [HideInInspector]
    public Coordinate currentLocation;

    public GameObject NameCanvas;
    [HideInInspector] public string Name;

    public string FullName
    {
        get { return "Grandpa " + Name; }
    }

	// Use this for initialization
    void Start()
    {
        Name = GameObject.Find("/PlayerManager").GetComponent<PlayerCharacterManager>().GetGrandpaName();
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
            substanceMaterial.SetProceduralEnum("CoatType", Random.Range(0, 5));
            substanceMaterial.SetProceduralEnum("CoatColor", Random.Range(2, 5));
        }

        UnityEngine.Random.seed = Random.Range(0, 10000).GetHashCode();

        substanceMaterial.RebuildTextures();
    }

    void LateUpdate()
    {
        NameCanvas.transform.rotation = Camera.main.transform.rotation;
    }
}
