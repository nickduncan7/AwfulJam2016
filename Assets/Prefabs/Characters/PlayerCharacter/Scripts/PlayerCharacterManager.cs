using System;
using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class PlayerCharacterManager : MonoBehaviour {
    // Private members
    private List<string> grandpaNames;
    private bool moving;
    private float startTime;
    private List<Coordinate> previousHighlightedPath;
    private List<Coordinate> highlightedPath;
    private List<Coordinate> path;
    private Coordinate destination;
    private Animator anim;
    private List<GameObject> grandpas; 

    // Private members for ray polling
    private float pollInterval = 0.1f;
    private float currentTime;

    // Public members
    public GameObject CharacterObjectPrefab;
    public float MoveSpeed = 4f;
    public GridGeneratorScript Grid;
    public GameObject NameCanvasPrefab;

    private int currentGrandpaIndex = 0;
    private int previousUnitIndex = 0;

    public GameObject currentGrandpa
    {
        get { return grandpas[currentGrandpaIndex]; }
    }

    public GameObject previousUnit
    {
        get { return grandpas[previousUnitIndex]; }
    }

    public Coordinate location
    {
        get { return currentGrandpa.GetComponent<PlayerCharacterScript>().currentLocation; }
    }

    public GameObject GetNextGrandpa()
    {
        Grid.GetTileAtCoordinates(location).GetComponent<HexTile>().highlighted = false;
        currentGrandpa.GetComponent<PlayerCharacterScript>().Active = false;

        previousUnitIndex = currentGrandpaIndex;

        if (currentGrandpaIndex >= grandpas.Count - 1)
            currentGrandpaIndex = 0;
        else
            currentGrandpaIndex++;

        anim = currentGrandpa.GetComponent<Animator>();
        moving = false;

        setupMove();

        currentGrandpa.GetComponent<PlayerCharacterScript>().Active = true;

        updateNameBadges();

        GameObjects.TimeManager.AdvanceHour();

        return currentGrandpa;
    }

    private void updateNameBadges()
    {
        var hud = GameObject.Find("Standard HUD");
        hud.transform.FindChild("CurrentBadge").FindChild("Name").GetComponent<Text>().text = currentGrandpa.GetComponent<PlayerCharacterScript>().Name;
        hud.transform.FindChild("NextBadge").FindChild("Name").GetComponent<Text>().text = previousUnit.GetComponent<PlayerCharacterScript>().Name;
    }

    private void setupMove()
    {
        anim = currentGrandpa.GetComponent<Animator>();
        moving = false;
    }

    public string GetGrandpaName()
    {
        var grandpaName = grandpaNames[0];
        grandpaNames.Remove(grandpaName);
        return grandpaName;
    }

    // Use this for initialization
    void Start()
    {
        grandpaNames = new List<string>
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

        grandpaNames = grandpaNames.OrderBy(x => Guid.NewGuid()).ToList();

        grandpas = new List<GameObject>();

        // First Grandpa
        var startTile = Grid.GetTileAtCoordinates(Grid.MainSpawn);
        if (startTile != null)
        {
            var playerObject = Instantiate(CharacterObjectPrefab, startTile.transform.position, Quaternion.Euler(0,180,0)) as GameObject;
            playerObject.tag = "PlayerCharacter";
            playerObject.GetComponent<PlayerCharacterScript>().currentLocation = Grid.MainSpawn;
            

            var nameCanvas = Instantiate(NameCanvasPrefab, startTile.transform.position + (2f * Vector3.up), Quaternion.identity) as GameObject;
            playerObject.GetComponent<PlayerCharacterScript>().NameCanvas = nameCanvas;
            playerObject.GetComponent<PlayerCharacterScript>().NameCanvas.transform.SetParent(playerObject.transform);
            playerObject.GetComponent<PlayerCharacterScript>().Active = true;

            Grid.GetTileAtCoordinates(Grid.MainSpawn).GetComponent<HexTile>().occupied = true;
            
            grandpas.Add(playerObject);
        }

        // Second Grandpa
        var secondTile = Grid.GetTileAtCoordinates(Grid.SpawnTwo);
        if (secondTile != null)
        {
            var playerObject = Instantiate(CharacterObjectPrefab, secondTile.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
            playerObject.tag = "PlayerCharacter";
            playerObject.GetComponent<PlayerCharacterScript>().currentLocation = Grid.SpawnTwo;

            var nameCanvas = Instantiate(NameCanvasPrefab, secondTile.transform.position + (2f * Vector3.up), Quaternion.identity) as GameObject;
            playerObject.GetComponent<PlayerCharacterScript>().NameCanvas = nameCanvas;
            playerObject.GetComponent<PlayerCharacterScript>().NameCanvas.transform.SetParent(playerObject.transform);
            playerObject.GetComponent<PlayerCharacterScript>().Active = false;

            Grid.GetTileAtCoordinates(Grid.SpawnTwo).GetComponent<HexTile>().occupied = true;

            grandpas.Add(playerObject);
        }

        // Third Grandpa
        var thirdTile = Grid.GetTileAtCoordinates(Grid.SpawnThree);
        if (thirdTile != null)
        {
            var playerObject = Instantiate(CharacterObjectPrefab, thirdTile.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
            playerObject.tag = "PlayerCharacter";
            playerObject.GetComponent<PlayerCharacterScript>().currentLocation = Grid.SpawnThree;

            var nameCanvas = Instantiate(NameCanvasPrefab, thirdTile.transform.position + (2f * Vector3.up), Quaternion.identity) as GameObject;
            playerObject.GetComponent<PlayerCharacterScript>().NameCanvas = nameCanvas;
            playerObject.GetComponent<PlayerCharacterScript>().NameCanvas.transform.SetParent(playerObject.transform);
            playerObject.GetComponent<PlayerCharacterScript>().Active = false;

            Grid.GetTileAtCoordinates(Grid.SpawnThree).GetComponent<HexTile>().occupied = true;

            grandpas.Add(playerObject);
        }

        // Fourth Grandpa
        var fourthTile = Grid.GetTileAtCoordinates(Grid.SpawnFour);
        if (fourthTile != null)
        {
            var playerObject = Instantiate(CharacterObjectPrefab, fourthTile.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
            playerObject.tag = "PlayerCharacter";
            playerObject.GetComponent<PlayerCharacterScript>().currentLocation = Grid.SpawnFour;

            var nameCanvas = Instantiate(NameCanvasPrefab, fourthTile.transform.position + (2f * Vector3.up), Quaternion.identity) as GameObject;
            playerObject.GetComponent<PlayerCharacterScript>().NameCanvas = nameCanvas;
            playerObject.GetComponent<PlayerCharacterScript>().NameCanvas.transform.SetParent(playerObject.transform);
            playerObject.GetComponent<PlayerCharacterScript>().Active = false;

            Grid.GetTileAtCoordinates(Grid.SpawnFour).GetComponent<HexTile>().occupied = true;

            grandpas.Add(playerObject);
        }

        // Fifth Grandpa
        var fifthTile = Grid.GetTileAtCoordinates(Grid.SpawnFive);
        if (fifthTile != null)
        {
            var playerObject = Instantiate(CharacterObjectPrefab, fifthTile.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
            playerObject.tag = "PlayerCharacter";
            playerObject.GetComponent<PlayerCharacterScript>().currentLocation = Grid.SpawnFive;

            var nameCanvas = Instantiate(NameCanvasPrefab, fifthTile.transform.position + (2f * Vector3.up), Quaternion.identity) as GameObject;
            playerObject.GetComponent<PlayerCharacterScript>().NameCanvas = nameCanvas;
            playerObject.GetComponent<PlayerCharacterScript>().NameCanvas.transform.SetParent(playerObject.transform);
            playerObject.GetComponent<PlayerCharacterScript>().Active = false;

            Grid.GetTileAtCoordinates(Grid.SpawnFive).GetComponent<HexTile>().occupied = true;

            grandpas.Add(playerObject);
        }

        setupMove();
        updateNameBadges();
    }
	
	// Update is called once per frame
	void Update()
	{
	    currentTime += Time.deltaTime;

	    if (currentTime >= pollInterval)
	    {
	        if (!moving && !GameObjects.TimeManager.transitioning)
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
                        var tile = Grid.GetTileAtCoordinates(node).GetComponent<HexTile>();
                        tile.highlighted = false;
                        tile.UpdateMaterial();
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
                                var tile = Grid.GetTileAtCoordinates(node).GetComponent<HexTile>();
                                tile.highlighted = true;
                                tile.UpdateMaterial();
                            }
                        }
	                }
	            }
	        }

	        currentTime = 0f;
	    }

        if (!moving && !GameObjects.TimeManager.transitioning && Input.GetKeyDown(KeyCode.Space))
            GetNextGrandpa();

        if (!moving && !GameObjects.TimeManager.transitioning && Input.GetMouseButtonDown(1))
        {
            Grid.GetTileAtCoordinates(location).GetComponent<HexTile>().highlighted = false;
            path = highlightedPath;

            if (path != null && path.Count > 0)
            {
                foreach (var node in path)
                {
                    var tile = Grid.GetTileAtCoordinates(node).GetComponent<HexTile>();
                    tile.highlighted = false;
                    tile.pathHighlighted = true;
                    tile.UpdateMaterial();
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
                currentGrandpa.transform.position = Vector3.Lerp(currentPosition, destinationPosition, fracJourney);
                currentGrandpa.transform.rotation = Quaternion.Lerp(currentGrandpa.transform.rotation,
	                Quaternion.LookRotation(destinationPosition - currentPosition), Time.deltaTime*10f);
	        }

	        if (Vector3.Distance(currentGrandpa.transform.position,
	            destinationPosition) < 0.05f)
	        {
	            var newLocationScript = Grid.GetTileAtCoordinates(destination).GetComponent<HexTile>();
                var oldLocationScript = Grid.GetTileAtCoordinates(location).GetComponent<HexTile>();
                newLocationScript.pathHighlighted = false;
	            newLocationScript.DestroyContents();

                oldLocationScript.occupied = false;

                // Completed journey to tile
                currentGrandpa.GetComponent<PlayerCharacterScript>().currentLocation = destination;

                // Mark occupied
                newLocationScript.occupied = true;

                startTime = Time.time;

	            if (path.Count != 0)
	            {
	                destination = path[0];
	                path.Remove(destination);
	            }
	            else
                {
                    anim.SetBool("Walking", false);
                    GetNextGrandpa();
	            }
	        }
	    }
	}
}
