using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff_SlowMoveSpeed : StatusEffectBase
{
    private float rate;
    private int decrease;

    public Debuff_SlowMoveSpeed(float duration, float _rate, Unit target, string _effectName) : base(duration, target)
    {
        effectName = _effectName;
        this.rate = _rate;
    }

    public override void ApplyEffect()
    {
        decrease = (int)(target.MoveSpeed * rate / 100);
        target.MoveSpeed -= decrease;
    }

    public override void RemoveEffect(bool isRefresh = false)
    {
        target.MoveSpeed += decrease;
    }
}
