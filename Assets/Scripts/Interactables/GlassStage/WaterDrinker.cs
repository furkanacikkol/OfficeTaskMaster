using UnityEngine;
using Zenject;

public class WaterDrinker : MonoBehaviour
{
    #region Injected Fields

    [Inject] private GlassStageManager _glassStageManager;

    #endregion

    #region Serialized Fields

    public Transform waterPosition;
    public ParticleSystem waterDripping;

    #endregion
    private void OnMouseDown()
    {
        _glassStageManager.HandleWaterDrinkerClicked(waterDripping, this);
    }
}