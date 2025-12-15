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

        caster.PlayAnimation(animName);

        //heatPressurePassive.heatPressure -= (int)(heatPressurePassive.heatPressure * 30) / 100;
        Buff_AttackSpeed attackSpeedBuff = new Buff_AttackSpeed(5f, casterUnit, 50f, "VentingPressure_AttackSpeed_Buff");
        Buff_MoveSpeed moveSpeedBuff = new Buff_MoveSpeed(5f, casterUnit, 50f, "VentingPressure_MoveSpeed_Buff");

        casterUnit.AddEffectProcess(attackSpeedBuff);
        casterUnit.AddEffectProcess(moveSpeedBuff);

        return true;
    }
}
