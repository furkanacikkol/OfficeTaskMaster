using System;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Door : MonoBehaviour
{
    [Inject] private AudioManager _audioManager;
    [Inject] private UIManager _uiManager;
    private GameObject _parentObject;
    private Camera _camera;
    private void Start()
    {
        _parentObject = transform.parent.gameObject;
        _camera = Camera.main;
    }

    private void OnMouseDown()
    {
        OpenTheDoor();
    }

    private void OpenTheDoor()
    {
        _audioManager.PlaySound("DoorOpening");
        var doorEulerAngles = _parentObject.transform.rotation.eulerAngles;
        doorEulerAngles.y += 75;
        _parentObject.transform.DORotate(doorEulerAngles, 1)
            .OnComplete(
                ()=>
                {
                    _uiManager.ShowUI(UIManager.UIScreen.Win);
                    _audioManager.StopSound();
                    _camera.transform.GetChild(0).gameObject.SetActive(true);
                    _audioManager.PlaySound("Win");
                });
    }
}
