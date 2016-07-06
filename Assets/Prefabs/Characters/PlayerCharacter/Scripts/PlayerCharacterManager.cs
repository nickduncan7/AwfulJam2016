using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public class PlayerCharacterManager : MonoBehaviour {
    // Private members
    private bool moving;
    private Coordinate location;
    private float startTime;
    private List<Coordinate> path;
    private Coordinate destination;
    private Animator anim;

    // Public members
    public GameObject CharacterObject;
    public float MoveSpeed = 4f;
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
        var startTile = Grid.GetTileAtCoordinates(Grid.MainSpawn);
        if (startTile != null)
        {
            (Instantiate(CharacterObject, startTile.transform.position, Quaternion.identity) as GameObject)
                .tag = "PlayerCharacter";
            location.q = Grid.MainSpawn.q;
            location.r = Grid.MainSpawn.r;
        }

	    anim = playerCharacter.GetComponent<Animator>();
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
            {
                path = Grid.CalculateRoute(location, dest.Coordinate);

                if (path != null && path.Count > 0)
                {
                    startTime = Time.time;
                    moving = true;
                    destination = path[0];

                    foreach (var node in path)
                    {
                        Grid.GetTileAtCoordinates(node).GetComponent<HexTile>().highlighted = true;
                    }

                    path.Remove(destination);
                }
            }
        }
        else if (moving)
        {
            anim.SetBool("Walking", true);
            var destinationPosition = Grid.GetTileAtCoordinates(destination).transform.position;
            var currentPosition = Grid.GetTileAtCoordinates(location).transform.position;
            var journeyLength = Vector3.Distance(currentPosition, destinationPosition);
            var distCovered = (Time.time - startTime) * MoveSpeed;
            var fracJourney = distCovered / journeyLength;
            
            playerCharacter.transform.position = Vector3.Lerp(currentPosition, destinationPosition, fracJourney);

            playerCharacter.transform.rotation = Quaternion.Lerp(playerCharacter.transform.rotation, Quaternion.LookRotation(destinationPosition - currentPosition), Time.deltaTime * 10f);

            if (Vector3.Distance(playerCharacter.transform.position,
                    destinationPosition) < 0.05f)
            {
                var newLocationScript = Grid.GetTileAtCoordinates(destination).GetComponent<HexTile>();
                newLocationScript.highlighted = false;
                newLocationScript.DestroyContents();
                location = destination;
                startTime = Time.time;

                if (path.Count != 0)
                {
                    destination = path[0];
                    path.Remove(destination);
                }
                else
                {
                    moving = false;
                    anim.SetBool("Walking", false);
                }
            }
        }
    }
}
