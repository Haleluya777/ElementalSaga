using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_AttackSpeed : StatusEffectBase
{
    private float rate;
    private float increase;

    public Buff_AttackSpeed(float duration, Unit target, float _rate, string _effectName) : base(duration, target)
    {
        base.effectName = _effectName;
        this.rate = _rate;
    }

    public override void ApplyEffect()
    {
        Debug.Log("공격 속도 버프 받음.");
        var anim = target.GetComponent<Animator>();
        increase = rate / 100f;
        anim.SetFloat("AnimationSpeed", 1 + increase);
        target.GetComponentInChildren<IAttackable>().ActiveSkills[0].coolDown = 0.15f;
    }

    public override void RemoveEffect()
    {
        Debug.Log("공격 속도 버프 삭제.");
        var anim = target.GetComponent<Animator>();
        anim.SetFloat("AnimationSpeed", 1);
    }
}
