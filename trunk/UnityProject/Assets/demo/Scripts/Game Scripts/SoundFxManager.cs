using UnityEngine;
using System.Collections;

public class SoundFxManager : MonoBehaviour 
{
    public AudioSource buttonClickSound;
    public AudioSource shootingSound;

    public AudioSource collisionSoundPoker;

    public AudioSource wallCollisionSoundPoker;

    
    public AudioSource themeMusic;

    AudioSource collisionSound;
    AudioSource wallCollisionSound;

    

    void Start()
    {
        collisionSound = collisionSoundPoker;
        wallCollisionSound = wallCollisionSoundPoker;

    }


    internal void PlayWallCollisionSound()
    {
        wallCollisionSound.Play();
    }

    internal void PlayShootingSound()
    {
        shootingSound.Play();

    }

    internal void PlayCollisionSound()
    {
        collisionSound.Play();
    }
}
