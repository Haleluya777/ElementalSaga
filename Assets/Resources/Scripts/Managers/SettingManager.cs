using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class SettingManager : MonoBehaviour, IDataInitializeable
{
    private List<Resolution> Resolutions = new List<Resolution>()
    {
        new Resolution{width = 1280, height = 720},
        new Resolution{width = 1920, height = 1080},
        new Resolution{width = 2560, height = 1440},
    };

    public float bgmVolume;
    public float sfxVolume;

    public bool Windowed;


    public void DataInitialize()
    {
        SettingInit();
    }

    private void SettingInit()
    {
        Windowed = false;
        Screen.SetResolution(Resolutions[0].width, Resolutions[1].height, Windowed);
    }

    public void SetResolution(int resolutionNum)
    {
        Screen.SetResolution(Resolutions[resolutionNum].width, Resolutions[resolutionNum].height, Windowed);
    }

    public void SetWindowMode()
    {
        Screen.fullScreen = Windowed;
    }
}
