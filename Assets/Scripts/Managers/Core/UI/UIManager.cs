using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject home, game, win, taskbar;
    [SerializeField] private TextMeshProUGUI taskText;

    private string[] tasks = new[]
    {
        "Touch the pen",
        "Touch the white board",
        "Paint the board black",
        "Drag the glass to the water dispenser",
        "Fill the glass",
        "Water the plant",
        "Throw the glass into the trash",
        "Open the door"
    };

    private int _currentTaskIndex = -1;

    public enum UIScreen
    {
        Home,
        Game,
        Win
    }

    private void Start()
    {
        ShowUI(UIScreen.Home);
    }

    public void ShowUI(UIScreen screen)
    {
        home.SetActive(false);
        game.SetActive(false);
        win.SetActive(false);

        switch (screen)
        {
            case UIScreen.Home:
                home.SetActive(true);
                break;
            case UIScreen.Game:
                game.SetActive(true);
                taskbar.transform.localScale = Vector3.zero;
                break;
            case UIScreen.Win:
                win.SetActive(true);
                break;
        }
    }

    public void ShowNextTask()
    {
        _currentTaskIndex++;
        if (_currentTaskIndex < 1)
        {
            taskText.transform.parent.DOScale(Vector3.one, 0.25f);
            UpdateTaskText();
            return;
        }

        if (_currentTaskIndex < tasks.Length)
        {
            taskText.transform.parent.DOScale(Vector3.zero, 0.25f).SetLoops(2, LoopType.Yoyo)
                .OnStepComplete(UpdateTaskText);
        }
        else
        {
            Debug.Log("Game win");
        }
    }

    private void UpdateTaskText()
    {
        taskText.text = tasks[_currentTaskIndex];
    }
}