using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI locationStatus;
    [SerializeField] public StateManager stateManager;
    [SerializeField] public TextMeshProUGUI startInfo;

    // Start is called before the first frame update
    void Start()
    {
        locationStatus.enabled = false;
        startInfo.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (stateManager.currentState)
        {
            case StateManager.State.StartScreen:
                startInfo.enabled = true;
                break;
            case StateManager.State.Locating:
                startInfo.enabled = false;
                locationStatus.enabled = true;
                break;
            case StateManager.State.InRange:
                
                
                break;
            case StateManager.State.Playing:
                
              
                break;
            case StateManager.State.Done:
                
               
                break;
        }
    }
}
