using UnityEngine;
using Zenject;

public class WhiteBoard : MonoBehaviour
{
    [Inject] private PencilStageManager _pencilStageManager;

    private void OnMouseDown() =>
        _pencilStageManager.HandleWhiteBoardClicked(this);
}
