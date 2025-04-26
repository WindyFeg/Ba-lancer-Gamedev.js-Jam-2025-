using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;

    public void PlayGetHit()
    {
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    public void PlayHit()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }

    public void PlayClick()
    {
        audioSource.clip = audioClips[2];
        audioSource.Play();
    }

    public void PlayEDie()
    {
        audioSource.clip = audioClips[3];
        audioSource.Play();
    }

    public void PlayLose()
    {
        audioSource.clip = audioClips[4];
        audioSource.Play();
    }
}
