using UnityEngine;
using XNode;
using System.Linq;

[Node.CreateNodeMenu("Behavior Tree/Composites/Sequence")]
[Node.NodeTint("#4a6378")] // 푸른색 계열
public class BTSequenceNode : BTCompositeNode
{
    public override INode Build(GameObject agent)
    {
        var childRuntimeNodes = GetChildNodes("children")
                                  .Select(child => child.Build(agent))
                                  .ToList();
        return new SequenceNode(childRuntimeNodes);
    }
}
