using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingDistance : BTNode
{
    Vector3 unitPos;
    Vector3 playerPos;

    public override NodeState Evaluate(AIController controller)
    {
        unitPos = controller.ParentObj.transform.position;
        playerPos = GameManager.instance.unitManager.PlayerUnit.transform.position;

        blackboard.Set<Vector3>("UnitPos", unitPos);
        blackboard.Set<float>("Distance", Mathf.Abs((playerPos.x - unitPos.x)));
        return NodeState.Failure;
    }
}
