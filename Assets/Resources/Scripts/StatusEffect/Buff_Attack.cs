using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Attack : StatusEffectBase
{
    [SerializeField] private int rate;
    private int increase;

    public Buff_Attack(float duration, Unit target, int _rate, string _effectName) : base(duration, target)
    {
        base.effectName = _effectName;
        this.rate = _rate;
    }

    public override void ApplyEffect()
    {
        increase = (target.Att * rate) / 100;
        target.Att += increase;
        Debug.Log("버프 받음");
    }

    public override void RemoveEffect()
    {
        target.Att -= increase;
        Debug.Log($"버프 제거됨 증가량 : {increase}");
    }
}
