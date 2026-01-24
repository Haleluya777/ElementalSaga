using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : BTNode
{
    public enum ConditionType { Hp, Melee_Distance, PlayerAction, Attacking }
    public ConditionType type;
    EnemyCharacter unit;
    Animator anim;
    //public float value;

    public override NodeState Evaluate(AIController controller)
    {
        if (anim is null) anim = controller.GetComponentInChildren<Animator>();
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
                if (currentDist <= unit.MeleeRange)
                {
                    Debug.Log("근접 공격 거리 안으로 들어옴.");
                    return NodeState.Success;
                }
                break;

            case ConditionType.PlayerAction:
                break;

            case ConditionType.Attacking:
                if (controller.curState != AIController.UnitState.Attacking)
                {
                    return NodeState.Success;
                }
                break;
        }

        return NodeState.Failure;
    }
}
