using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class SoundManager : BaseManager<SoundManager>
{
    [Tooltip("���� ����� �ͼ�")]
    [SerializeField]
    private AudioMixer mixer;

    /// <summary>
    /// ���� ����
    /// </summary>
    //private float masterVolume = 1.0f;

    /// <summary>
    /// ������� ����
    /// </summary>
    //private float bgmVolume = 1.0f;

    /// <summary>
    /// ȿ���� ����
    /// </summary>
    //private float sfxVolume = 1.0f;


    private AudioSource musicSource;

    private AudioSource sfxSource;

    private AudioSource voiceSource;

    private AudioSource ambientSource;

    private AudioSource uiSource;



    private Transform sourceHolder;

    public SoundAsset startingBgmSA;


    //private LinkedList<SoundProvider> providers = new LinkedList<SoundProvider>();


    public UnityAction<float> onBgmVolumeChanged;
    public UnityAction<float> onSfxVolumeChanged;



    public void Initialize()
    {
        InitialzeSources();
        Play(startingBgmSA);
    }

    public void Play(SoundAsset sound)
    {
        if (sound == null)
        {
            Debug.LogWarning("���� ���� ����!");
            return;
        }

        AudioSource source;

        switch (sound.soundType)
        {
            case SoundType.Music:
                source = musicSource;
                break;

            case SoundType.PlayerVoice:
                source = voiceSource;
                break;

            case SoundType.Ambient:
                source = ambientSource;
                break;

            case SoundType.Sfx:
                sfxSource.PlayOneShot(sound.clip, sound.volume);
                return;

            case SoundType.UI:
                uiSource.PlayOneShot(sound.clip, sound.volume);
                return;

            default:
                return;
        }

        if (source == null)
            return;

        source.clip = sound.clip;
        source.loop = sound.loop;
        source.volume = sound.volume;
        source.Play();
    }

    public void Play(SoundAsset sound, SoundProvider provider)
    {
        if (sound == null)
        {
            Debug.LogWarning("���� ���� ����!");
            return;
        }
        if (provider == null)
        {
            Debug.LogWarning("����� �ҽ� ����!");
            return;
        }

        provider.Source.clip = sound.clip;
        provider.Source.loop = sound.loop;
        provider.Source.volume = sound.volume;
        provider.Source.Play();
    }

    public void PlayOneShot(SoundAsset sound, SoundProvider provider)
    {
        if (sound == null)
        {
            Debug.LogWarning("���� ���� ����!");
            return;
        }
        if (provider == null)
        {
            Debug.LogWarning("����� �ҽ� ����!");
            return;
        }

        provider.Source.PlayOneShot(sound.clip, sound.volume);
    }

    public void Mute()
    {
        mixer.SetFloat("MasterVolume", -80f);
    }


    private void InitialzeSources()
    {
        if (sourceHolder != null)
            return;

        GameObject holder = ResourceManager.Instance.CreatePrefab("SoundSources");
        DontDestroyOnLoad(holder);
        sourceHolder = holder.transform;

        AudioSource[] sources = holder.GetComponents<AudioSource>();
        musicSource = sources[0];
        sfxSource = sources[1];
        voiceSource = sources[2];
        ambientSource = sources[3];
        uiSource = sources[4];
    }
}
