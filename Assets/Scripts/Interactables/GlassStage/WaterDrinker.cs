using UnityEngine;
using Zenject;

public class WaterDrinker : MonoBehaviour
{
    [Inject] private GlassStageManager _glassStageManager;
    public Transform waterPosition;
    public ParticleSystem waterDripping;

    private void OnMouseDown()
    {
        _glassStageManager.HandleWaterDrinkerClicked(waterDripping);
    }
}