using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private PencilStageManager _pencilStageManager;
    [Inject] private GlassStageManager _glassStageManager;
    private IStageManager _stageManager;

    [SerializeField] private Transform[] cameraStageTransform;
    private Camera _camera;
    private int _currentStageIndex = 0;

    private void Start()
    {
        Application.targetFrameRate = 60;
        
        _camera = Camera.main;
        _stageManager = _pencilStageManager;
        Initialize();
    }

    private void Initialize()
    {
        MoveCameraToNextStage();
        if (_currentStageIndex < 2) _stageManager.StageCompleted += OnStageComplete;
        _stageManager.Initialize();
    }

    private void OnStageComplete()
    {
        _currentStageIndex++;
        if (_currentStageIndex == 1)
            _stageManager = _glassStageManager;

        Initialize();
    }


    private void MoveCameraToNextStage()
    {
        if (_currentStageIndex < cameraStageTransform.Length)
        {
            var targetPosition = cameraStageTransform[_currentStageIndex];
            Debug.Log(targetPosition);
            _camera.transform.DOMove(targetPosition.position, 1).SetDelay(1);
            _camera.transform.DORotate(targetPosition.eulerAngles, 1).SetDelay(1);
        }
        else
        {
            Debug.LogWarning("No more camera stage positions!");
        }
    }
}