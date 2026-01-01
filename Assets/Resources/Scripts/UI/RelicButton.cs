using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicButton : PoolAble
{
    void OnEnable()
    {
        if (GameManager.instance is not null)
        {
            GameManager.instance.eventManager.ReleaseAllRelicButton -= ReleaseObject;
            GameManager.instance.eventManager.ReleaseAllRelicButton += ReleaseObject;
        }
    }

    void OnDisable()
    {
        if (GameManager.instance is not null) GameManager.instance.eventManager.ReleaseAllRelicButton -= ReleaseObject;
    }
}
