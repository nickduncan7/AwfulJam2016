using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;

public class PlayerCharacterScript : ICharacterScript
{
    public string FullName
    {
        get { return "Grandpa " + Name; }
    }

	// Use this for initialization
    void Awake()
    {
        UnitReady();

        Type = UnitType.Friendly;

        AttackStat = 10;
        DefenseStat = 20;
        Health = 100;
        MovementStat = 4;

        IsPlayer = true;

        Name = GameObjects.GameManager.GetGrandpaName();
        NameCanvas.transform.FindChild("NamePlate").GetComponent<Text>().text = FullName;

        var substanceMaterial =
            transform.FindChild("Base_Character").FindChild("Cylinder_001").GetComponent<SkinnedMeshRenderer>().material as ProceduralMaterial;

        substanceMaterial.SetProceduralEnum("SkinTone", Random.Range(0, 5));
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

        Random.seed = Random.Range(0, 10000).GetHashCode();

        substanceMaterial.RebuildTextures();
    }

    void LateUpdate()
    {
        UpdateNamePlate();
    }
}
