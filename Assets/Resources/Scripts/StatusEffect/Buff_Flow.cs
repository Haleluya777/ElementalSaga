using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Flow : StatusEffectBase
{
    public Buff_Flow(float duration, Unit target, string _effectName) : base(duration, target)
    {
        effectName = _effectName;
    }

    public override void ApplyEffect()
    {

    }

    public override void RemoveEffect(bool isRefresh = false)
    {

    }
}
