using UnityEngine;
using XNode;
using System.Linq;

[CreateAssetMenu(fileName = "New Behavior Tree", menuName = "AI/Behavior Tree Graph")]
public class BehaviorTreeGraph : NodeGraph
{
    /// <summary>
    /// 루트 노드를 찾아 재귀적인 빌드 프로세스를 시작합니다.
    /// </summary>
    /// <param name="agent">트리를 실행할 에이전트입니다.</param>
    /// <returns>런타임 INode 트리의 루트입니다.</returns>
    public INode Build(GameObject agent)
    {
        // 그래프에서 BTRootNode를 찾습니다.
        BTRootNode rootNode = nodes.FirstOrDefault(n => n is BTRootNode) as BTRootNode;

        if (rootNode == null)
        {
            Debug.LogError("행동 트리 그래프에는 루트 노드(Root Node)가 반드시 하나 있어야 합니다.");
            return null;
        }

        return rootNode.Build(agent);
    }
}