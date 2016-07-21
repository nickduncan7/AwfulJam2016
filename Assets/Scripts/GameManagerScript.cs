using System;
using UnityEngine;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour {
    // Private members
    private List<string> grandpaNames;
    private List<string> guardNames;
    private bool playerMoving;
    private float startTime;
    private List<Coordinate> previousHighlightedPath;
    private List<Coordinate> highlightedPath;
    private List<Coordinate> path;
    private Coordinate destination;
    private Animator anim;
    private List<GameObject> units;

    private List<GameObject> allUnits
    {
        get { return GameObject.FindGameObjectsWithTag("Unit").ToList(); }
    }

    // Private members for ray polling
    private float pollInterval = 0.1f;
    private float currentTime;

    // Public members
    public GameObject CharacterObjectPrefab;
    public float MoveSpeed = 4f;
    public GridGeneratorScript Grid;
    public GameObject NameCanvasPrefab;


    public GameObject currentUnit;


    public Coordinate location
    {
        get { return currentUnit.GetComponent<ICharacterScript>().currentLocation; }
    }

    public GameObject GetNextUnit()
    {
        if (units.Any())
        {
            if (currentUnit != null)
            {

                Grid.GetTileAtCoordinates(location).GetComponent<HexTile>().highlighted = false;
                currentUnit.GetComponent<ICharacterScript>().Active = false;
            }

            currentUnit = units[0];
            units.Remove(currentUnit);

            SetupMove();

            currentUnit.GetComponent<ICharacterScript>().Active = true;

            UpdateNameBadges();   

        }
        else
        {
            endTurn();
        }

        return currentUnit;
    }

    private void UpdateNameBadges()
    {
        var hud = GameObject.Find("/Standard HUD");
        var a = hud.transform.FindChild("CurrentBadge");
        var b = a.FindChild("Name");
        var c = b.GetComponent<Text>();
        c.text = currentUnit.GetComponent<ICharacterScript>().Name;

        var d = hud.transform.FindChild("NextBadge");
        var e = d.FindChild("Name");
        var f = e.GetComponent<Text>();
        f.text = units.Any() ? units[0].GetComponent<ICharacterScript>().Name : "Nobody";
    }

    private void SetupMove()
    {
        anim = currentUnit.GetComponent<Animator>();
        playerMoving = false;
    }

    private void RollInitiative()
    {
        foreach (var unit in allUnits)
        {
            var unitScript = unit.GetComponent<ICharacterScript>();
            unitScript.Initiative = Dice.Roll(20);
        }

        units = allUnits.OrderByDescending(x => x.GetComponent<ICharacterScript>().Initiative).ToList();

        GetNextUnit();

        UpdateNameBadges();
    }

    private void endTurn()
    {
        GameObjects.TimeManager.AdvanceHour();
        RollInitiative();
    }

    public string GetGrandpaName()
    {
        var grandpaName = grandpaNames[0];
        grandpaNames.Remove(grandpaName);
        return grandpaName;
    }

    public string GetGuardName()
    {
        var guardName = guardNames[0];
        guardNames.Remove(guardName);
        return guardName;
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

        guardNames = new List<string>
        {
            "Wolfgang",
            "Uwe",
            "Gunther",
            "Dieter",
            "Hans",
            "Klaus",
            "Peter",
            "Max",
            "Johann",
            "Tobias",
            "Adolf",
            "Lucas",
            "Alex",
            "Philipp",
            "Frank",
            "Paul"
        };

        guardNames = guardNames.OrderBy(x => Guid.NewGuid()).ToList();

        units = new List<GameObject>();

        // First Grandpa
        var startTile = Grid.GetTileAtCoordinates(Grid.MainSpawn);
        if (startTile != null)
        {
            var playerObject = Instantiate(CharacterObjectPrefab, startTile.transform.position, Quaternion.Euler(0,180,0)) as GameObject;
            playerObject.GetComponent<PlayerCharacterScript>().currentLocation = Grid.MainSpawn;

            Grid.GetTileAtCoordinates(Grid.MainSpawn).GetComponent<HexTile>().occupied = true;
        }

        // Second Grandpa
        var secondTile = Grid.GetTileAtCoordinates(Grid.SpawnTwo);
        if (secondTile != null)
        {
            var playerObject = Instantiate(CharacterObjectPrefab, secondTile.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
            playerObject.GetComponent<PlayerCharacterScript>().currentLocation = Grid.SpawnTwo;

            Grid.GetTileAtCoordinates(Grid.SpawnTwo).GetComponent<HexTile>().occupied = true;
        }

        // Third Grandpa
        var thirdTile = Grid.GetTileAtCoordinates(Grid.SpawnThree);
        if (thirdTile != null)
        {
            var playerObject = Instantiate(CharacterObjectPrefab, thirdTile.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
            playerObject.GetComponent<PlayerCharacterScript>().currentLocation = Grid.SpawnThree;

            Grid.GetTileAtCoordinates(Grid.SpawnThree).GetComponent<HexTile>().occupied = true;
        }

        // Fourth Grandpa
        var fourthTile = Grid.GetTileAtCoordinates(Grid.SpawnFour);
        if (fourthTile != null)
        {
            var playerObject = Instantiate(CharacterObjectPrefab, fourthTile.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
            playerObject.GetComponent<PlayerCharacterScript>().currentLocation = Grid.SpawnFour;

            Grid.GetTileAtCoordinates(Grid.SpawnFour).GetComponent<HexTile>().occupied = true;
        }

        // Fifth Grandpa
        var fifthTile = Grid.GetTileAtCoordinates(Grid.SpawnFive);
        if (fifthTile != null)
        {
            var playerObject = Instantiate(CharacterObjectPrefab, fifthTile.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
            playerObject.GetComponent<PlayerCharacterScript>().currentLocation = Grid.SpawnFive;

            Grid.GetTileAtCoordinates(Grid.SpawnFive).GetComponent<HexTile>().occupied = true;
        }

        StartCoroutine(SetupDelay());
    }

    private IEnumerator SetupDelay()
    {
        yield return new WaitForSeconds(1);
        RollInitiative();
        SetupMove();
        ready = true;
    }


    private bool ready = false;
    // Update is called once per frame
    void Update()
	{
        if (ready)
        {
            currentTime += Time.deltaTime;

            var locationTile = Grid.GetTileAtCoordinates(location).GetComponent<HexTile>();

            if (currentTime >= pollInterval)
            {
                if (!playerMoving && !GameObjects.TimeManager.transitioning)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    Physics.Raycast(ray, out hit);

                    previousHighlightedPath = highlightedPath;

                    if (previousHighlightedPath != null && previousHighlightedPath.Count > 0)
                    {
                        locationTile.highlighted = false;
                        locationTile.UpdateMaterial();

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
                                locationTile.highlighted = true;
                                locationTile.UpdateMaterial();
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

            if (!playerMoving && !GameObjects.TimeManager.transitioning && Input.GetKeyDown(KeyCode.Space))
                GetNextUnit();

            if (!playerMoving && !GameObjects.TimeManager.transitioning && Input.GetMouseButtonDown(1))
            {
                locationTile.highlighted = false;
                locationTile.UpdateMaterial();
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
                    playerMoving = true;
                }
            }

            if (playerMoving)
            {
                anim.SetBool("Walking", true);
                var destinationPosition = Grid.GetTileAtCoordinates(destination).transform.position;
                var currentPosition = Grid.GetTileAtCoordinates(location).transform.position;
                var journeyLength = Vector3.Distance(currentPosition, destinationPosition);
                var distCovered = (Time.time - startTime)*MoveSpeed;
                var fracJourney = Mathf.Clamp(distCovered/journeyLength, 0f, 1f);

                if (destinationPosition != currentPosition)
                {
                    currentUnit.transform.position = Vector3.Lerp(currentPosition, destinationPosition, fracJourney);
                    currentUnit.transform.rotation = Quaternion.Lerp(currentUnit.transform.rotation,
                        Quaternion.LookRotation(destinationPosition - currentPosition), Time.deltaTime*10f);
                }

                if (Vector3.Distance(currentUnit.transform.position,
                    destinationPosition) < 0.05f)
                {
                    var newLocationScript = Grid.GetTileAtCoordinates(destination).GetComponent<HexTile>();
                    var oldLocationScript = Grid.GetTileAtCoordinates(location).GetComponent<HexTile>();
                    newLocationScript.pathHighlighted = false;
                    newLocationScript.UpdateMaterial();
                    newLocationScript.DestroyContents();

                    oldLocationScript.occupied = false;

                    // Completed journey to tile
                    currentUnit.GetComponent<ICharacterScript>().currentLocation = destination;

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
                        GetNextUnit();
                    }
                }
            }
        }
	}
}
