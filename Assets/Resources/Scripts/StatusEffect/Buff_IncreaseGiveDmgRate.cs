using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_IncreaseGiveDmgRate : StatusEffectBase
{
    private int rate;

    public Buff_IncreaseGiveDmgRate(float duration, Unit target, int _rate, string _effectName) : base(duration, target)
    {
        effectName = _effectName;
        this.rate = _rate;
    }

    public override void ApplyEffect()
    {
        target.GivingDmgRate = rate;
    }

    public override void RemoveEffect(bool isRefresh = false)
    {
        target.GivingDmgRate = 0;
    }
}
