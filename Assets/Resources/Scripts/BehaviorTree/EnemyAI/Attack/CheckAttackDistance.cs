using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAttackDistance : BTNode
{
    public override NodeState Evaluate(AIController controller)
    {
        if (blackboard is null) return NodeState.Failure;

        EnemyCharacter unit = controller.ParentObj.GetComponent<EnemyCharacter>();
        if (unit.MeleeRange >= blackboard.Get<float>("Distance") || unit.RangerRange >= blackboard.Get<float>("Distance"))
        {
            Debug.Log("공격 범위 안에 잇음");
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}
