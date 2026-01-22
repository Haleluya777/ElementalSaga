using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingDistance : BTNode
{
    Vector3 unitPos;
    Vector3 playerPos;

    public override NodeState Evaluate(AIController controller)
    {
        unitPos = blackboard.Get<Vector3>("UnitPos");
        playerPos = GameManager.instance.unitManager.PlayerUnit.transform.position;

        blackboard.Set<float>("Distance", Mathf.Abs((playerPos.x - unitPos.x)));
        return NodeState.Failure;
    }
}
