using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "HighTide", menuName = "ScriptableObject/Skills/Active/YunSeoYeon/HighTide")]
public class HighTide : SkillBase
{
    [SerializeField] private Skill_Module reinforcedBasicSkill;
    [SerializeField] private Skill_Module basicSkill;

    public override bool UseSkill(ISkillCaster caster)
    {
        var unit = caster.GetCom<Unit>();
        unit.AddEffectProcess(new Buff_ReinforcedBasicSkill(30f, unit, "ReinforcedSkill", reinforcedBasicSkill, basicSkill));
        return true;
    }
}
