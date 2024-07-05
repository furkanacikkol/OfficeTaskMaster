using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class GlassStageManager : IStageManager
{
    [Inject] private AudioManager _audioManager;
    [Inject] private UIManager _uiManager;
    public event Action StageCompleted;
    public bool GlassOnWaterDrinker { get; private set; }
    public bool IsGlassFilled { get; private set; }
    public bool IsPlantWatered { get; private set; }

    private Glass _glass;

    public void Initialize()
    {
        Debug.Log("Initialize Glass stage");
    }

    public void Cleanup()
    {
        StageCompleted?.Invoke();
        _glass?.Initialize();
    }

    public void GlassPlacedOnWaterDrinker(Glass glass)
    {
        _uiManager.ShowNextTask();
        GlassOnWaterDrinker = true;
        _glass = glass;
        Debug.Log("Glass placed on water drinker. Glass stage progressing...");
    }

    public void HandleWaterDrinkerClicked(ParticleSystem waterParticle)
    {
        if (!GlassOnWaterDrinker) return;

        waterParticle.Play();
        _audioManager.PlaySound("WaterSound");
        _glass.GetComponent<Renderer>().material.DOColor(Color.cyan, 2).OnComplete(() =>
        {
            waterParticle.Stop();
            _audioManager.StopSound();
            IsGlassFilled = true;

            Cleanup();
        });
    }

    public void PlantWatered(Transform glassPosition)
    {
        _glass.transform.DOMove(glassPosition.position, 1);
        _glass.transform.DORotate(glassPosition.rotation.eulerAngles, 1);
        _glass.waterFall.Stop();
        IsPlantWatered = true;

        Cleanup();
    }
}