using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class RootNode : BTNode
{
    [Output] public BTNode selectorNode;

    public override NodeState Evaluate(AIController controller)
    {
        return selectorNode.Evaluate(controller);
    }
}
