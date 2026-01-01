using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public event Action<ISkillCaster> DeadEvent;
    public event Action SelectDoor;
    public event Action ReleaseAllRelicButton;

    public void ReportDead(ISkillCaster attacker, GameObject deadObj)
    {
        DeadEvent?.Invoke(attacker);
        if (deadObj.tag == "Enemy") GameManager.instance.stageManager.totalCount--;
    }

    public void ReleaseDoor()
    {
        SelectDoor?.Invoke();
    }

    public void ReleaseRelicButton()
    {
        ReleaseAllRelicButton?.Invoke();
        GameManager.instance.canvasManager.amendPanel.SetActive(false);
    }
}
