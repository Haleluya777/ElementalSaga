using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VentingPressure", menuName = "ScriptableObject/Skills/Active/YeonHaRyeon/VentingPressure")]
public class VentingPressure : SkillBase
{
    [SerializeField] private HeatPressure heatPressurePassive;
    public override bool UseSkill(ISkillCaster caster)
    {
        Unit casterUnit = caster.GetCom<Unit>();

        //heatPressurePassive.heatPressure -= (int)(heatPressurePassive.heatPressure * 30) / 100;
        Buff_Attack attackBuff = new Buff_Attack(5f, casterUnit, 50, "VectingPressure_Attack_Buff");
        casterUnit.AddEffectProcess(attackBuff);
        return true;
    }
}
