using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : BTNode
{
    public enum ConditionType { Hp, Distance, PlayerAction }
    public ConditionType type;
    public float value;

    public override NodeState Evaluate(AIController controller)
    {
        switch (type)
        {
            case ConditionType.Hp:
                break;

            case ConditionType.Distance:
                float currentDist = (graph as BehaviorTreeGraph).blackboard.Get<float>("Distance");
                break;

            case ConditionType.PlayerAction:
                break;
        }

        return NodeState.Failure;
    }
}
