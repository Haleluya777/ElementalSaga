using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class SequenceNode : BTNode
{
    [Output] public List<BTNode> childs;

    public override NodeState Evaluate()
    {
        foreach (var child in childs)
        {
            switch (child.Evaluate())
            {
                case NodeState.Success:
                    continue;

                case NodeState.Failure:
                    return NodeState.Failure;
            }
        }
        return NodeState.Success;
    }
}
