using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public event Action<ISkillCaster> DeadEvent;

    public void ReportDead(ISkillCaster attacker)
    {
        DeadEvent?.Invoke(attacker);
    }
}
