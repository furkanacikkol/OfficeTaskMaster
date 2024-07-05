using DG.Tweening;
using UnityEngine;
using Zenject;

public class Plant : MonoBehaviour
{
    #region Injected Fields

    [Inject] private GlassStageManager _glassStageManager;
    [Inject] private AudioManager _audioManager;

    #endregion

    #region Serialized Fields

    public Transform glassPosition, newGlassPosition;
    [SerializeField] private GameObject waterParticleEffect;

    #endregion

    #region Private Fields

    private GameObject _plant;
    private Collider _collider;

    #endregion

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