using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class WaterDrinker : MonoBehaviour
{
    [Inject] private GlassStageManager _glassStageManager;
    [FormerlySerializedAs("waterLocation")] public Transform waterPosition;
    
    //TODO
    //buraya particle efekt eklenecek
    //su sesi eklenecek

    private void OnMouseDown()
    {
        _glassStageManager.HandleWaterDrinkerClicked(); 
    }
}