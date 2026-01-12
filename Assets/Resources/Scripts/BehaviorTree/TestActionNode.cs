using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class TestActionNode : BTNode
{
    public override NodeState Evaluate()
    {
        return NodeState.Success;
    }
}
