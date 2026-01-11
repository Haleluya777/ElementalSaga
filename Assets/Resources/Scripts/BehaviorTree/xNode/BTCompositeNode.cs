using UnityEngine;
using XNode;

public abstract class BTCompositeNode : BTNode
{
    [Input(connectionType = ConnectionType.Override)] public BTNode parent;
    [Output(dynamicPortList = true)] public BTNode children;
    // 참고: 실제 Build 로직은 Sequence와 Selector에 따라 다르므로
    // 파생 클래스에서 구현됩니다.
}
