using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_ReinforcedBasicSkill : StatusEffectBase
{
    private Skill_Module reinforcedSkill;
    private Skill_Module basicSkill;

    public Buff_ReinforcedBasicSkill(float duration, Unit target, string _effectName, Skill_Module reinforced, Skill_Module basic) : base(duration, target)
    {
        effectName = _effectName;
        reinforcedSkill = reinforced;
        basicSkill = basic;
    }

    public override void ApplyEffect()
    {
        Debug.Log("평타 강화 적용");
        target.GetComponentInChildren<IAttackable>().ActiveSkills[0] = reinforcedSkill;
    }

    public override void RemoveEffect(bool isRefresh = false)
    {
        Debug.Log("평타 강화 제거");
        target.GetComponentInChildren<IAttackable>().ActiveSkills[0] = basicSkill;
    }
}
