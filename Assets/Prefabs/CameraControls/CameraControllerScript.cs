using UnityEngine;
using System.Collections;

public class CameraControllerScript : MonoBehaviour {
    // Constants
    float edgeSpeed = 14f;
    float cameraEdge = 0.04f;
    
    // Member fields
    [HideInInspector]
    public GameObject cameraInstance;
    private bool canMove = true;

    [SerializeField]
    public GameObject CameraObject;
    public GridGeneratorScript Grid;

    // Use this for initialization
    void Start ()
    {
        // Instantiate camera instance
        cameraInstance = Instantiate(CameraObject, Grid.GetTileAtCoordinates(Grid.MainSpawn).transform.position, Quaternion.identity) as GameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Q))
	    {
	        canMove = !canMove;
	    }

	    if (canMove)
	    {
	        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x > Screen.width*(1 - cameraEdge))
	        {
	            cameraInstance.transform.Translate(edgeSpeed*(Vector3.right*Time.deltaTime));
	        }
	        else if (Input.GetKey(KeyCode.A) || Input.mousePosition.x < Screen.width*cameraEdge)
	        {
	            cameraInstance.transform.Translate((-edgeSpeed)*(Vector3.right*Time.deltaTime));
	        }

	        if (Input.GetKey(KeyCode.W) || Input.mousePosition.y > Screen.height*(1 - cameraEdge))
	        {
	            cameraInstance.transform.Translate(edgeSpeed*(Vector3.forward*Time.deltaTime));
	        }
	        else if (Input.GetKey(KeyCode.S) || Input.mousePosition.y < Screen.height*cameraEdge)
	        {
	            cameraInstance.transform.Translate((-edgeSpeed)*(Vector3.forward*Time.deltaTime));
	        }
	    }
    }   
}
