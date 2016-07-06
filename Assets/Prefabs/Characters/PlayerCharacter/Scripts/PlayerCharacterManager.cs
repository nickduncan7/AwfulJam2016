using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;

public class PlayerCharacterManager : MonoBehaviour {
    // Private members
    private bool moving;
    private Coordinate location;
    private float startTime;
    private List<Coordinate> previousHighlightedPath;
    private List<Coordinate> highlightedPath;
    private List<Coordinate> path;
    private Coordinate destination;
    private Animator anim;

    // Private members for ray polling
    private float pollInterval = 0.1f;
    private float currentTime;

    // Public members
    public GameObject CharacterObjectPrefab;
    public float MoveSpeed = 4f;
    public GridGeneratorScript Grid;
    public GameObject NameCanvasPrefab;

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
            var firstPlayerObject = Instantiate(CharacterObjectPrefab, startTile.transform.position, Quaternion.identity) as GameObject;
            firstPlayerObject.tag = "PlayerCharacter";
            location.q = Grid.MainSpawn.q;
            location.r = Grid.MainSpawn.r;

            firstPlayerObject.GetComponent<PlayerCharacterScript>().NameCanvas =
                Instantiate(NameCanvasPrefab, startTile.transform.position, Quaternion.identity) as GameObject;
        }

	    anim = playerCharacter.GetComponent<Animator>();
        moving = false;
    }
	
	// Update is called once per frame
	void Update()
	{
	    currentTime += Time.deltaTime;

	    if (currentTime >= pollInterval)
	    {
	        if (!moving)
	        {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);

                previousHighlightedPath = highlightedPath;

                if (previousHighlightedPath != null && previousHighlightedPath.Count > 0)
                {
                    Grid.GetTileAtCoordinates(location).GetComponent<HexTile>().highlighted = false;
                    foreach (var node in previousHighlightedPath)
                    {
                        Grid.GetTileAtCoordinates(node).GetComponent<HexTile>().highlighted = false;
                    }
                }

                if (hit.transform != null)
	            {
	                var dest = hit.transform.GetComponent<HexTile>();

	                if (dest != null)
	                {
	                    highlightedPath = Grid.CalculateRoute(location, dest.Coordinate);

	                    if (highlightedPath != null && highlightedPath.Count > 0)
	                    {
                            Grid.GetTileAtCoordinates(location).GetComponent<HexTile>().highlighted = true;
                            foreach (var node in highlightedPath)
	                        {
	                            Grid.GetTileAtCoordinates(node).GetComponent<HexTile>().highlighted = true;
	                        }
                        }
	                }
	            }
	        }

	        currentTime = 0f;
	    }

        if (!moving && Input.GetMouseButtonDown(1))
        {
            Grid.GetTileAtCoordinates(location).GetComponent<HexTile>().highlighted = false;
            path = highlightedPath;

            if (path != null)
            {
                foreach (var node in path)
                {
                    Grid.GetTileAtCoordinates(node).GetComponent<HexTile>().highlighted = false;
                    Grid.GetTileAtCoordinates(node).GetComponent<HexTile>().pathHighlighted = true;
                }

                startTime = Time.time;
                destination = path[0];
                moving = true;
            }
        }

	    if (moving)
	    {
	        anim.SetBool("Walking", true);
	        var destinationPosition = Grid.GetTileAtCoordinates(destination).transform.position;
	        var currentPosition = Grid.GetTileAtCoordinates(location).transform.position;
	        var journeyLength = Vector3.Distance(currentPosition, destinationPosition);
	        var distCovered = (Time.time - startTime)*MoveSpeed;
	        var fracJourney = Mathf.Clamp(distCovered/journeyLength, 0f, 1f);

	        if (destinationPosition != currentPosition)
	        {
	            playerCharacter.transform.position = Vector3.Lerp(currentPosition, destinationPosition, fracJourney);
	            playerCharacter.transform.rotation = Quaternion.Lerp(playerCharacter.transform.rotation,
	                Quaternion.LookRotation(destinationPosition - currentPosition), Time.deltaTime*10f);
	        }

	        if (Vector3.Distance(playerCharacter.transform.position,
	            destinationPosition) < 0.05f)
	        {
	            var newLocationScript = Grid.GetTileAtCoordinates(destination).GetComponent<HexTile>();
	            newLocationScript.pathHighlighted = false;
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
