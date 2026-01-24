using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class WeightSelectorNode : BTNode
{
    [Output(dynamicPortList = true)] public List<float> WeightList;

    public override NodeState Evaluate(AIController controller)
    {
        //NodePort port = GetOutputPort("WeightList");
        //Debug.Log(port.ConnectionCount);
        float total = 0;
        List<NodePort> connectedPorts = new List<NodePort>();
        //for (int i = 0; i < port.ConnectionCount; i++) total += WeightList[i];

        for (int i = 0; i < WeightList.Count; i++)
        {
            // "WeightList 0", "WeightList 1" 처럼 인덱스가 붙은 포트 이름을 찾습니다.
            NodePort port = GetOutputPort("WeightList " + i);

            if (port != null && port.IsConnected)
            {
                total += WeightList[i];
                connectedPorts.Add(port);
            }
        }

        float roll = Random.Range(0, total);
        float cumulative = 0;

        for (int i = 0; i < WeightList.Count; i++)
        {
            NodePort port = GetOutputPort("WeightList " + i);
            if (port == null || !port.IsConnected) continue;

            cumulative += WeightList[i];
            if (roll <= cumulative)
            {
                // 연결된 포트에서 상대방 노드를 가져옴
                BTNode child = port.Connection.node as BTNode;
                if (child != null)
                {
                    return child.Evaluate(controller);
                }
            }
        }

        // for (int i = 0; i < port.ConnectionCount - 1; i++)
        // {
        //     cumulative += WeightList[i];
        //     Debug.Log("공격 시작");
        //     if (roll <= cumulative)
        //     {

        //         BTNode child = port.GetConnection(i).node as BTNode;
        //         return child.Evaluate(controller);
        //     }
        // }

        return NodeState.Failure;
    }
}
