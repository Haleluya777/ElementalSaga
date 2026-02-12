using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FillHeatSeal", menuName = "ScriptableObject/Skills/Passive/YeonHaRyeon/FillHeatSeal")]
public class FillHeatSeal : OnHitEventBase
{
    private HeatSeal heatSeal;

    public override void Initialize(Unit caster)
    {
        base.Initialize(caster);
        if (heatSeal == null)
        {
            heatSeal = attack.FindSkill(heatSeal);
        }
    }

    public override void Execute(GameObject target, ISkillCaster caster, int dmg)
    {
        heatSeal.heatSeal += 1;
        heatSeal.heatSeal = Mathf.Clamp(heatSeal.heatSeal, 0, 3);
        Debug.Log($"현재 열장인 수 : {heatSeal.heatSeal}");
    }
}
