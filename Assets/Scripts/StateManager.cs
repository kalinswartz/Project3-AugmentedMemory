using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class StateManager : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject modelTarget;

    [SerializeField] private Transform cam;
    
    [SerializeField] private GPSController gps;
    
    [SerializeField] private Button startButton;
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
        arrow.gameObject.SetActive(false);
        modelTarget.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rotation = gps.GetNearestBenchRotation();
        arrow.transform.rotation = Quaternion.Slerp(arrow.transform.rotation, rotation, Time.deltaTime * 2);
        switch (currentState)
        {
            case State.StartScreen:
                break;
            case State.Locating:
                modelTarget.gameObject.SetActive(true);
                arrow.gameObject.SetActive(true);
                break;
            case State.InRange:
                arrow.gameObject.SetActive(true);
                break;
            case State.Playing:
                arrow.gameObject.SetActive(false);   
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
