using UnityEngine;
using XNode;

[Node.CreateNodeMenu("Behavior Tree/Root", 100)] // 메뉴에서 높은 우선순위
[Node.NodeTint("#347c34")] // 진한 녹색
public class BTRootNode : BTNode
{
    [Output(connectionType = ConnectionType.Override)] public BTNode child;

    // 이 노드는 직접적인 런타임 대응물이 없습니다.
    // 그래프가 시작점을 찾기 위한 유틸리티 노드입니다.
    // Build 메소드는 단순히 자식에게 빌드를 위임합니다.
    public override INode Build(GameObject agent)
    {
        BTNode childNode = GetChildNode("child");
        if (childNode == null)
        {
            Debug.LogError("루트 노드가 자식 노드에 연결되어 있지 않습니다.");
            return null;
        }
        return childNode.Build(agent);
    }
}
