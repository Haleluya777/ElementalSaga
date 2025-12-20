using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBuff_IncreaseDmgRate : StatusEffectBase
{
    private float rate;
    private float increase;
    public DeBuff_IncreaseDmgRate(float duration, float _rate, Unit target, string _effectName) : base(duration, target)
    {
        effectName = _effectName;
        this.rate = _rate;
    }

    public override void ApplyEffect()
    {
        increase = rate;
        target.DmgRate += increase;
    }

    public override void RemoveEffect(bool isRefresh = false)
    {
        target.DmgRate -= increase;
    }
}
