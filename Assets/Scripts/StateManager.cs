using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using static UnityEngine.GraphicsBuffer;

public class StateManager : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject modelTarget;

    [SerializeField] private Transform cam;
    
    
    [SerializeField] private Button startButton;

    [SerializeField] private GPSController gps;

    [SerializeField] private VideoPlayer memory;
    [SerializeField] private VideoPlayer concert;
    [SerializeField] private AudioSource soundscape;
    [SerializeField] private ParticleSystem confetti;


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
        concert.loopPointReached += concertDone;
        memory.loopPointReached += memoryDone;
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
                if (!startButton.isActiveAndEnabled) { 
                    memory.Stop();
                    concert.Stop();
                    soundscape.Stop();
                    confetti.Stop();
                    arrow.gameObject.SetActive(true);
                }
                break;

            case State.InRange:
                arrow.gameObject.SetActive(true);
                modelTarget.gameObject.SetActive(true);
                break;

            case State.Playing:
                arrow.gameObject.SetActive(false);
                //check length of video, enable/disable depending on how far in
                if(memory.time > 24.0)
                {
                    concert.Play();
                    confetti.Play();
                }
                break;

            case State.Done:
                //remove bench from bench list / mark as visited
                break;
        }
    }

    public void startGame()
    {
        startButton.gameObject.SetActive(false);
        currentState = State.Locating;
    }

    public void playVideo()
    {
        currentState = State.Playing;
        memory.Play();
        soundscape.Play();
        concert.loopPointReached += concertDone;
        memory.loopPointReached += memoryDone;
    }

    public void concertDone(VideoPlayer vp)
    {
        concert.gameObject.SetActive(false);
    }
    public void memoryDone(VideoPlayer vp)
    {
        gps.GPS_Allowed[gps.getClosestBenchIndex()] = false;
        memory.gameObject.SetActive(false);
        modelTarget.SetActive(false);
        currentState = State.Locating;
    }
}
