using Lofelt.NiceVibrations;
using UnityEngine;
using Zenject;

public class HapticManager : MonoBehaviour
{
    [Inject] private UIManager _uiManager;
    public static int vibration = 1;
    public static void SoftVibrate()
    {
        if (PlayerPrefs.HasKey("Vibration"))
            vibration = PlayerPrefs.GetInt("Vibration");
        else
            PlayerPrefs.SetInt("Vibration", 1);

        if (vibration == 1)
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
       
    }

    public static void SuccesVibrate()
    {
        if (PlayerPrefs.HasKey("Vibration"))
            vibration = PlayerPrefs.GetInt("Vibration");
        else
            PlayerPrefs.SetInt("Vibration", 1);

        if (vibration == 1)
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
       
    }
}