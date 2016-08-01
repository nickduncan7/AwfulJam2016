using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PlayerCharacterScript : ICharacterScript
{
    public bool hasDocuments = false;
    public bool hasGun = false;
    public bool hasLumber = false;
    public bool hasPickaxe = false;
    public bool hasShovel = false;

    public new string FullName
    {
        get
        {
            if (string.IsNullOrEmpty(base.FullName)) base.FullName = "Grandpa " + Name;
            return base.FullName;
        }
    }

	// Use this for initialization
    void Start()
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

        Random.InitState(Random.Range(0, 10000).GetHashCode());

        substanceMaterial.RebuildTextures();
    }

    private void UpdateIcons()
    {
        if (hasDocuments) NameCanvas.transform.FindChild("Icons").FindChild("Documents").GetComponent<Image>().sprite = GameObjects.GameManager.FilledDocsImage;
        if (hasLumber) NameCanvas.transform.FindChild("Icons").FindChild("Lumber").GetComponent<Image>().sprite = GameObjects.GameManager.FilledLumberImage;
        if (hasPickaxe) NameCanvas.transform.FindChild("Icons").FindChild("Pickaxe").GetComponent<Image>().sprite = GameObjects.GameManager.FilledPickaxeImage;
        if (hasShovel) NameCanvas.transform.FindChild("Icons").FindChild("Shovel").GetComponent<Image>().sprite = GameObjects.GameManager.FilledShovelImage;
        if (hasGun) NameCanvas.transform.FindChild("Gun").GetComponent<Image>().color = Color.white;
    }

    void LateUpdate()
    {
        UpdateNamePlate();
        UpdateIcons();
    }
}
