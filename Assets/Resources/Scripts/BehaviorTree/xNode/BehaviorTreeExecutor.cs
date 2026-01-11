using UnityEngine;

public class BehaviorTreeExecutor : MonoBehaviour
{
    [Tooltip("실행할 행동 트리 그래프 애셋입니다.")]
    public BehaviorTreeGraph behaviorTreeGraph;

    // 행동 트리의 런타임 인스턴스입니다.
    private INode _runtimeTree;

    void Start()
    {
        if (behaviorTreeGraph == null)
        {
            Debug.LogError("BehaviorTreeExecutor에 BehaviorTreeGraph가 할당되지 않았습니다.", this);
            return;
        }

        // 그래프 애셋으로부터 런타임 트리를 빌드합니다.
        _runtimeTree = behaviorTreeGraph.Build(this.gameObject);
    }

    void Update()
    {
        // 트리가 유효하면 매 프레임 실행(Evaluate)합니다.
        if (_runtimeTree != null)
        {
            _runtimeTree.Evaluate();
        }
    }

    // --- 액션 메서드 라이브러리 ---
    // AI의 액션 메서드들을 여기에 추가하세요.
    // 반드시 public이어야 하고 INode.NodeState를 반환해야 합니다.
    // BTActionNode는 이 메서드들을 이름으로 찾아 호출합니다.

    [Header("디버그용 액션")]
    public bool shouldSucceed = true;

    public INode.NodeState TestSuccess()
    {
        Debug.Log("TestSuccess 액션 실행: 성공");
        return INode.NodeState.Success;
    }

    public INode.NodeState TestFailure()
    {
        Debug.Log("TestFailure 액션 실행: 실패");
        return INode.NodeState.Failure;
    }

    public INode.NodeState TestConditional()
    {
        if (shouldSucceed)
        {
            Debug.Log("TestConditional 액션 실행: 성공");
            return INode.NodeState.Success;
        }
        else
        {
            Debug.Log("TestConditional 액션 실행: 실패");
            return INode.NodeState.Failure;
        }
    }
}
