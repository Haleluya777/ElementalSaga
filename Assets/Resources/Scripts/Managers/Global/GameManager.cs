using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public EventManager eventManager;
    public CanvasManager canvasManager;

    public SoundManager soundManager;
    public ScreenManager screenManager;
    public SystemDataManager dataManager;

    [Header("사라지면 안되는 UI캔버스")]
    public GameObject settingCanvas;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }

        foreach (var init in GetComponentsInChildren<IDataInitializeable>())
        {
            init.DataInitialize();
        }

        DontDestroyOnLoad(settingCanvas);
    }

    void Update()
    {

    }
}
