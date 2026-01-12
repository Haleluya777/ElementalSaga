using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class SelectorNode : BTNode
{
    [Output] public List<BTNode> childs;

    public override NodeState Evaluate()
    {
        foreach (var child in childs)
        {
            switch (child.Evaluate())
            {
                case NodeState.Success:
                    return NodeState.Success;

                case NodeState.Running:
                    return NodeState.Running;

                case NodeState.Failure:
                    continue;
            }
        }
        return NodeState.Failure;
    }
}
