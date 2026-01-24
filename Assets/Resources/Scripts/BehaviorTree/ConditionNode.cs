using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : BTNode
{
    public enum ConditionType { Hp, Melee_Distance, PlayerAction }
    public ConditionType type;
    EnemyCharacter unit;
    //public float value;

    public override NodeState Evaluate(AIController controller)
    {
        if (unit is null)
        {
            Debug.Log("EnemyCharacter 컴포넌트가 없음.");
            unit = controller.ParentObj.GetComponent<EnemyCharacter>();
        }

        switch (type)
        {
            case ConditionType.Hp:
                break;

            case ConditionType.Melee_Distance:
                float currentDist = (graph as BehaviorTreeGraph).blackboard.Get<float>("Distance");
                //Debug.Log(currentDist);
                if (currentDist <= unit.MeleeRange)
                {
                    Debug.Log("근접 공격 거리 안으로 들어옴.");
                    return NodeState.Success;
                }
                break;

            case ConditionType.PlayerAction:
                break;
        }

        return NodeState.Failure;
    }
}
