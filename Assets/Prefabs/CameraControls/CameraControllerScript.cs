using UnityEngine;
using System.Collections;
using System;

public class CameraControllerScript : MonoBehaviour {
    // Constants
    float edgeSpeed = 14f;
    float cameraEdge = 0.04f;

    // Member fields
    [HideInInspector]
    public GameObject cameraInstance;
    private bool canMove = true;
    private bool canChangeCamera = true;
    private bool transitioning = false;
    private Vector3 velocity = Vector3.zero;

    // Private members for finishing transitions
    private float transitionTime = 2f;
    private float currentTime;

    public Transform TargetTransform;
    public bool Ready = false;

    [SerializeField]
    public GameObject CameraObject;
    public float DampTime = 0.08f;
    public float SlowDamp = 0.2f;
    public GridGeneratorScript Grid;

    public void SetTarget(Transform target)
    {
        if (TargetTransform == target)
            return;

        if (target)
        {
            canMove = false;
            canChangeCamera = false;
        }
        else
        {
            canMove = true;
            canChangeCamera = true;
        }
        
        TargetTransform = target;
    }

    public void TransitionTo(Transform target)
    {
        Ready = false;
        transitioning = true;
        SetTarget(target);
    }

    // Use this for initialization
    void Start ()
    {
        // Instantiate camera instance
        cameraInstance = Instantiate(CameraObject, Grid.GetTileAtCoordinates(Grid.MainSpawn).transform.position, Quaternion.identity) as GameObject;
        canMove = false;
        canChangeCamera = false;
        Ready = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (transitioning && TargetTransform)
	    {
            currentTime += Time.deltaTime;
            cameraInstance.transform.position = Vector3.SmoothDamp(cameraInstance.transform.position, TargetTransform.position, ref velocity, SlowDamp);
	        if (Vector3.Distance(cameraInstance.transform.position, TargetTransform.position) < Double.Epsilon || currentTime >= transitionTime)
	        {
                currentTime = 0f;
	            transitioning = false;
	            Ready = true;
                SetTarget(null);
	        }
        }
	    else
	    {
	        if (TargetTransform)
	        {
	            cameraInstance.transform.position = Vector3.SmoothDamp(cameraInstance.transform.position, TargetTransform.position, ref velocity, DampTime);
	        }

	        if (Input.GetKeyDown(KeyCode.Q) && canChangeCamera)
	        {
	            canMove = !canMove;
	        }

	        if (Input.GetKeyDown(KeyCode.C))
	        {
	            cameraInstance.transform.position = GameObjects.GameManager.currentUnit.transform.position;
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
}
