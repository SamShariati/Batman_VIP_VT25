using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip humanWalking;
    public AudioClip caveMusic;
    public AudioClip spiderSound;
    public AudioClip torchSound;
    public AudioClip quakeDoorSound;
    [SerializeField] private AudioSource humanWalkingAudioSource;
    [SerializeField] private AudioSource quakeAudioSource;

    private void Awake()
    {
        instance = this;
    }

    public void PlayWalkingSound()
    {
        if (humanWalkingAudioSource != null && humanWalking != null)
        {
            if (!humanWalkingAudioSource.isPlaying)
            {
                humanWalkingAudioSource.clip = humanWalking;
                humanWalkingAudioSource.Play();
            }
        }
    }

    public void StopWalkingSound()
    {
        if (humanWalkingAudioSource != null && humanWalkingAudioSource.isPlaying)
        {
            humanWalkingAudioSource.Stop();
        }
    }

    public void PlayQuakeSound()
    {
        if (quakeAudioSource != null && quakeDoorSound != null)
        {
            if (!quakeAudioSource.isPlaying)
            {
                quakeAudioSource.clip = humanWalking;
                quakeAudioSource.Play();
            }
        }
    }
    public void StopQuakeSound()
    {
        if (quakeAudioSource != null && quakeAudioSource.isPlaying)
        {
            quakeAudioSource.Stop();
        }
    }
}