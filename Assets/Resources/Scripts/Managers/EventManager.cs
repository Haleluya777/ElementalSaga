using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public event Action<ISkillCaster> DeadEvent;
    public event Action SelectDoor;
    public event Action ReleaseAllRelicButton; //보상 상자에서 유물을 선택할 때, 실행할 이벤트
    public event Action ReleaseAllProductButton; //상점에서 유물을 구입한 뒤 실행할 이벤트.
    public event Action<string[]> InitChar;

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
    }

    public void InitDialogueCharacter(string[] name)
    {
        Debug.Log("등장인물 초기화");
        InitChar?.Invoke(name);
    }
}
