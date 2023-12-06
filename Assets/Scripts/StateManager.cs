using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public GameObject cube;
    public GPSController gps;
    [SerializeField] private Button startButton;
    public GameObject modelTarget;
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
        cube.gameObject.SetActive(false);
        modelTarget.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        cube.transform.forward = Vector3.Normalize(gps.getVectorToNearestBench());
        switch (currentState)
        {
            case State.StartScreen:
                break;
            case State.Locating:
                modelTarget.gameObject.SetActive(true);
                cube.gameObject.SetActive(true);
                break;
            case State.InRange:
                cube.gameObject.SetActive(true);
                break;
            case State.Playing:
                cube.gameObject.SetActive(false);   
                break;
            case State.Done:
                break;
        }
    }

    public void startGame()
    {
        startButton.gameObject.SetActive(false);
        currentState = State.Locating;
    }
}
