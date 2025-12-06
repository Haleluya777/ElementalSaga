using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_ReinforcedBasicSkill : StatusEffectBase
{
    private SkillBase reinforcedSkill;
    private SkillBase basicSkill;
    private Skill_Module module;

    public Buff_ReinforcedBasicSkill(float duration, Unit target, string _effectName, SkillBase reinforced, SkillBase basic, Skill_Module _module) : base(duration, target)
    {
        effectName = _effectName;
        reinforcedSkill = reinforced;
        basicSkill = basic;
        module = _module;
    }

    public override void ApplyEffect()
    {
        Debug.Log("평타 강화 적용");
        module.ChangeSkillModule(basicSkill, reinforcedSkill);
    }

    public override void RemoveEffect()
    {
        Debug.Log("평타 강화 제거");
        module.ChangeSkillModule(reinforcedSkill, basicSkill);
    }
}
