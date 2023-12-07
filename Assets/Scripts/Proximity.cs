using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Proximity : MonoBehaviour
{
    [SerializeField] private Transform cam;

    float distance = 0f;
    VideoPlayer videoplayer;
    // Start is called before the first frame update
    void Start()
    {
        videoplayer = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, cam.position);  
        if (distance < 1)
        {
            distance = 1;
        }
        videoplayer.SetDirectAudioVolume(0, (.25f / distance));
    }
}
