using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffectBase
{
    public string effectName;
    public float duration;
    public Unit target;

    public StatusEffectBase(float duration, Unit target)
    {
        this.duration = duration;
        this.target = target;
    }

    public abstract void RemoveEffect(bool isRefresh = false);
    public abstract void ApplyEffect();
}
