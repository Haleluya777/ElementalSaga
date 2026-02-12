using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionNode : BTNode
{
    public enum ConditionType { Hp, Melee_Distance, Range_Distance, PlayerAction, Attacking }
    public enum SignType { Greater, Lower, EqualGreater, EqualLower, Equal, NotEqual }

    public ConditionType type;
    public SignType sign;

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

            case ConditionType.Range_Distance:
                {
                    float currentDist = (graph as BehaviorTreeGraph).blackboard.Get<float>("Distance");
                    switch (sign)
                    {
                        case SignType.Greater:
                            {
                                if (currentDist > unit.RangerRange)
                                {
                                    Debug.Log("원거리 공격 거리 밖임. 이동해야 함.");
                                    return NodeState.Success;
                                }
                                else return NodeState.Failure;
                            }

                        case SignType.EqualLower:
                            {
                                if (currentDist <= unit.RangerRange)
                                {
                                    //Debug.Log("근접 공격 거리 안으로 들어옴.");
                                    return NodeState.Success;
                                }
                                else return NodeState.Failure;
                            }
                    }
                    break;
                }

            case ConditionType.Melee_Distance:
                {
                    float currentDist = (graph as BehaviorTreeGraph).blackboard.Get<float>("Distance");
                    switch (sign)
                    {
                        case SignType.Greater:
                            {
                                if (currentDist > unit.MeleeRange)
                                {
                                    Debug.Log("근접 공격 거리 밖임. 이동해야 함.");
                                    return NodeState.Success;
                                }
                                else return NodeState.Failure;
                            }

                        case SignType.EqualLower:
                            {
                                if (currentDist <= unit.MeleeRange)
                                {
                                    //Debug.Log("근접 공격 거리 안으로 들어옴.");
                                    return NodeState.Success;
                                }
                                else return NodeState.Failure;
                            }
                    }
                    break;
                }

            case ConditionType.PlayerAction:
                break;

            case ConditionType.Attacking:
                switch (sign)
                {
                    case SignType.Equal:
                        {
                            if (controller.curState == AIController.UnitState.Attacking)
                            {
                                return NodeState.Success;
                            }
                            else return NodeState.Failure;
                        }

                    case SignType.NotEqual:
                        {
                            if (controller.curState != AIController.UnitState.Attacking)
                            {
                                //Debug.Log("공격중이 아님");
                                return NodeState.Success;
                            }
                            else return NodeState.Failure;
                        }
                }
                return NodeState.Failure;
        }

        return NodeState.Failure;
    }
}
