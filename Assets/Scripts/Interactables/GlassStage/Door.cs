using System;
using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    private GameObject _parentObject;

    private void Start()
    {
        _parentObject = transform.parent.gameObject;
    }

    private void OnMouseDown()
    {
        OpenTheDoor();
    }

    private void OpenTheDoor()
    {
        var doorEulerAngles = _parentObject.transform.rotation.eulerAngles;
        doorEulerAngles.y += 75;
        _parentObject.transform.DORotate(doorEulerAngles, 1);
    }
}
