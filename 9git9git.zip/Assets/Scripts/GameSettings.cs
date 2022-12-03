using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;
using TMPro;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown UI_Display_Resolution;
    [SerializeField] private TMP_Dropdown UI_Display_Screen;
    [SerializeField] private TMP_Dropdown UI_Display_GraphicQuality;
    [SerializeField] private Slider UI_Display_Brightness;

    [SerializeField] private Slider UI_Sound_Master;
    [SerializeField] private Slider UI_Sound_BGM;
    [SerializeField] private Slider UI_Sound_AMB;
    [SerializeField] private Slider UI_Sound_SFX;

    private float temp_Timescale = 1.0f;

    private void Start()
    {
        
    }

    public void Display_SetResolution()
    {
        int value = UI_Display_Resolution.value;

        PlayerPrefs.SetInt("Setting_Display_Resolution", value);
        if (value == 0)
        {
            Screen.SetResolution(2560, 1440, Screen.fullScreenMode);
        }
        else if (value == 1)
        {
            Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
        }
        else if (value == 2)
        {
            Screen.SetResolution(1280, 720, Screen.fullScreenMode);

        }
    }

    public void Display_SetScreen()
    {
        int value = UI_Display_Screen.value;

        PlayerPrefs.SetInt("Setting_Display_Screen", value);
        if(value == 0)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else if(value == 1)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if(value == 2)
        {
            Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
        }
        else if(value == 3)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    public void Display_SetGraphicQuailty()
    {
        var value = UI_Display_Resolution.value;
        PlayerPrefs.SetInt("Setting_Display_GraphicQuality", value);
        QualitySettings.SetQualityLevel(value);
    }

    public void Display_SetBrightness()
    {
        var value = UI_Display_Brightness.value;
        PlayerPrefs.SetFloat("Setting_Display_Brightness", value);
        Screen.brightness = (value + 1) / 2f;
    }

    public void Sound_SetMasterVolume()
    {
        var value = UI_Sound_Master.value;
        PlayerPrefs.SetFloat("Setting_Sound_Master", value);
        var master = FMODUnity.RuntimeManager.GetVCA("vca:/Master");
        master.setVolume(DbToLinear(value));
    }

    public void Sound_SetBGM()
    {
        var value = UI_Sound_BGM.value;
        PlayerPrefs.SetFloat("Setting_Sound_BGM", value);
        var bgm = FMODUnity.RuntimeManager.GetVCA("vca:/BGM");
        bgm.setVolume(DbToLinear(value));
    }

    public void Sound_SetAMB()
    {
        var value = UI_Sound_AMB.value;
        PlayerPrefs.SetFloat("Setting_Sound_AMB", value);
        var amb = FMODUnity.RuntimeManager.GetVCA("vca:/AMB");
        amb.setVolume(DbToLinear(value));
    }

    public void Sound_SetSFX()
    {
        var value = UI_Sound_SFX.value;
        PlayerPrefs.SetFloat("Setting_Sound_SFX", value);
        var sfx = FMODUnity.RuntimeManager.GetVCA("vca:/SFX");
        sfx.setVolume(DbToLinear(value));
    }

    private float DbToLinear(float db)
    {
        return db;
    }
}
