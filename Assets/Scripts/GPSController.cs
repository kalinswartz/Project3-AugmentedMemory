using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

public class GPSController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI latitudeValue;
    [SerializeField] TextMeshProUGUI longitudeValue;
    [SerializeField] TextMeshProUGUI resultValue;
    [SerializeField] TextMeshProUGUI mapStatus;
    [SerializeField] TextMeshProUGUI construction;
    [SerializeField] private Transform cam;
    [SerializeField] private StateManager sm;

    private float KeyLatitude;
    private float KeyLongitude;
    private List<Vector2> GPS_Points;
    public List<bool> GPS_Allowed;
    private float threshold = 0.00015f; //if location is within range of gps
    public LocationInfo lastLoc;
    

    // Start is called before the first frame update
    void Start()
    {
        construction.gameObject.SetActive(false);
        GPS_Points = new List<Vector2>();
        GPS_Points.Add(new Vector2(30.61101f, -96.337115f)); //aggie park = 0
        GPS_Points.Add(new Vector2(30.6129917f, -96.3404589f));// rudder fountain = 1
        GPS_Points.Add(new Vector2(30.61608f, -96.34135f)); //century tree = 2
        GPS_Points.Add(new Vector2(30.6178027f, -96.3405648f));//h2o fountain = 3
        GPS_Points.Add(new Vector2(30.61854f, -96.33802f)); //langford A = 4
        GPS_Allowed = new List<bool>();
        GPS_Allowed.Add(true);
        GPS_Allowed.Add(true);
        GPS_Allowed.Add(true);
        GPS_Allowed.Add(true);
        GPS_Allowed.Add(true);

        StartCoroutine(GPSLocalization());
    }

    // Update is called once per frame
    void Update()
    {
        bool cont = false;
        for(int i=0; i<GPS_Allowed.Count; i++)
        {
            if (GPS_Allowed[i]) cont = true;
        }

        if (!cont)
        {
            sm.currentState = StateManager.State.Done;
        }
    }

    //threads - parallel execution path
    //in unity: co-routines
    IEnumerator GPSLocalization()
    {
        if (!Input.location.isEnabledByUser) //if app is NOT allowed to access location
        {
            yield break;
        }

        Input.compass.enabled = true;
        Input.location.Start();

        int maxWait = 20;

        while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) //still not running
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            resultValue.text = "Time out | no connection for now";
            yield break; //try again
        }

        else if (Input.location.status == LocationServiceStatus.Failed)
        {
            resultValue.text = "Unable to determine device location";
        }

        else
        {
            // access granted; run REPEATEDLY the routine accessing the GPS data
            InvokeRepeating("ReadGPSData", 0.5f, 1f); //call function every second
        }

    }

    private void ReadGPSData()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            resultValue.text = "";
            //read the data
            //latitudeValue.text = Input.location.lastData.latitude.ToString();
            //longitudeValue.text = Input.location.lastData.longitude.ToString();

            lastLoc = Input.location.lastData;

            for (int i = 0; i < GPS_Points.Count; i++)
            {
                if (Mathf.Abs(GPS_Points[i].x) - threshold <= Mathf.Abs(Input.location.lastData.latitude) &&
                Mathf.Abs(GPS_Points[i].x) + threshold >= Mathf.Abs(Input.location.lastData.latitude) &&
                Mathf.Abs(GPS_Points[i].y) - threshold <= Mathf.Abs(Input.location.lastData.longitude) &&
                Mathf.Abs(GPS_Points[i].y) + threshold >= Mathf.Abs(Input.location.lastData.longitude))
                {

                    mapStatus.text = "At: " + GetBenchName(i);
                    if (i == 0)
                    {
                        construction.gameObject.SetActive(false);
                        sm.currentState = StateManager.State.InRange;
                    }
                    else
                    {
                        construction.gameObject.SetActive(true);
                    }
                }
                else
                {
                    construction.gameObject.SetActive(false);
                    sm.currentState= StateManager.State.Locating;
                    mapStatus.text = "Go to: " + GetBenchName(getClosestBenchIndex());
                }
            }
        }
        else
        {
            resultValue.text = "Connection Stopped";
        }
    }

    private string GetBenchName(int index)
    {
        switch (index)
        {
            case 0:
                return "Aggie Park";
            case 1:
                return "Rudder Fountain";
            case 2:
                return "Century Tree";
            case 3:
                return "H20 Fountain";
            case 4:
                return "LANG-A Patio";
            default:
                return "Unknown Bench";
        }
    }


    private float CalculateDistance(Vector2 location1, LocationInfo location2)
    {
        // Haversine formula calculation
        const float EarthRadius = 6371.0f; // Earth's radius in kilometers

        float lat1 = Mathf.Deg2Rad * location1.x;
        float lon1 = Mathf.Deg2Rad * location1.y;
        float lat2 = Mathf.Deg2Rad * location2.latitude;
        float lon2 = Mathf.Deg2Rad * location2.longitude;

        float dLat = lat2 - lat1;
        float dLon = lon2 - lon1;

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                  Mathf.Cos(lat1) * Mathf.Cos(lat2) *
                  Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);

        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

        float distance = EarthRadius * c;

        return distance;
    }

    private double CalculateAngle(Vector2 location1, LocationInfo location2)
    {
        float lat1 = Mathf.Deg2Rad * location2.latitude;
        float lon1 = Mathf.Deg2Rad * location2.longitude;
        float lat2 = Mathf.Deg2Rad * location1.x;
        float lon2 = Mathf.Deg2Rad * location1.y;
        double x = Math.Cos(lat2 * Mathf.Deg2Rad) * Math.Sin((lon2 - lon1) * Mathf.Deg2Rad);
        double y = Math.Cos(lat1 * Mathf.Deg2Rad) * Math.Sin(lat2 * Mathf.Deg2Rad) - Math.Sin(lat1 * Mathf.Deg2Rad) * Math.Cos(lat2 * Mathf.Deg2Rad) * Math.Cos((lon2 - lon1) * Mathf.Deg2Rad);
        double degrees = Math.Atan2(x, y) * Mathf.Rad2Deg;
        return degrees;
    }

    public int getClosestBenchIndex()
    {
        int closestBenchIndex = 0;
        float closestDistance = CalculateDistance(GPS_Points[0], Input.location.lastData);

        for (int i = 1; i < GPS_Points.Count; i++)
        {
            if (GPS_Allowed[i]) { 
                float distance = CalculateDistance(GPS_Points[i], Input.location.lastData);

                if (distance < closestDistance)
                {
                    closestBenchIndex = i;
                    closestDistance = distance;
                }
            }
        }
        return closestBenchIndex;
    }

    public Quaternion GetNearestBenchRotation()
    {
        int index = getClosestBenchIndex();
        float angleToNorth = Input.compass.trueHeading;
        Quaternion northAngle = Quaternion.Euler(0.0f, cam.rotation.eulerAngles.y - angleToNorth, 0.0f);
        float angleToBench = (float)CalculateAngle(GPS_Points[index], Input.location.lastData);
        Quaternion benchAngle = Quaternion.Euler(0.0f, northAngle.eulerAngles.y + angleToBench, 0.0f);

        latitudeValue.text = angleToNorth.ToString();
        longitudeValue.text = angleToBench.ToString();
        return benchAngle;
    }
}
