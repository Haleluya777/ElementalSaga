using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformTestAttack : BTNode
{
    public override NodeState Evaluate(AIController controller)
    {
        if (blackboard is null) return NodeState.Failure;

        //var skill = controller.attack.

        controller.CallMoveEvent(Vector3.zero);
        Debug.Log("또 다른 공격 시작");

        return NodeState.Success;
    }
}
