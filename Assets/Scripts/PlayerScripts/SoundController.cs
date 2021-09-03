using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource audioSource = null;

    [SerializeField]
    private AudioClip footstepSound;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootStepSound()
    {
        audioSource.PlayOneShot(footstepSound);
    }
}
