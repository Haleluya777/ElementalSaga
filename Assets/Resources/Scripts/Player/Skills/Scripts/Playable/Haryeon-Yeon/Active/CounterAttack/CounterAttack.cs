using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CounterAttack", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/CounterAttack")]
public class CounterAttack : SkillBase
{
    private Unit unit;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit is null) unit = caster.GetCom<Unit>();

        unit.AddEffectProcess(new Buff_Counter(-1f, unit, "Counter"));

        return true;
    }
}
