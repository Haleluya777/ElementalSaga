using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_KillDrain : StatusEffectBase
{
    private float rate;

    public Buff_KillDrain(float duration, Unit target, float _rate, string _effectName) : base(duration, target)
    {
        effectName = _effectName;
        rate = _rate;
    }

    public override void ApplyEffect()
    {
        GameManager.instance.eventManager.DeadEvent += KillDrain;
    }

    public override void RemoveEffect(bool isRefresh = false)
    {
        GameManager.instance.eventManager.DeadEvent -= KillDrain;
    }

    public void KillDrain(ISkillCaster attacker)
    {
        Debug.Log($"킬! 체력 흡수! 흡수량 : {(int)(target.MaxHp * rate / 100)}");
        target.CurHp += (int)(target.MaxHp * rate / 100);
    }
}
