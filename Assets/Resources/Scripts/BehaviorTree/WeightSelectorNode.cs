using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class WeightSelectorNode : BTNode
{
    [Output(dynamicPortList = true)] public List<float> weightList;

    public override NodeState Evaluate(AIController controller)
    {
        NodePort port = GetOutputPort("가중치목록");
        float total = 0;
        for (int i = 0; i < port.ConnectionCount; i++) total += weightList[i];

        float roll = Random.Range(0, total);
        float cumulative = 0;

        for (int i = 0; i < port.ConnectionCount; i++)
        {
            cumulative += weightList[i];
            if (roll <= cumulative)
            {
                BTNode child = port.GetConnection(i).node as BTNode;
                return child.Evaluate(controller);
            }
        }

        return NodeState.Failure;
    }
}
