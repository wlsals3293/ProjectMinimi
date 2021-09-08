using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource audioSource = null;

    [SerializeField]
    private SoundAsset footstepSound1;

    [SerializeField]
    private SoundAsset footstepSound2;

    [SerializeField]
    private SoundAsset landingSound;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SoundManager.Instance.SourceSetup(footstepSound1, audioSource);
    }

    public void PlayFootStepSound1()
    {
        SoundManager.Instance.Play(footstepSound1, audioSource);
    }

    public void PlayFootStepSound2()
    {
        SoundManager.Instance.Play(footstepSound2, audioSource);
    }

    public void PlayLandingSound()
    {
        SoundManager.Instance.Play(landingSound, audioSource);
    }
}
