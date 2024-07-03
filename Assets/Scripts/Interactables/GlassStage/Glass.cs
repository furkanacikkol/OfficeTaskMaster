using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Glass : MonoBehaviour
{
    [Inject] private GlassStageManager _glassStageManager;
    private bool _isDragging;
    private Vector3 _offset, _firstPosition;
    private Camera _camera;

    private Collider _collider;
    private Renderer _renderer;


    public enum TargetType
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
                Debug.Log("Water drinker found");
                transform.DOMove(waterDrinker.waterPosition.position, 1)
                    .OnComplete(() => _glassStageManager.GlassPlacedOnWaterDrinker(this));
            }
            else if (targetType == TargetType.Plant && hit.collider.TryGetComponent<Plant>(out var plant))
            {
                Debug.Log("Plant found");
                transform.DOMove(plant.glassPosition.position, 1).OnComplete(
                    () =>
                    {
                        transform.DORotate(plant.glassPosition.rotation.eulerAngles, 0.5f);
                        plant.WaterPlant();
                    }
                );
            }
            else if (targetType == TargetType.Bin && hit.collider.CompareTag("Bin"))
            {
                Debug.Log("Bin found");
                var binPosition = hit.collider.transform.position;
                transform.DOMove(binPosition + Vector3.up * .25f, 1)
                    .OnComplete(() =>
                    {
                        binPosition.y *= 2f;
                        transform.DOMove(binPosition, 0.5f);
                        transform.DORotate(Vector3.right * 60, 0.5f)
                            .OnComplete(()=>_glassStageManager.Cleanup());
                    });
            }
            else
            {
                Debug.Log("Target not found");
                ResetPosition();
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything");
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