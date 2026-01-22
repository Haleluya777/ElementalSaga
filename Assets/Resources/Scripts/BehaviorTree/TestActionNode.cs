using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class TestActionNode : BTNode
{
    public override NodeState Evaluate(AIController controller)
    {
        Debug.Log("행동중");
        return NodeState.Success;
    }
}
