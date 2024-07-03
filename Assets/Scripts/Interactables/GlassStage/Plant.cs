using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Plant : MonoBehaviour
{
    [Inject] private GlassStageManager _glassStageManager;
    public Transform glassPosition, newGlassPosition;

    private GameObject _plant;
    private Collider _collider;
    
    private void Start()
    {
        _collider = GetComponent<Collider>();
        _plant = transform.GetChild(1).gameObject;
    }

    public void WaterPlant()
    {
        _plant.transform.DOScale(Vector3.one, 1).OnComplete(
            ()=> _glassStageManager.PlantWatered(newGlassPosition));
    }
    
    
}
