using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeatPressure", menuName = "ScriptableObject/Skills/Passive/YeonHaRyeon/HeatPressure")]
public class HeatPressure : SkillBase
{
    private Unit unit;
    public int heatPressure = 0;
    private int previousHeatPressure = 0;
    private const int MAX_AMOUNT = 100;

    // 유닛의 원본 스탯을 저장할 변수들
    private bool statsCaptured = false;
    private int baseAtt;
    private float baseMoveSpeed;
    private int baseJumpForce;

    public override void Initialize(Skill_Module module)
    {
        base.Initialize(module);
        // 스킬이 다시 초기화되면 스탯을 다시 캡쳐해야 함
        statsCaptured = false;
    }

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit == null)
        {
            unit = caster.GetGameObject().GetComponent<Unit>();
        }

        // 처음 unit을 얻었을 때, 원본 스탯을 한 번만 저장.
        if (unit != null && !statsCaptured)
        {
            baseAtt = unit.Att;
            baseMoveSpeed = unit.MoveSpeed;
            baseJumpForce = unit.JumpForce;
            statsCaptured = true;
            Debug.Log($"원본 스탯 저장: Att({baseAtt}), MoveSpeed({baseMoveSpeed}), JumpForce({baseJumpForce})");
        }

        // heatPressure 값이 변경되었을 때만 스탯을 업데이트.
        if (heatPressure != previousHeatPressure)
        {
            UpdateStats();
            previousHeatPressure = heatPressure;
        }
        else
        {
            //Debug.Log("열압 변경 없음.");
        }

        return true;
    }

    private void UpdateStats()
    {
        if (!statsCaptured) return;

        // 항상 원본 스탯을 기준으로 현재 heatPressure에 맞는 스탯을 계산하여 적용합니다.
        unit.Att = baseAtt + heatPressure;

        //열압 수치에 따른 이동속도및 점프 높이 감소. 현재 테스트를 위해 주석 처리. 추후 주석 제거.
        //unit.MoveSpeed = baseMoveSpeed - (heatPressure * 0.05f);
        //unit.JumpForce = baseJumpForce - (int)(heatPressure * 0.05f);

        Debug.Log($"스탯 업데이트: Heat({heatPressure}) -> Att({unit.Att}), MoveSpeed({unit.MoveSpeed}), JumpForce({unit.JumpForce})");
    }
}
