using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Plant : MonoBehaviour
{
    [Inject] private GlassStageManager _glassStageManager;
    [Inject] private AudioManager _audioManager;
    public Transform glassPosition, newGlassPosition;

    [SerializeField] private GameObject waterParticleEffect;
    private GameObject _plant;
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _plant = transform.GetChild(1).gameObject;
    }

    public void WaterPlant()
    {
        _audioManager.PlaySound("Watering");
        _plant.transform.DOScale(Vector3.one, 2.5f).OnComplete(
            () =>
            {
                _audioManager.StopSound();
                _glassStageManager.PlantWatered(newGlassPosition);
            }
        );
    }
}