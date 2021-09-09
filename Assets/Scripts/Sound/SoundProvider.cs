using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



[RequireComponent(typeof(AudioSource))]
public class SoundProvider : MonoBehaviour
{
    [SerializeField]
    private SoundAsset[] soundAsset;

    private AudioSource source;



    public AudioSource Source { get => source; }


    private void Awake()
    {
        if (source == null)
            source = GetComponent<AudioSource>();
    }

    public void Play(int index = 0)
    {
        if (index < 0 || index >= soundAsset.Length)
        {
            Debug.LogWarning("사운드 에셋 없음!");
            return;
        }

        SoundManager.Instance.Play(soundAsset[index], this);
    }

    public void PlayOneShot(int index = 0)
    {
        if (index < 0 || index >= soundAsset.Length)
        {
            Debug.LogWarning("사운드 에셋 없음!");
            return;
        }

        SoundManager.Instance.PlayOneShot(soundAsset[index], this);
    }


}
