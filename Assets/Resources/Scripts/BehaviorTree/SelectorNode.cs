using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorNode : INode
{
    List<INode> childs;

    public SelectorNode(List<INode> _childs)
    {
        childs = _childs;
    }

    public INode.NodeState Evaluate()
    {
        if (childs == null)
        {
            return INode.NodeState.Failure;
        }

        foreach (var child in childs)
        {
            switch (child.Evaluate())
            {
                case INode.NodeState.Running:
                    return INode.NodeState.Running;

                case INode.NodeState.Success:
                    return INode.NodeState.Success;
            }
        }

        return INode.NodeState.Failure;
    }
}
