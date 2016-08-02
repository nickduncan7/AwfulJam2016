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
    private bool unitMoving;
    private float startTime;
    private List<Coordinate> highlightedPath;
    private List<Coordinate> path;
    private Coordinate destination;
    private Animator anim;
    private List<GameObject> units;
    private int currentMoveAvailable;
    private GameObject temporaryHit;

    private List<GameObject> allUnits
    {
        get { return GameObject.FindGameObjectsWithTag("Unit").ToList(); }
    }

    private List<GameObject> grandpas
    {
        get { return allUnits.Where(unit => unit.GetComponent<ICharacterScript>().Type == UnitType.Friendly).ToList(); }
    }

    // Private members for ray polling
    private float pollInterval = 0.18f;
    private float currentTime;

    // Public members
    public GameObject CharacterObjectPrefab;
    public float MoveSpeed = 4f;
    public GridGeneratorScript Grid;
    public GameObject NameCanvasPrefab;
    public GameObject EnemyNameCanvasPrefab;

    public Sprite FilledDocsImage;
    public Sprite FilledGunImage;
    public Sprite FilledLumberImage;
    public Sprite FilledShovelImage;
    public Sprite FilledPickaxeImage;

    [HideInInspector]
    public bool? wonGame = null;

    [HideInInspector]
    public int grandpasSaved = 0;

    [HideInInspector]
    public GameObject currentUnit;


    public Coordinate location
    {
        get { return currentUnit.GetComponent<ICharacterScript>().currentLocation; }
    }

    public void GetNextUnit()
    {
        ready = false;
        if (anim != null) anim.SetBool("Walking", false);

        if (units.Any())
        {
            ICharacterScript unitScript;
            if (currentUnit != null)
            {
                unitScript = currentUnit.GetComponent<ICharacterScript>();
                Grid.GetTileAtCoordinates(location).GetComponent<HexTile>().highlighted = false;
                unitScript.Active = false;
            }

            currentUnit = units[0];
            units.Remove(currentUnit);

            SetupMove();

            if (currentUnit != null)
            {
                unitScript = currentUnit.GetComponent<ICharacterScript>();
                unitScript.Active = true;

                if (unitScript.Type == UnitType.Friendly)
                    GameObjects.AudioManager.PlaySound(SoundType.PlayerTurnFanfare);
            }
        }
        else
        {
            ready = true;
            endTurn();
        }
        ready = true;
    }

    private void UpdateNameBadges()
    {
        if (currentUnit != null && allUnits.Any())
        {
            var currentUnitScript = currentUnit.GetComponent<ICharacterScript>();
            var hud = GameObject.Find("/Standard HUD");
            var currentUnitPanel = hud.transform.FindChild("CurrentUnit");
            var currentUnitName = currentUnitPanel.FindChild("Name").GetComponent<Text>();
            var currentUnitMoves = currentUnitPanel.FindChild("Moves").GetComponent<Text>();
            currentUnitName.text = currentUnit.GetComponent<ICharacterScript>().FullName;
            currentUnitMoves.text = String.Format("{0} out of {1} moves remaining", currentMoveAvailable, currentUnitScript.MovementStat);

            var d = hud.transform.FindChild("NextUnit");
            var e = d.FindChild("Name");
            var f = e.GetComponent<Text>();
            f.text = units.Any() ? string.Format("{0} is next!", units[0].GetComponent<ICharacterScript>().FullName) : "Nobody is next.";
        }
    }

    private void SetupMove()
    {
        if (currentUnit != null)
        {
            anim = currentUnit.GetComponent<Animator>();
            var unitScript = currentUnit.GetComponent<ICharacterScript>();
            unitMoving = false;
            currentMoveAvailable = unitScript.MovementStat;

            GameObjects.CameraController.TransitionTo(currentUnit.transform);

            GameObjects.GridGenerator.GetAccessibleTiles(unitScript.currentLocation, unitScript.Type == UnitType.Enemy, unitScript.MovementStat);
        }
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

    }

    private void endTurn()
    {
        if (ready)
        {
            GameObjects.TimeManager.AdvanceHour();
            RollInitiative();
        }
    }

    public string GetGrandpaName()
    {
        if (grandpaNames == null || grandpaNames.Count == 0)
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
        }

        var grandpaName = grandpaNames[0];
        grandpaNames.Remove(grandpaName);
        return grandpaName;
    }

    public string GetGuardName()
    {
        if (guardNames == null || guardNames.Count == 0)
        {
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
        }

        var guardName = guardNames[0];
        guardNames.Remove(guardName);
        return guardName;
    }

    // Use this for initialization
    void Start()
    {
        // Force to persist
        DontDestroyOnLoad(this);
        wonGame = null;

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

        RollInitiative();
        SetupMove();
        ready = true;
    }

    public void UpdateLocation(int weight)
    {
        if (currentUnit.GetComponent<ICharacterScript>().currentLocation != destination)
        {
            currentUnit.GetComponent<ICharacterScript>().currentLocation = destination;
            currentMoveAvailable -= weight;
        }
    }

    void LateUpdate()
    {
        UpdateNameBadges();
    }

    private bool ready = false;
    // Update is called once per frame
    void Update()
	{
        if (ready && !wonGame.HasValue)
        {
            if (grandpas.Count == 0)
            {
                wonGame = grandpasSaved != 0;

                UnityEngine.SceneManagement.SceneManager.LoadScene("Endgame");
            }

            if (currentMoveAvailable == 0)
            {
                unitMoving = false;
                GetNextUnit();
                foreach (var tile in GameObjects.GridGenerator.tiles)
                {
                    var tileScript = tile.GetComponent<HexTile>();
                    if (tileScript.highlighted || tileScript.pathHighlighted)
                    {
                        tileScript.pathHighlighted = tileScript.highlighted = false;
                        tileScript.UpdateMaterial();
                    }
                }
            }

            if (GameObjects.CameraController.Ready)
            {
                if (currentUnit.GetComponent<ICharacterScript>().Type == UnitType.Enemy)
                {
                    GameObjects.CameraController.SetTarget(currentUnit.transform);
                }

                if (Input.GetKeyDown(KeyCode.Space) && currentUnit.GetComponent<ICharacterScript>().Type == UnitType.Friendly)
                {
                    foreach (var tile in GameObjects.GridGenerator.tiles)
                    {
                        var tileScript = tile.GetComponent<HexTile>();
                        if (tileScript.highlighted || tileScript.pathHighlighted)
                        {
                            tileScript.pathHighlighted = tileScript.highlighted = false;
                            tileScript.UpdateMaterial();
                        }
                    }
                    GetNextUnit();
                }

                if (!allUnits.Any()) return;
                if (currentUnit == null) GetNextUnit();
                if (currentUnit != null)
                {
                    if (ready)
                    {
                        if (!currentUnit.GetComponent<ICharacterScript>().IsPlayer)
                        {
                            GuardMove();
                        }
                        else
                        {
                            PlayerMove();
                        }
                    }
                }
            }
        }
	}

    private void PlayerMove()
    {
        currentTime += Time.deltaTime;
        var locationTile = Grid.GetTileAtCoordinates(location).GetComponent<HexTile>();
        
        if (!unitMoving && !GameObjects.TimeManager.transitioning && Input.GetMouseButtonDown(1))
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
                GameObjects.CameraController.SetTarget(currentUnit.transform);
                unitMoving = true;
            }
        }

        if (currentTime >= pollInterval && !unitMoving)
        {
            if (!unitMoving && !GameObjects.TimeManager.transitioning)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Physics.Raycast(ray, out hit);

                var newPath = new List<Coordinate>();

                if (temporaryHit == null || hit.transform == null || hit.transform.gameObject != temporaryHit)
                foreach (var tile in GameObjects.GridGenerator.tiles)
                {
                    var tileScript = tile.GetComponent<HexTile>();
                    if (tileScript.highlighted)
                    {
                        tileScript.highlighted = false;
                        tileScript.UpdateMaterial();
                    }
                }

                if (hit.transform != null)
                {
                    var dest = hit.transform.GetComponent<HexTile>();

                    if (dest == null)
                        return;
                    
                    if (!GameObjects.GridGenerator.graph.Contains(dest.Coordinate))
                        return;

                    if (hit.transform.gameObject != temporaryHit)
                    {
                        temporaryHit = hit.transform.gameObject;
                        newPath = Grid.CalculateRoute(location, dest.Coordinate, currentUnit.GetComponent<PlayerCharacterScript>().hasGun, currentMoveAvailable);
                        if (newPath == null) return;

                        if (newPath != highlightedPath)
                            highlightedPath = newPath;

                        if (highlightedPath != null && highlightedPath.Count > 0)
                        {
                            locationTile.highlighted = true;
                            locationTile.UpdateMaterial();

                            foreach (var node in highlightedPath)
                            {
                                var tile = Grid.GetTileAtCoordinates(node).GetComponent<HexTile>();
                                if (!tile.highlighted)
                                {
                                    tile.highlighted = true;
                                    tile.UpdateMaterial();
                                }
                            }
                        }
                    }
                }
                else
                {
                    temporaryHit = null;
                }
            }

            currentTime = 0f;
        }

        if (unitMoving)
        {
            anim.SetBool("Walking", true);
            var destinationPosition = Grid.GetTileAtCoordinates(destination).transform.position;
            var currentPosition = Grid.GetTileAtCoordinates(location).transform.position;
            var journeyLength = Vector3.Distance(currentPosition, destinationPosition);
            var distCovered = (Time.time - startTime) * MoveSpeed;
            var fracJourney = Mathf.Clamp(distCovered / journeyLength, 0f, 1f);

            if (destinationPosition != currentPosition)
            {
                currentUnit.transform.position = Vector3.Lerp(currentPosition, destinationPosition, fracJourney);
                currentUnit.transform.rotation = Quaternion.Lerp(currentUnit.transform.rotation,
                    Quaternion.LookRotation(destinationPosition - currentPosition), Time.deltaTime * 10f);
            }

            if (Vector3.Distance(currentUnit.transform.position,
                destinationPosition) < 0.05f)
            {
                var newLocationScript = Grid.GetTileAtCoordinates(destination).GetComponent<HexTile>();
                var oldLocationScript = Grid.GetTileAtCoordinates(location).GetComponent<HexTile>();
                newLocationScript.pathHighlighted = false;
                newLocationScript.UpdateMaterial();

                var unitScript = currentUnit.GetComponent<PlayerCharacterScript>();

                var destroyedTileContents = newLocationScript.DestroyContents();
                switch (destroyedTileContents)
                {
                    case TileContents.Documents:
                        unitScript.hasDocuments = true;
                        break;
                    case TileContents.Gun:
                        unitScript.hasGun = true;
                        break;
                    case TileContents.Lumber:
                        unitScript.hasLumber = true;
                        break;
                    case TileContents.Pickaxe:
                        unitScript.hasPickaxe = true;
                        break;
                    case TileContents.Shovel:
                        unitScript.hasShovel = true;
                        break;
                }

                oldLocationScript.occupied = false;
                oldLocationScript.Occupier = null;
                oldLocationScript.OccupierType = UnitType.None;

                // Completed journey to tile
                UpdateLocation(newLocationScript.Weight);

                if (newLocationScript.EscapeZone && unitScript.hasDocuments && unitScript.hasLumber && unitScript.hasPickaxe && unitScript.hasShovel)
                {
                    grandpasSaved++;
                    Destroy(currentUnit);
                }

                if (newLocationScript.OccupierType == UnitType.Enemy)
                {
                    if (currentUnit.GetComponent<PlayerCharacterScript>().hasGun)
                    {
                        Destroy(newLocationScript.Occupier);
                        newLocationScript.Occupier = null;
                        newLocationScript.OccupierType = UnitType.None;
                    }
                    else
                    {
                        if (!newLocationScript.Safe)
                        {
                            Destroy(currentUnit);
                            GetNextUnit();
                        }
                    }
                }

                // Mark occupied
                newLocationScript.occupied = true;
                newLocationScript.Occupier = currentUnit;
                newLocationScript.OccupierType = UnitType.Friendly;

                var enemies = GameObject.FindGameObjectsWithTag("Unit")
                    .ToList()
                    .Where(unit => unit.GetComponent<ICharacterScript>().Type == UnitType.Enemy);

                GuardScript enemyScript;
                foreach (var enemy in enemies)
                {
                    enemyScript = enemy.GetComponent<GuardScript>();

                    //if (Coordinate.Distance(enemyScript.currentLocation, destination) <= 7)
                    {
                        enemyScript.ScanForPlayers();
                    }
                }

                startTime = Time.time;

                if (path.Count != 0)
                {
                    destination = path[0];
                    path.Remove(destination);
                }
                else
                {
                    anim.SetBool("Walking", false);
                    unitMoving = false;

                    GameObjects.CameraController.SetTarget(null);
                }
            }
        }
    }

    private void GuardMove()
    {
        var guardScript = currentUnit.GetComponent<GuardScript>();
        if (!unitMoving)
        {
            if (guardScript.DestinationCoordinate == null)
            {
                if (guardScript.Wander)
                {
                    Grid.GetAccessibleTiles(location, false, currentMoveAvailable);
                    path = Grid.CalculateRoute(location, Grid.graph.OrderBy(x => Guid.NewGuid()).Take(1).First(), true, currentMoveAvailable);
                    if (path == null) return;
                    destination = path[0];
                    unitMoving = true;
                    return;
                }
                else
                {
                    guardScript.ScanForPlayers();

                    if (guardScript.target == null)
                    {
                        guardScript.DestinationCoordinate = null;
                        GetNextUnit();
                        return;
                    }
                }
            }

            path = Grid.CalculateRoute(location, guardScript.DestinationCoordinate.Value, true, currentMoveAvailable);
            if (path == null)
            {
                GetNextUnit();
                return;
            }
            destination = path[0];
            unitMoving = true;
        }
        else
        {
            anim.SetBool("Walking", true);
            var destinationPosition = Grid.GetTileAtCoordinates(destination).transform.position;
            var currentPosition = Grid.GetTileAtCoordinates(location).transform.position;
            var journeyLength = Vector3.Distance(currentPosition, destinationPosition);
            var distCovered = (Time.time - startTime) * MoveSpeed;
            var fracJourney = Mathf.Clamp(distCovered / journeyLength, 0f, 1f);

            if (destinationPosition != currentPosition)
            {
                currentUnit.transform.position = Vector3.Lerp(currentPosition, destinationPosition, fracJourney);
                currentUnit.transform.rotation = Quaternion.Lerp(currentUnit.transform.rotation,
                    Quaternion.LookRotation(destinationPosition - currentPosition), Time.deltaTime * 10f);
            }

            if (Vector3.Distance(currentUnit.transform.position,
                destinationPosition) < 0.05f)
            {
                var newLocationScript = Grid.GetTileAtCoordinates(destination).GetComponent<HexTile>();
                var oldLocationScript = Grid.GetTileAtCoordinates(location).GetComponent<HexTile>();

                oldLocationScript.occupied = false;
                oldLocationScript.Occupier = null;
                oldLocationScript.OccupierType = UnitType.None;

                // Completed journey to tile
                UpdateLocation(newLocationScript.Weight);

                if (newLocationScript.Coordinate == guardScript.DestinationCoordinate)
                {
                    guardScript.ScanForPlayers();
                    if (guardScript.target == null)
                        guardScript.DestinationCoordinate = null;
                }

                if (newLocationScript.OccupierType == UnitType.Friendly)
                {
                    if (newLocationScript.Occupier.GetComponent<PlayerCharacterScript>().hasGun)
                        Destroy(currentUnit);
                    else
                    {
                        Destroy(newLocationScript.Occupier);
                        newLocationScript.Occupier = null;
                        newLocationScript.OccupierType = UnitType.None;
                    }
                }

                // Mark occupied
                newLocationScript.occupied = true;
                newLocationScript.Occupier = currentUnit;
                newLocationScript.OccupierType = UnitType.Enemy;               

                startTime = Time.time;

                if (path.Count != 0)
                {
                    destination = path[0];
                    path.Remove(destination);
                }
                else
                {
                    if (currentMoveAvailable > 0)
                    {
                        guardScript.ScanForPlayers();
                        if (guardScript.target == null)
                        {
                            guardScript.DestinationCoordinate = null;
                            GetNextUnit();
                            return;
                        }

                        path = Grid.CalculateRoute(location, guardScript.DestinationCoordinate.Value, true, currentMoveAvailable);
                        if (path == null)
                        {
                            GetNextUnit();
                            return;
                        }
                        else
                        {
                            destination = path[0];
                            unitMoving = true;
                            return;
                        }
                    }

                    guardScript.target = null;
                    unitMoving = false;
                    GetNextUnit();
                }
            }
        }
    }
}
