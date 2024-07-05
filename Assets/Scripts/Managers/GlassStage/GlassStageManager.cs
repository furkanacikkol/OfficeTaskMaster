using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class GlassStageManager : IStageManager
{
    #region Injected Fields

    [Inject] private AudioManager _audioManager;
    [Inject] private UIManager _uiManager;

    #endregion

    #region Public Properties

    public event Action StageCompleted;
    public bool GlassOnWaterDrinker { get; private set; }
    public bool IsGlassFilled { get; private set; }
    public bool IsPlantWatered { get; private set; }

    #endregion

    private Glass _glass;

    public void Initialize()
    {
    }

    public void Cleanup()
    {
        StageCompleted?.Invoke();
        _glass?.Initialize();
    }

    public void GlassPlacedOnWaterDrinker(Glass glass)
    {
        HapticManager.SoftVibrate();
        _uiManager.ShowNextTask();
        GlassOnWaterDrinker = true;
        _glass = glass;
    }

    public void HandleWaterDrinkerClicked(ParticleSystem waterParticle, WaterDrinker waterDrinker)
    {
        if (!GlassOnWaterDrinker) return;
        if (IsGlassFilled) return;

        HapticManager.SuccesVibrate();
        waterParticle.Play();
        _audioManager.PlaySound("WaterSound");
        waterDrinker.GetComponent<Collider>().enabled = false;
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
        HapticManager.SuccesVibrate();
        _glass.transform.DOMove(glassPosition.position, 1);
        _glass.transform.DORotate(glassPosition.rotation.eulerAngles, 1);
        _glass.waterFall.Stop();
        IsPlantWatered = true;

        Cleanup();
    }
}