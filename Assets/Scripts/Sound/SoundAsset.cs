using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[CreateAssetMenu(menuName = "Sound Asset")]
public class SoundAsset : ScriptableObject
{
    public SoundType soundType;

    public AudioClip clip;

    public bool loop = false;

    [Range(0f, 1f)]
    public float volume = 1f;

}
