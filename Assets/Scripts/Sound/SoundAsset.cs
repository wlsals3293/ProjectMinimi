using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Sound Asset")]
public class SoundAsset : ScriptableObject
{
    public SoundType soundType;

    public AudioClip audioClip;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(0, 256)]
    public int priority = 128;

    public bool loop = false;

    public bool is3d = false;

    [Range(0f, 1f)]
    public float spatialBlend = 1f;

    [Range(1f, 500f)]
    public float minDistance = 1f;

    [Range(1f, 500f)]
    public float maxDistance = 300f;
}
