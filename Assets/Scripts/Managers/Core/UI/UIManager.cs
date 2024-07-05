using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class UIManager : MonoBehaviour
{
    [Inject] private AudioManager _audioManager;
    
    [Header("UI Screen")] 
    [SerializeField] private GameObject home;
    [SerializeField] private GameObject game;
    [SerializeField] private GameObject win;
    [SerializeField] private GameObject taskbar;
    [SerializeField] private TextMeshProUGUI taskText;

    [Header("Settings")] 
    [SerializeField] private VerticalLayoutGroup settingsPanel;
    [SerializeField] private Button settingsButton;
    [SerializeField] private GameObject soundButton;
    [SerializeField] private GameObject hapticButton;

    [Header("Setting Images")] 
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;
    [SerializeField] private Sprite hapticOn;
    [SerializeField] private Sprite hapticOff;

    private Image _soundImage, _hapticImage;

    private readonly string[] _tasks = new[]
    {
        "TOUCH THE PEN",
        "TOUCH THE WHITE BOARD",
        "PAINT THE BOARD BLACK",
        "DRAG THE GLASS TO THE WATER DISPENSER",
        "FILL THE GLASS",
        "WATER THE PLANT",
        "THROW THE GLASS INTO THE TRASH",
        "OPEN THE DOOR"
    };

    [HideInInspector] public int sound = 1;
    private int _currentTaskIndex = -1;
    private int _vibration = 1;
    private bool _settingsOpen = false;


    public enum UIScreen
    {
        Home,
        Game,
        Win
    }

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Vibration"))
        {
            _vibration = PlayerPrefs.GetInt("Vibration");
        }

        if (PlayerPrefs.HasKey("Sound"))
        {
            sound = PlayerPrefs.GetInt("Sound");
            Debug.Log(sound);
        }
    }

    private void Start()
    {
        ShowUI(UIScreen.Home);

        settingsButton.onClick.AddListener(ToggleSettingsPanel);
        soundButton.GetComponent<Button>().onClick.AddListener(ToggleSound);
        hapticButton.GetComponent<Button>().onClick.AddListener(ToggleHaptic);

        _soundImage = soundButton.GetComponent<Image>();
        _hapticImage = hapticButton.GetComponent<Image>();
        _soundImage.sprite = sound == 1 ? soundOn : soundOff;
        _hapticImage.sprite = _vibration == 1 ? hapticOn : hapticOff;

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

        if (_currentTaskIndex < _tasks.Length)
        {
            taskText.transform.parent.DOScale(Vector3.zero, 0.25f)
                .From(Vector3.one)
                .SetLoops(2, LoopType.Yoyo)
                .OnStepComplete(UpdateTaskText);
        }
    }

    private void UpdateTaskText()
    {
        taskText.text = _tasks[_currentTaskIndex];
    }

    #region Event method

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void ToggleSettingsPanel()
    {
        _settingsOpen = !_settingsOpen;
        float targetSpacing = _settingsOpen ? 0 : -250;

        DOTween.To(() => settingsPanel.spacing,
                x => settingsPanel.spacing = x,
                targetSpacing,
                0.25f)
            .SetEase(Ease.OutCubic);

        soundButton.gameObject.SetActive(_settingsOpen);
        hapticButton.gameObject.SetActive(_settingsOpen);
    }

    private void ToggleSound()
    {
        if (sound == 1)
        {
            sound = 0;
            _soundImage.sprite = soundOff;
            _audioManager.StopSound();
        }
        else
        {
            sound = 1;
            _soundImage.sprite = soundOn;
        }

        PlayerPrefs.SetInt("Sound", sound);
    }

    private void ToggleHaptic()
    {
        if (_vibration == 1)
        {
            _vibration = 0;
            _hapticImage.sprite = hapticOff;
        }
        else
        {
            _vibration = 1;
            _hapticImage.sprite = hapticOn;
        }

        PlayerPrefs.SetInt("Vibration", _vibration);
    }

    #endregion
}