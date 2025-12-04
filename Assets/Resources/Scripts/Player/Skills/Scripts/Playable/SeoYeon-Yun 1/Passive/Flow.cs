using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flow", menuName = "ScriptableObject/Skills/Passive/YunSeoYeon/Flow")]
public class Flow : SkillBase
{
    private Unit unit;
    public int flowGage = 0;
    private int previousFlowGage = 0;

    //원본 스탯 변수.
    private bool statsCaptured = false;
    private int baseAtt;
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

        // 처음 unit을 얻었을 때, 원본 스탯을 한 번만 저장.
        if (unit != null && !statsCaptured)
        {
            baseAtt = unit.Att;
            statsCaptured = true;
            //Debug.Log($"원본 스탯 저장: Att({baseAtt}), MoveSpeed({baseMoveSpeed}), JumpForce({baseJumpForce})");
        }

        if (flowGage != previousFlowGage)
        {
            UpdateStats();
            previousFlowGage = flowGage;
        }

        if (flowGage == 6)
        {
            GameManager.instance.coroutineRunner.StartCoroutine(ResetFlow());
            return true;
        }

        return true;
    }

    private void UpdateStats()
    {
        if (!statsCaptured) return;

        // 항상 원본 스탯을 기준으로 현재 heatPressure에 맞는 스탯을 계산하여 적용합니다.
        unit.Att = baseAtt + ((baseAtt * 5 / 100) * flowGage);

        Debug.Log($"스탯 업데이트: Flow({flowGage}) -> Att({unit.Att})");
    }

    private IEnumerator ResetFlow()
    {
        yield return new WaitForSeconds(5f);
        flowGage = 0;
    }
}
