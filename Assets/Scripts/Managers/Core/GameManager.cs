using DG.Tweening;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    #region Injected Fields

    [Inject] private UIManager _uiManager;
    [Inject] private PencilStageManager _pencilStageManager;
    [Inject] private GlassStageManager _glassStageManager;
    [Inject] private AudioManager _audioManager;

    #endregion

    #region Private Fields

    private IStageManager _stageManager;
    [SerializeField] private Transform[] cameraStageTransform;
    private Camera _camera;
    private int _currentStageIndex = 0;

    #endregion

    private void Start()
    {
        Application.targetFrameRate = 60;

        _camera = Camera.main;
        _stageManager = _pencilStageManager;
    }

    public void Initialize(bool isGameStart = true)
    {
        if (!isGameStart)
        {
            _uiManager.ShowUI(UIManager.UIScreen.Game);
        }

        MoveCameraToNextStage();
        if (_currentStageIndex < 2) _stageManager.StageCompleted += OnStageComplete;
        _stageManager.Initialize();
    }

    private void OnStageComplete()
    {
        _currentStageIndex++;
        _audioManager.PlaySound("Success");
        if (_currentStageIndex == 1)
            _stageManager = _glassStageManager;

        Initialize();
    }


    private void MoveCameraToNextStage()
    {
        if (_currentStageIndex < cameraStageTransform.Length)
        {
            var targetPosition = cameraStageTransform[_currentStageIndex];
            _camera.transform.DOMove(targetPosition.position, 1).SetDelay(1);
            _camera.transform.DORotate(targetPosition.eulerAngles, 1).SetDelay(1)
                .OnComplete(_uiManager.ShowNextTask);
        }
    }
}