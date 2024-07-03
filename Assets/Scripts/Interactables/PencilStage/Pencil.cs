using UnityEngine;
using Zenject;

public class Pencil : MonoBehaviour
{
    [Inject] private PencilStageManager _pencilStageManager;

    private void OnMouseDown() =>
        _pencilStageManager.HandlePencilClicked(this);
}