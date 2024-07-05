using System;
using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    private Dictionary<string, AudioClip> audioClips;
    private AudioSource audioSource;

    [Inject]
    public void Construct(Dictionary<string, AudioClip> audioClips, AudioSource audioSource)
    {
        this.audioClips = audioClips;
        this.audioSource = audioSource;
    }

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(string clipName)
    {
        if (audioSource.isPlaying && audioSource.clip != null && audioSource.clip.name == clipName) return;

        if (!audioClips.TryGetValue(clipName, out var clip)) return;
        audioSource.clip = clip;
        audioSource.Play();
    }
    
    public void StopSound()
    {
        audioSource.Stop();
    }
}