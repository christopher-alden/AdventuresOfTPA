using UnityEngine.Audio;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get { return instance; }
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    [SerializeField] private List<AudioClip> BGMSounds;
    [SerializeField] private List<AudioClip> AmbientSounds;
    [SerializeField] private AudioSource BGM;
    [SerializeField] private AudioSource Ambient;
    [SerializeField, Range(0f, 1f)] private float BGMvolume = 0.5f;
    [SerializeField, Range(0f, 1f)] private float AmbientVolume = 0.5f;

    private void Start()
    {
        PlayAudioClip(BGM, BGMSounds, 0, BGMvolume, true);
        PlayAudioClip(Ambient, AmbientSounds, 0, AmbientVolume, true);
    }

    public void PlayAudioClip(AudioSource audioSource, List<AudioClip> audioClips, int clipIndex, float volume, bool loop)
    {
        if (clipIndex >= 0 && clipIndex < audioClips.Count)
        {
            audioSource.clip = audioClips[clipIndex];
            audioSource.volume = volume;
            audioSource.loop = loop;
            audioSource.Play();
        }
    }
    public void PauseBGM()
    {
        BGM.Pause();
    }
    public void PlayBGM()
    {
        BGM.Play();
    }
}
