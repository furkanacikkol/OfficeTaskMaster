using DG.Tweening;
using UnityEngine;
using Zenject;

public class Pencil : MonoBehaviour
{
    [Inject] private PencilStageManager _pencilStageManager;
    private Collider _collider;
    private Tweener _pencilAnimation;
    
    public void PreparePencil()
    {
        _collider = GetComponent<Collider>();
        var rotation = transform.rotation.eulerAngles;
        rotation.y = 75;
        _pencilAnimation = transform.DORotate(rotation, 0.25f).SetLoops(-1, LoopType.Yoyo).SetDelay(3);
    }

    private void OnMouseDown()
    {
        _collider.enabled = false;
        _pencilAnimation.Kill();
        _pencilStageManager.HandlePencilClicked();
    }
}