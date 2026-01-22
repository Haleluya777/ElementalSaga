using XNode;


//행동트리의 각 노드의 상태.
public enum NodeState { Running, Success, Failure }

public abstract class BTNode : Node
{
    [Input] public BTNode parent;

    //모든 행동트리 노드가 실행해야 할 메서드
    public abstract NodeState Evaluate(AIController controller);

    public BlackBoard blackboard => (graph as BehaviorTreeGraph)?.blackboard;
}
