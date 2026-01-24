using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformAttack : BTNode
{
    public int SkillNum;

    public override NodeState Evaluate(AIController controller)
    {
        if (blackboard is null) return NodeState.Failure;

        var skill = controller.attack.ActiveSkills[SkillNum];

        controller.CallMoveEvent(Vector3.zero);
        controller.attack.PerformAttack(skill);

        controller.curState = AIController.UnitState.Attacking;

        return NodeState.Success;
    }
}
