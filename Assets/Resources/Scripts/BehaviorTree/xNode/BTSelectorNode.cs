using UnityEngine;
using XNode;
using System.Linq;

[Node.CreateNodeMenu("Behavior Tree/Composites/Selector")]
[Node.NodeTint("#78644a")] // 주황/갈색 계열
public class BTSelectorNode : BTCompositeNode
{
    public override INode Build(GameObject agent)
    {
        var childRuntimeNodes = GetChildNodes("children")
                                  .Select(child => child.Build(agent))
                                  .ToList();
        return new SelectorNode(childRuntimeNodes);
    }
}
