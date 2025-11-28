using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_MoveSpeed : StatusEffectBase
{
    private float rate;
    private float increase;

    public Buff_MoveSpeed(float duration, Unit target, float _rate, string _effectName) : base(duration, target)
    {
        base.effectName = _effectName;
        this.rate = _rate;
    }

    public override void ApplyEffect()
    {
        increase = (target.MoveSpeed * rate) / 100;
        target.MoveSpeed += (int)increase;
    }

    public override void RemoveEffect()
    {
        target.MoveSpeed -= (int)increase;
        Debug.Log("이동 속도 버프 삭제.");
    }
}
