using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Attack : StatusEffectBase
{
    private float rate;
    private float increase;

    public Buff_Attack(float duration, Unit target, int _rate, string _effectName) : base(duration, target)
    {
        base.effectName = _effectName;
        this.rate = _rate;
    }

    public override void ApplyEffect()
    {
        increase = (target.Att * rate) / 100;
        target.Att += (int)increase;
        Debug.Log("버프 받음");
    }

    public override void RemoveEffect(bool isRefresh = false)
    {
        target.Att -= (int)increase;
        Debug.Log($"버프 제거됨 증가량 : {increase}");
    }
}
