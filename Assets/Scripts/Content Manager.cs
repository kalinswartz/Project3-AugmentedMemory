using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ContentManager : MonoBehaviour
{
    [SerializeField] public VideoClip video;
    [SerializeField] public AudioClip soundscape;
    [SerializeField] public VideoClip memorySpeakerClip;
    [SerializeField] public ParticleSystem ps1;
    [SerializeField] public ParticleSystem ps2;
    
    void Update()
    {
        VideoPlayer memorySpeakerObject = GameObject.Find("MemoryVideo").GetComponent<VideoPlayer>();
        memorySpeakerObject.clip = memorySpeakerClip;

        AudioSource soundscapeSource = AudioSource.FindObjectOfType<AudioSource>();
        soundscapeSource.clip = soundscape;

        VideoPlayer videoPanelObject = GameObject.Find("VideoPanel").GetComponent<VideoPlayer>();
        videoPanelObject.clip = video;

    }
}
