using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Inject] private UIManager _uiManager;
    private Dictionary<string, AudioClip> _audioClips;
    private AudioSource _audioSource;

    [Inject]
    public void Construct(Dictionary<string, AudioClip> audioClips, AudioSource audioSource)
    {
        this._audioClips = audioClips;
        this._audioSource = audioSource;
    }

    private void Start()
    {
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlaySound(string clipName)
    {
        if (_uiManager.sound == 0) return;
        if (_audioSource.isPlaying && _audioSource.clip != null && _audioSource.clip.name == clipName) return;

        if (!_audioClips.TryGetValue(clipName, out var clip)) return;
        _audioSource.clip = clip;
        _audioSource.Play();
    }
    
    public void StopSound()
    {
        _audioSource.Stop();
    }
}