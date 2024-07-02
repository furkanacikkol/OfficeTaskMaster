using UnityEngine;
using DG.Tweening;

public class PencilStageManager : IStageManager
{
    private GameObject _pencil, _whiteBoard;
    private Camera _camera;

    private bool _isPencilSelect;

    public event System.Action StageCompleted;
    public void Initialize()
    {
        Debug.Log("Initialize Pencil stage");
        _camera = Camera.main;
    }

    public void Cleanup()
    {
        Debug.Log("Cleanup");
        StageCompleted?.Invoke();
    }

    public void HandlePencilClicked(Pencil pencil)
    {
        _pencil = pencil.gameObject;
        _isPencilSelect = true;
    }

    public void HandleWhiteBoardClicked(WhiteBoard whiteBoard)
    {
        if (!_isPencilSelect) return;
        _whiteBoard = whiteBoard.gameObject;
        MoveCameraToWhiteBoard();
    }

    private void MoveCameraToWhiteBoard()
    {
        var targetPosition = _whiteBoard.transform.position + Vector3.forward * 1.5f;
        var targetRotation = Quaternion.Euler(0, -180, 0);
        _camera.transform.DOMove(targetPosition, 1);
        _camera.transform.DORotateQuaternion(targetRotation, 1)
            .OnComplete(SetupPaint);
    }

    private void SetupPaint()
    {
        _whiteBoard.GetComponentInChildren<PaintOnPlane>().enabled = true;
        _whiteBoard.GetComponent<Collider>().enabled = false;
    }
}