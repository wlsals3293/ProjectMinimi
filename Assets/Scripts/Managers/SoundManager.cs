using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : BaseManager<SoundManager>
{
    [Tooltip("종합 볼륨")]
    [Range(0f, 1f)]
    public float masterVolume = 1.0f;

    [Tooltip("배경음악 볼륨")]
    [Range(0f, 1f)]
    public float bgmVolume = 1.0f;

    [Tooltip("효과음 볼륨")]
    [Range(0f, 1f)]
    public float sfxVolume = 1.0f;


    [SerializeField]
    [Range(3, 10)]
    private int sourceCount = 5;

    private AudioSource bgmSource;

    private AudioSource uiSource;

    private AudioSource[] sfxSources;

    private Transform sourceHolder;

    public SoundAsset bgmSA;



    public delegate void VolumeChangeDelegate(float volume);

    public VolumeChangeDelegate onBgmVolumeChanged;
    public VolumeChangeDelegate onSfxVolumeChanged;


    public void Initialize()
    {
        InitialzeSources();

        Play(bgmSA);
    }

    public void Play(SoundAsset sound)
    {
        if (sound == null)
        {
            Debug.LogWarning("사운드 에셋 없음!");
            return;
        }

        switch (sound.soundType)
        {
            case SoundType.BGM:
                bgmSource.volume = sound.volume * bgmVolume * masterVolume;
                bgmSource.clip = sound.audioClip;
                bgmSource.Play();
                break;

            case SoundType.Sfx:
            case SoundType.PlayerVoice:
            case SoundType.Environment:
                int minPriority = 257;
                int minPriorityIndex = -1;
                int sourceIndex = -1;
                bool success = false;

                for (int i = 0; i < sourceCount; i++)
                {
                    if (sfxSources[i].isPlaying)
                    {
                        if (minPriority > sfxSources[i].priority)
                        {
                            minPriority = sfxSources[i].priority;
                            minPriorityIndex = i;
                        }
                        continue;
                    }

                    sourceIndex = i;
                    success = true;
                    break;
                }

                if (!success && minPriority <= sound.priority)
                {
                    sourceIndex = minPriorityIndex;
                }

                if (sourceIndex < 0)
                    return;

                sfxSources[sourceIndex].clip = sound.audioClip;
                sfxSources[sourceIndex].volume = sound.volume * sfxVolume * masterVolume;
                sfxSources[sourceIndex].priority = sound.priority;
                sfxSources[sourceIndex].loop = sound.loop;
                sfxSources[sourceIndex].spatialize = false;

                sfxSources[sourceIndex].Play();

                break;

            case SoundType.UI:
                uiSource.volume = sfxVolume * masterVolume;
                uiSource.PlayOneShot(sound.audioClip, sound.volume);
                break;

            default:
                break;
        }
    }

    public void Play(SoundAsset sound, Vector3 position, Transform parent = null)
    {
        if (sound == null)
        {
            Debug.LogWarning("사운드 에셋 없음!");
            return;
        }

        switch (sound.soundType)
        {
            case SoundType.BGM:
                bgmSource.volume = sound.volume * bgmVolume * masterVolume;
                bgmSource.clip = sound.audioClip;
                bgmSource.Play();
                break;

            case SoundType.Sfx:
            case SoundType.PlayerVoice:
            case SoundType.Environment:
                int minPriority = 257;
                int minPriorityIndex = -1;
                int sourceIndex = -1;
                bool success = false;

                for (int i = 0; i < sourceCount; i++)
                {
                    if (sfxSources[i].isPlaying)
                    {
                        if (minPriority > sfxSources[i].priority)
                        {
                            minPriority = sfxSources[i].priority;
                            minPriorityIndex = i;
                        }
                        continue;
                    }

                    sourceIndex = i;
                    success = true;
                    break;
                }

                if (!success && minPriority <= sound.priority)
                {
                    sourceIndex = minPriorityIndex;
                }

                if (sourceIndex < 0)
                    return;

                sfxSources[sourceIndex].clip = sound.audioClip;
                sfxSources[sourceIndex].volume = sound.volume * sfxVolume * masterVolume;
                sfxSources[sourceIndex].priority = sound.priority;
                sfxSources[sourceIndex].loop = sound.loop;
                sfxSources[sourceIndex].spatialize = false;
                sfxSources[sourceIndex].spatialize = sound.is3d;
                if (sound.is3d)
                {
                    sfxSources[sourceIndex].spatialBlend = sound.spatialBlend;
                    sfxSources[sourceIndex].minDistance = sound.minDistance;
                    sfxSources[sourceIndex].maxDistance = sound.maxDistance;
                }


                sfxSources[sourceIndex].transform.position = position;

                if(parent != null)
                {
                    sfxSources[sourceIndex].transform.SetParent(parent);
                    sfxSources[sourceIndex].transform.localPosition = Vector3.zero;
                }
                else if(sfxSources[sourceIndex].transform.parent != sourceHolder)
                {
                    sfxSources[sourceIndex].transform.SetParent(sourceHolder);
                }


                sfxSources[sourceIndex].Play();

                break;

            case SoundType.UI:
                uiSource.volume = sfxVolume * masterVolume;
                uiSource.PlayOneShot(sound.audioClip, sound.volume);
                break;

            default:
                break;
        }
    }

    public void Play(SoundAsset sound, AudioSource source)
    {
        if (sound == null)
        {
            Debug.LogWarning("사운드 에셋 없음!");
            return;
        }
        if (source == null)
        {
            Debug.LogWarning("오디오 소스 없음!");
            return;
        }

        switch (sound.soundType)
        {
            case SoundType.BGM:
                source.volume = sound.volume * bgmVolume * masterVolume;
                break;

            case SoundType.Sfx:
            case SoundType.PlayerVoice:
            case SoundType.Environment:
            case SoundType.UI:
                source.volume = sound.volume * sfxVolume * masterVolume;
                break;

            default:
                source.volume = sound.volume * masterVolume;
                break;
        }

        source.clip = sound.audioClip;
        source.priority = sound.priority;
        source.loop = sound.loop;
        source.spatialize = sound.is3d;
        if (sound.is3d)
        {
            source.spatialBlend = sound.spatialBlend;
            source.minDistance = sound.minDistance;
            source.maxDistance = sound.maxDistance;
        }

        source.Play();
    }

    public void PlayOneShot(SoundAsset sound, AudioSource source)
    {
        if (sound == null)
        {
            Debug.LogWarning("사운드 에셋 없음!");
            return;
        }
        if (source == null)
        {
            Debug.LogWarning("오디오 소스 없음!");
            return;
        }

        source.PlayOneShot(sound.audioClip, sound.volume);
    }


    public void SourceSetup(SoundAsset sound, AudioSource source)
    {
        if (sound == null)
        {
            Debug.LogWarning("사운드 에셋 없음!");
            return;
        }
        if (source == null)
        {
            Debug.LogWarning("오디오 소스 없음!");
            return;
        }

        switch (sound.soundType)
        {
            case SoundType.BGM:
                source.volume = bgmVolume * masterVolume;
                break;

            case SoundType.Sfx:
            case SoundType.PlayerVoice:
            case SoundType.Environment:
            case SoundType.UI:
                source.volume = sfxVolume * masterVolume;
                break;

            default:
                source.volume = masterVolume;
                break;
        }

        source.clip = sound.audioClip;
        source.priority = sound.priority;
        source.loop = sound.loop;
        source.spatialize = sound.is3d;
        if (sound.is3d)
        {
            source.spatialBlend = sound.spatialBlend;
            source.minDistance = sound.minDistance;
            source.maxDistance = sound.maxDistance;
        }
    }


    private void InitialzeSources()
    {
        // Holder
        GameObject holder = new GameObject("SoundSourceHolder");
        sourceHolder = holder.transform;


        // BGM
        bgmSource = holder.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        // UI
        uiSource = holder.AddComponent<AudioSource>();
        uiSource.playOnAwake = false;



        // SFX
        sfxSources = new AudioSource[sourceCount];

        for (int i = 0; i < sourceCount; i++)
        {
            GameObject source = new GameObject("SfxSource_" + (i + 1));

            source.transform.SetParent(sourceHolder);

            sfxSources[i] = source.AddComponent<AudioSource>();
            sfxSources[i].spatialize = true;
            sfxSources[i].spatialBlend = 1.0f;
            sfxSources[i].playOnAwake = false;
        }
    }
}
