using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private PencilStageManager _pencilStageManager;
    
    private void Start()
    {
        StartPencilStage();
    }

    private void StartPencilStage()
    {
        Debug.Log("Start Pencil Stage");
        _pencilStageManager.gameObject.SetActive(true);
        _pencilStageManager.Initialize();
    }
}
