using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>

public class ParticleAudioManager : MonoBehaviour 
{
    int numberOfAudioSources = 5;
    public AudioClip soundClip;
    AudioSource[] audioSources;
    int index = 0;

    float minDuration = .05f;
   

    bool play = true;

    void Start()
    {
        for (int i = 0; i < numberOfAudioSources; i++)
        {
            AudioSource temp = gameObject.AddComponent<AudioSource>();
            temp.playOnAwake = false;
            temp.clip = soundClip;
        }

        audioSources = GetComponents<AudioSource>();
    }

    internal void PlayParticleSound()
    {
        if (!play)
            return;
        play = false;
        Invoke("EnablePlay", minDuration);
        audioSources[index++].Play();        

        if (index >= audioSources.Length)
            index = 0;
    }

    void EnablePlay()
    {
        play = true;
    }
}
