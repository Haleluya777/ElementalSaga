using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlameBlood", menuName = "ScriptableObject/Skills/Passive/YeonHaRyeong/FlameBlood")]
public class FlameBlood : SkillBase
{
    private Unit unit;
    public int flameBloodGage = 0;
    private int previousHeatPressure = 0;

    // 유닛의 원본 스탯을 저장할 변수들
    private bool statsCaptured = false;
    private int baseAtt;
    private int baseMoveSpeed;
    private int baseMaxHp;

    public override void Initialize(Skill_Module module)
    {
        base.Initialize(module);
        statsCaptured = false;
    }

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit == null)
        {
            unit = caster.GetGameObject().GetComponent<Unit>();
        }

        if (unit != null && !statsCaptured)
        {
            baseAtt = unit.Att;
            baseMoveSpeed = unit.MoveSpeed;
            baseMaxHp = unit.MaxHp;
            statsCaptured = true;
            //Debug.Log($"원본 스탯 저장: Att({baseAtt}), MoveSpeed({baseMoveSpeed}), JumpForce({baseJumpForce})");
        }

        return true;
    }

    private void UpdateStats()
    {
        if (!statsCaptured) return;

        // 항상 원본 스탯을 기준으로 현재 heatPressure에 맞는 스탯을 계산하여 적용합니다.
        unit.Att = baseAtt + (int)(flameBloodGage * 5 / 100);
        unit.MoveSpeed = baseMoveSpeed - (int)(flameBloodGage * 0.05f);
        unit.JumpForce = baseMaxHp - (int)(flameBloodGage * 0.05f);

        Debug.Log($"스탯 업데이트: 염혈:({flameBloodGage}) -> 공격력:({unit.Att}), 이동 속도:({unit.MoveSpeed}), 최대 체력:({unit.MaxHp})");
    }
}
