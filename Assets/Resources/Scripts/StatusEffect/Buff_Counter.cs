using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Counter : StatusEffectBase
{
    public Buff_Counter(float duration, Unit target, string _effectName) : base(duration, target)
    {
        base.effectName = _effectName;
    }

    public override void ApplyEffect()
    {
        target.GraceState = true;
        target.TakeDamageEvent += AutoDodge;
    }

    public override void RemoveEffect(bool isRefresh = false)
    {
        target.GraceState = false;
        target.TakeDamageEvent -= AutoDodge;
    }

    public void AutoDodge(int dmg, ISkillCaster attacker, GameObject obj)
    {
        Debug.Log("공격 무시!");
    }
}
