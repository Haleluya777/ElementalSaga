using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeatSeal", menuName = "ScriptableObject/Skills/Passive/YeonHaRyeon/HeatSeal")]
public class HeatSeal : SkillBase
{
    private Unit unit;
    public int heatSeal = 0;
    private int previousHeatSeal = 0;

    public override void Initialize(Skill_Module module)
    {
        base.Initialize(module);
    }

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit == null)
        {
            unit = caster.GetGameObject().GetComponent<Unit>();
        }

        if (heatSeal != previousHeatSeal)
        {
            UpdateBuff();
            previousHeatSeal = heatSeal;
        }
        else
        {
            Debug.Log($"열인장 변경 없음. 현재 열인장 {heatSeal}");
        }

        return true;
    }

    private void UpdateBuff()
    {
        switch (heatSeal)
        {
            case 0:
                unit.RemoveEffect("IncreaseGiveDmgRate");
                break;

            case 1:
                unit.AddEffectProcess(new Buff_IncreaseGiveDmgRate(-1f, unit, 25, "IncreaseGiveDmgRate"));
                break;

            case 2:
                unit.AddEffectProcess(new Buff_IncreaseGiveDmgRate(-1f, unit, 50, "IncreaseGiveDmgRate"));
                break;

            case 3:
                unit.AddEffectProcess(new Buff_IncreaseGiveDmgRate(-1f, unit, 50, "IncreaseGiveDmgRate"));
                break;
        }
    }
}
