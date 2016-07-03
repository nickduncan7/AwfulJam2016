using UnityEngine;
using System.Collections;

public class PlayerCharacterManager : MonoBehaviour {
    // Private members
    private bool moving;
    
    // Public members
    public GameObject CharacterObject;
    public int StartQCoordinate;
    public int StartRCoordinate;
    public GridGeneratorScript Grid;

    private GameObject playerCharacter
    {
        get
        {
            return GameObject.FindGameObjectWithTag("PlayerCharacter");
        }
    }

	// Use this for initialization
	void Start()
    {
        var startTile = Grid.GetTileAtCoordinates(StartQCoordinate, StartRCoordinate);
        if (startTile != null)
        {
            (Instantiate(CharacterObject, startTile.transform.position + Vector3.up, Quaternion.identity) as GameObject)
                .tag = "PlayerCharacter";
        }

        moving = false;
    }
	
	// Update is called once per frame
	void Update()
    {
        if (Input.GetMouseButtonDown(1) && !moving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            var dest = hit.transform.GetComponent<HexTile>();

            if (dest != null)
                playerCharacter.transform.position = (Grid.GetTileAtCoordinates(dest.q, dest.r).transform.position + Vector3.up);
        }

    }
}
