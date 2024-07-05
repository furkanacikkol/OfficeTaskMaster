using DG.Tweening;
using UnityEngine;
using Zenject;

public class Glass : MonoBehaviour
{
    #region Injected Fields

    [Inject] private GlassStageManager _glassStageManager;
    [Inject] private AudioManager _audioManager;

    #endregion

    #region Serialized Fields

    public ParticleSystem waterFall;
    [SerializeField] private GameObject door;

    #endregion

    #region Private Fields

    private bool _isDragging;
    private Vector3 _offset, _firstPosition;
    private Camera _camera;
    private Collider _collider;
    private Renderer _renderer;

    #endregion

    private enum TargetType
    {
        WaterDrinker,
        Plant,
        Bin
    }

    public void Initialize()
    {
        _collider.enabled = true;
    }

    private void Start()
    {
        _camera = Camera.main;
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<Renderer>();
    }


    private void OnMouseDown()
    {
        _isDragging = true;
        _offset = transform.position - GetMouseWorldPos();
        _firstPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        if (_isDragging)
        {
            transform.position = GetMouseWorldPos() + _offset;
        }
    }

    private void OnMouseUp()
    {
        _isDragging = false;

        if (_glassStageManager.IsPlantWatered)
            CheckForTarget(TargetType.Bin);
        else if (_glassStageManager.IsGlassFilled)
            CheckForTarget(TargetType.Plant);
        else if (!_glassStageManager.GlassOnWaterDrinker)
            CheckForTarget(TargetType.WaterDrinker);
    }

    private void CheckForTarget(TargetType targetType)
    {
        _collider.enabled = false;
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit))
        {
            if (targetType == TargetType.WaterDrinker &&
                hit.collider.TryGetComponent<WaterDrinker>(out var waterDrinker))
            {
                transform.DOMove(waterDrinker.waterPosition.position, 1)
                    .OnComplete(() => _glassStageManager.GlassPlacedOnWaterDrinker(this));
            }
            else if (targetType == TargetType.Plant && hit.collider.TryGetComponent<Plant>(out var plant))
            {
                transform.DOMove(plant.glassPosition.position, 1).OnComplete(
                    () =>
                    {
                        transform.DORotate(plant.glassPosition.rotation.eulerAngles, 0.5f);
                        _renderer.material.DOColor(Color.white, 1).SetDelay(1);

                        waterFall.Play();
                        plant.WaterPlant();
                    }
                );
            }
            else if (targetType == TargetType.Bin && hit.collider.CompareTag("Bin"))
            {
                var binPosition = hit.collider.transform.position;
                transform.DOMove(binPosition + Vector3.up * .5f, 1)
                    .OnComplete(() =>
                    {
                        HapticManager.SuccesVibrate();
                        _audioManager.PlaySound("Trash");
                        binPosition.y *= 2.25f;
                        transform.DOMove(binPosition, 0.5f);
                        transform.DORotate(Vector3.right * 60, 0.5f)
                            .OnComplete(() =>
                            {
                                _audioManager.StopSound();
                                _glassStageManager.Cleanup();
                                door.GetComponent<Door>().StageInitialize();
                            });
                    });
            }
            else
            {
                ResetPosition();
            }
        }
        else
        {
            ResetPosition();
        }
    }


    private void ResetPosition()
    {
        transform.DOMove(_firstPosition, 0.5f)
            .OnComplete(() => _collider.enabled = true);
    }

    private Vector3 GetMouseWorldPos()
    {
        var mousePoint = Input.mousePosition;
        mousePoint.z = _camera.WorldToScreenPoint(gameObject.transform.position).z;

        return _camera.ScreenToWorldPoint(mousePoint);
    }
}