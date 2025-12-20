using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff_Stun : StatusEffectBase
{
    public Debuff_Stun(float duration, Unit target, string _effectName, GameObject _attacker) : base(duration, target)
    {
        effectName = _effectName;
    }

    public override void ApplyEffect()
    {
        Debug.Log("행동불가");
        target.CantAction = true;
    }

    public override void RemoveEffect(bool isRefresh = false)
    {
        Debug.Log("행동가능");
        target.CantAction = false;
    }
}
