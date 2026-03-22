using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    public static GlobalGameManager instance;

    public SettingManager settingManager;
    //public 

    void Awake()
    {
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
    }
}
