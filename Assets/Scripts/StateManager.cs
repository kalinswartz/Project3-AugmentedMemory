using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public GameObject cube;
    public GPSController gps;
    public enum State
    {
        StartScreen,
        Locating,
        InRange,
        Playing,
        Done
    }
    public State currentState;
    // Start is called before the first frame update
    void Start()
    {
        currentState = State.StartScreen;
    }

    // Update is called once per frame
    void Update()
    {
        cube.transform.forward = gps.getVectorToNearestBench();
        switch (currentState)
        {
            case State.StartScreen:
                break;
            case State.Locating:
                break;
            case State.InRange:
                break;
            case State.Playing:
                break;
            case State.Done:
                break;
        }
    }
}
