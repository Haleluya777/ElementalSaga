using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckingGround", menuName = "ScriptableObject/Skills/Passive/YunSeoYeon/CheckingGround")]
public class CheckingGround : SkillBase
{
    private Unit unit;

    public override bool UseSkill(ISkillCaster caster)
    {
        if (unit == null)
        {
            Debug.Log("겟 컴을 합니다.");
            unit = caster.GetCom<Unit>();
        }

        if (!unit.isAirial) //이 스킬을 가진 유닛이 땅을 딛고 있을 때.
        {
            parentModule.blackBoard.Set<bool>("Condition", true);
        }

        return true;
    }
}
