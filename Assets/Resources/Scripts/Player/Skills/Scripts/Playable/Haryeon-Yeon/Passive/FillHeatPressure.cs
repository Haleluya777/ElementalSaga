using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FillHeatPressure", menuName = "ScriptableObject/Skills/Passive/YeonHaRyeon/FillHeatPressure")]
public class FillHeatPressure : OnHitEventBase
{
    //패시브 : 일반 공격시 열압 게이지 증가.
    //열압 게이지는 패시브 스킬 스크립트 내부에 존재.
    [SerializeField] private HeatPressure heatPressurePassive;

    public override void Execute(GameObject target, ISkillCaster caster, int dmg)
    {
        heatPressurePassive.heatPressure += 3;
    }
}
