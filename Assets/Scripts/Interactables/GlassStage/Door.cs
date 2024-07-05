using DG.Tweening;
using UnityEngine;
using Zenject;

public class Door : MonoBehaviour
{
    #region Injected Fields
    [Inject] private AudioManager _audioManager;
    [Inject] private UIManager _uiManager;
    #endregion

    #region Serialized Fields
    [SerializeField] private Transform winPosition;
    #endregion

    #region Private Fields
    private GameObject _parentObject;
    private Camera _camera;
    private Renderer _renderer;
    #endregion

    private void Start()
    {
        _parentObject = transform.parent.gameObject;
        _camera = Camera.main;

        _renderer = GetComponent<Renderer>();
    }

    private void OnMouseDown()
    {
        OpenTheDoor();
        _renderer.enabled = false;
    }

    private void OpenTheDoor()
    {
        _audioManager.PlaySound("DoorOpening");
        var doorEulerAngles = _parentObject.transform.rotation.eulerAngles;
        doorEulerAngles.y += 75;
        _parentObject.transform.DORotate(doorEulerAngles, 1)
            .OnComplete(
                () =>
                {
                    _uiManager.ShowUI(UIManager.UIScreen.Win);
                    _audioManager.StopSound();
                    _camera.transform.GetChild(0).gameObject.SetActive(true);
                    _camera.transform.GetChild(0).parent = null;
                    _audioManager.PlaySound("Win");
                    HapticManager.SuccesVibrate();

                    _camera.transform.DOMove(winPosition.position, 1).SetDelay(0.5f);
                    _camera.transform.DORotate(winPosition.eulerAngles, 1).SetDelay(0.5f);
                });
    }

    public void StageInitialize()
    {
        var color = _renderer.material.color;
        color.a = 0.5f;
        _renderer.material.color = color;

        _renderer.material.DOColor(new Color(color.r, color.g, color.b, 0f), 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}