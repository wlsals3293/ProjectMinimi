using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource audioSource = null;

    [SerializeField]
    private AudioClip footstepSound1;

    [SerializeField]
    private AudioClip footstepSound2;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFootStepSound1()
    {
        audioSource.PlayOneShot(footstepSound1);
    }

    public void PlayFootStepSound2()
    {
        audioSource.PlayOneShot(footstepSound2);
    }
}
