using UnityEngine;
using XNode;
using System.Collections.Generic;

// 행동 트리 에디터의 모든 비주얼 노드를 위한 기본 클래스입니다.
// 주요 목적은 런타임 INode 객체를 생성하는 팩토리 역할을 하는 것입니다.
public abstract class BTNode : Node
{
    /// <summary>
    /// 이 비주얼 노드를 해당하는 런타임 노드로 변환합니다.
    /// </summary>
    /// <param name="agent">행동 트리를 실행할 게임 오브젝트입니다.</param>
    /// <returns>런타임 INode 객체.</returns>
    public abstract INode Build(GameObject agent);

    // 단일 자식 연결을 가져오는 헬퍼
    protected BTNode GetChildNode(string portName)
    {
        NodePort port = GetOutputPort(portName);
        if (port == null || !port.IsConnected)
        {
            return null;
        }
        return port.Connection.node as BTNode;
    }

    // 여러 자식 연결을 가져오는 헬퍼
    protected List<BTNode> GetChildNodes(string portName)
    {
        var childNodes = new List<BTNode>();
        NodePort port = GetOutputPort(portName);
        if (port != null)
        {
            foreach (var connection in port.GetConnections())
            {
                if (connection.node is BTNode btNode)
                {
                    childNodes.Add(btNode);
                }
            }
        }
        return childNodes;
    }

    public override object GetValue(NodePort port)
    {
        return null;
    }
}