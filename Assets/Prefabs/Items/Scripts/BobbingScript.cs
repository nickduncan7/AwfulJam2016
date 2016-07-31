using UnityEngine;
using System.Collections;

public class BobbingScript : MonoBehaviour {
    private float period = 0.6f;
    private float offset;

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
        offset = Random.Range(0f, 100f);
        transform.FindChild("Item").transform.Rotate(transform.up, Random.Range(0f, 360f));
	}
	
	// Update is called once per frame
	void Update () {
        var theta = Time.time + offset / period;
        var t = (Mathf.Sin(theta) + 1) * 0.5f;

        transform.FindChild("Item").transform.position = Vector3.Lerp(a.transform.position, b.transform.position, t);
        transform.FindChild("Item").transform.Rotate(transform.up, 26f * Time.deltaTime);
    }
}
