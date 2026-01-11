using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceNode : INode
{
    List<INode> childs;

    public SequenceNode(List<INode> _childs)
    {
        childs = _childs;
    }

    public INode.NodeState Evaluate()
    {
        if (childs == null || childs.Count == 0) return INode.NodeState.Failure;

        foreach (var child in childs)
        {
            switch (child.Evaluate())
            {
                case INode.NodeState.Running:
                    return INode.NodeState.Running;

                case INode.NodeState.Success:
                    continue;

                case INode.NodeState.Failure:
                    return INode.NodeState.Failure;
            }
        }

        return INode.NodeState.Success;
    }
}
