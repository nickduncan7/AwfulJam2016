using UnityEngine;
using System.Collections;

public class TimeManagerScript : MonoBehaviour {
    private Color sunlightColor;
    private Color sunriseColor = new Color(255 / 255, 255 / 132, 0);

    private int dayLength = 14;
    private int dayStart = 6;

    private float rotateSpeed = 24f;
    private float transitionTime;

    private Quaternion startAngle;
    private Quaternion endAngle;
    private float angleDelta;

    [HideInInspector]
    public bool transitioning = false;
    private bool transitionToDay = false;
    private bool transitionToNight = false;

    private bool isDay
    {
        get { return CurrentHour >= dayStart && CurrentHour < dayStart + dayLength; }
    }
    
    public GameObject SolarObject;

    public int CurrentHour;
    private int PreviousHour;

    public void AdvanceHour()
    {
        PreviousHour = CurrentHour;

        if (CurrentHour == 23)
            CurrentHour = 0;
        else
            CurrentHour++;

        UpdateLightSources();
    }

    private void UpdateLightSources()
    {
        transitionTime = Time.time;
        var sunlight = SolarObject.transform.FindChild("Sunlight").GetComponent<Light>();

        if (PreviousHour == dayStart - 1)
        {
            sunlight.color = sunriseColor;
            transitionToDay = true;
        }
        else if (PreviousHour == dayStart + dayLength - 1)
        {
            transitionToNight = true;
        }

        startAngle = SolarObject.transform.rotation;

        angleDelta = 180f;
        if (isDay)
        {
            angleDelta /= dayLength;
        }
        else
        {
            angleDelta /= (24 - dayLength);
        }

        endAngle = Quaternion.Euler(0, 0, startAngle.eulerAngles.z + angleDelta);

        transitioning = true;
    }

	// Use this for initialization
	void Start () {
        CurrentHour = 8;

        float angle = 180f;
        angle /= dayLength;
        angle *= (CurrentHour - dayStart);
        angle = -90f + angle;

        SolarObject.transform.rotation = Quaternion.Euler(0, 0, angle);

        sunlightColor = SolarObject.transform.FindChild("Sunlight").GetComponent<Light>().color;
    }
	
	// Update is called once per frame
	void Update () {
        if (transitioning)
        {

            var anglesCovered = (Time.time - transitionTime) * rotateSpeed;
            var percentComplete = Mathf.Clamp(anglesCovered / angleDelta, 0f, 1f);

            SolarObject.transform.rotation = Quaternion.Slerp(startAngle, endAngle, percentComplete);

            var sunlight = SolarObject.transform.FindChild("Sunlight").GetComponent<Light>();
            var moonlight = SolarObject.transform.FindChild("Moonlight").GetComponent<Light>();

            if (transitionToDay)
            {
                sunlight.color = Color.Lerp(sunriseColor, sunlightColor, percentComplete);
                sunlight.intensity = Mathf.Lerp(0.0f, 0.8f, percentComplete);
                moonlight.intensity = Mathf.Lerp(0.5f, 0.0f, percentComplete);  
            }
            else if (transitionToNight)
            {
                sunlight.color = Color.Lerp(sunlightColor, sunriseColor, percentComplete);
                sunlight.intensity = Mathf.Lerp(0.8f, 0.0f, percentComplete);
                moonlight.intensity = Mathf.Lerp(0.0f, 0.5f, percentComplete);
            }

            if (percentComplete == 1f)
            {
                transitionToDay = false;
                transitionToNight = false;
                transitioning = false;
            }
        }
        
        else
        {
            // TODO: Remove this debugging code
            if (Input.GetKeyDown(KeyCode.T))
                AdvanceHour();
        }
	}
}
