using UnityEngine;
using Zenject;
using System.Collections.Generic;

public class AudioInstaller : MonoInstaller
{
    public AudioSource audioSourcePrefab;
    public AudioManager audioManagerPrefab;
    public List<AudioClip> audioClips;

    public override void InstallBindings()
    {
        var audioClipDict = new Dictionary<string, AudioClip>();
        foreach (var clip in audioClips)
        {
            audioClipDict[clip.name] = clip;
        }
        Container.Bind<AudioSource>().FromNewComponentOnNewGameObject().AsSingle().NonLazy(); 
        Container.Bind<Dictionary<string, AudioClip>>().FromInstance(audioClipDict).AsSingle().NonLazy();
        Container.Bind<AudioManager>().FromComponentInNewPrefab(audioManagerPrefab).AsSingle().NonLazy();
    }
}