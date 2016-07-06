using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;

public class PlayerCharacterScript : MonoBehaviour
{
    private List<string> names;
    private Vector3 labelPosition;
    private Quaternion labelRotation;

    public GameObject NameCanvas;
    [HideInInspector] public string Name;

    public string FullName
    {
        get { return "Grandpa " + Name; }
    }

	// Use this for initialization
    void Start()
    {
        names = new List<string>
        {
            "Ernest",
            "Albert",
            "Charles",
            "Henry",
            "Norman",
            "Wallace",
            "Richard",
            "Ralph",
            "Percy",
            "Alfred",
            "Harold",
            "Milton",
            "Mortimer",
            "Murray",
            "Stan",
            "Walter",
            "Ben",
            "Edward",
            "Herb",
            "Donald"
        };

        Name = names.OrderBy(x => Guid.NewGuid()).First();
        NameCanvas.transform.FindChild("NamePlate").GetComponent<Text>().text = FullName;
        
    }

    void OnGUI()
    {
        NameCanvas.transform.position = transform.position + (2f * Vector3.up);
        NameCanvas.transform.rotation = Camera.main.transform.rotation;
    }
}
