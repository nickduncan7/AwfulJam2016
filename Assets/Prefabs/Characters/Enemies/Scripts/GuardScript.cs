using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;

public class GuardScript : ICharacterScript
{
    public string FullName
    {
        get { return "Guard " + Name; }
    }

    public GameObject target;

    // Use this for initialization
    void Awake()
    {
        UnitReady();
        Name = GameObjects.GameManager.GetGuardName();
        NameCanvas.transform.FindChild("NamePlate").GetComponent<Text>().text = FullName;

        var substanceMaterial =
            transform.FindChild("Base_Character").FindChild("Cylinder_001").GetComponent<SkinnedMeshRenderer>().material as ProceduralMaterial;

        substanceMaterial.SetProceduralEnum("ShoeSize", Random.Range(0, 3));
        substanceMaterial.SetProceduralEnum("Hair", Random.Range(3, 5));
        substanceMaterial.SetProceduralEnum("HairColor", Random.Range(0, 3));
        substanceMaterial.SetProceduralEnum("FacialHairStyle", Random.Range(0, 5));

        substanceMaterial.SetProceduralBoolean("Armband", true);
        substanceMaterial.SetProceduralBoolean("Coat", true);
        substanceMaterial.SetProceduralEnum("SkinTone", 1);

        Random.seed = Random.Range(0, 10000).GetHashCode();

        substanceMaterial.RebuildTextures();

    }

    void LateUpdate()
    {
        UpdateNamePlate();
    }
}
