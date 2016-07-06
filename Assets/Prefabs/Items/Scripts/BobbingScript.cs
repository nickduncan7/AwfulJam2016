using UnityEngine;
using System.Collections;

public class BobbingScript : MonoBehaviour {
    private float period = 0.6f;

    private Transform _b;
    private Transform b
    {
        get
        {
            if (_b == null)
                _b = transform.FindChild("B");

            return _b;
        }
    }

    private Transform _a;
    private Transform a
    {
        get
        {
            if (_a == null)
                _a = transform.FindChild("A");

            return _a;
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        var theta = Time.time / period;
        var t = (Mathf.Sin(theta) + 1) * 0.5f;

        transform.FindChild("Item").transform.position = Vector3.Lerp(a.transform.position, b.transform.position, t);
        transform.FindChild("Item").transform.Rotate(Vector3.forward, 26f * Time.deltaTime);
    }
}
