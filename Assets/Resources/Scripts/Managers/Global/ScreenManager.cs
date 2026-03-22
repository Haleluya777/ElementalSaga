using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour, IDataInitializeable
{
    private Resolution[] resolutions =
    {
        new Resolution {width = 1280, height = 720},
        new Resolution {width = 1920, height = 1080},
        new Resolution {width = 2560, height = 1440},
        new Resolution {width = 3840, height = 2160}
    };

    private FullScreenMode[] screenModes = { FullScreenMode.Windowed, FullScreenMode.ExclusiveFullScreen, FullScreenMode.FullScreenWindow };
    private int[] frameRates = { 30, 60, 120, -1 };
    private int currentResolutionNum;
    private int currentFrame;

    public void DataInitialize()
    {
        //currentResolutionNum = resolutions.Length - 1;
        currentFrame = 1;

        Screen.SetResolution(resolutions[0].width, resolutions[0].height, true);

        Screen.fullScreen = false;
    }

    public void SetResolution(int num)
    {
        if (currentResolutionNum == num) return;
        currentResolutionNum = num;

        Debug.Log($"{resolutions[num]}로 해상도 변경.");
        Resolution resolution = resolutions[currentResolutionNum];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFrameRate(int num)
    {
        if (currentFrame == num) return;
        currentFrame = num;

        Debug.Log($"{frameRates[num]}로 프레임 제한 변경.");
        Application.targetFrameRate = frameRates[currentFrame];
    }

    public void SetFullScreen(bool value)
    {
        Screen.SetResolution(resolutions[currentResolutionNum].width, resolutions[currentResolutionNum].height, value);
    }
}
