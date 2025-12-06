using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HighTide", menuName = "ScriptableObject/Skills/Active/YunSeoYeon/HighTide")]
public class HighTide : SkillBase
{
    [SerializeField] private SkillBase reinforcedBasicSkill;
    [SerializeField] private SkillBase basicSkill;

    public override bool UseSkill(ISkillCaster caster)
    {
        var unit = caster.GetCom<Unit>();
        unit.AddEffectProcess(new Buff_ReinforcedBasicSkill(30f, unit, "ReinforcedSkill", reinforcedBasicSkill, basicSkill, parentModule));
        return true;
    }
}
