using UnityEngine;
using XNode;
using System;
using System.Reflection;

[Node.CreateNodeMenu("Behavior Tree/Action")]
[Node.NodeTint("#784a4a")] // 붉은색 계열
public class BTActionNode : BTNode
{
    [Input(connectionType = ConnectionType.Override)] public BTNode parent;

    [Tooltip("BehaviorTreeExecutor에서 호출할 메서드의 이름입니다.")]
    public string actionName;

    public override INode Build(GameObject agent)
    {
        if (string.IsNullOrEmpty(actionName))
        {
            Debug.LogError("액션 노드에 액션 이름이 지정되지 않았습니다.");
            return new ActionNode(() => INode.NodeState.Failure);
        }

        // 에이전트에서 Executor 컴포넌트를 가져옵니다.
        var executor = agent.GetComponent<BehaviorTreeExecutor>();
        if (executor == null)
        {
            Debug.LogError($"에이전트 '{agent.name}'에 BehaviorTreeExecutor 컴포넌트가 없습니다.");
            return new ActionNode(() => INode.NodeState.Failure);
        }

        // 리플렉션을 사용하여 Executor에서 메서드를 찾습니다.
        MethodInfo method = executor.GetType().GetMethod(actionName, BindingFlags.Public | BindingFlags.Instance);

        if (method == null || method.ReturnType != typeof(INode.NodeState))
        {
            Debug.LogError($"에이전트 '{agent.name}'의 Executor에 '{actionName}'이라는 이름의 유효한 public 메서드가 없거나 반환 타입이 INode.NodeState가 아닙니다.");
            return new ActionNode(() => INode.NodeState.Failure);
        }

        // 찾은 메서드를 호출하는 델리게이트를 생성합니다.
        Func<INode.NodeState> action = () => (INode.NodeState)method.Invoke(executor, null);

        return new ActionNode(action);
    }
}
