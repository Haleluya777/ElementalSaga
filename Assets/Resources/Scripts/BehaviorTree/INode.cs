public interface INode
{
    public enum NodeState { Running, Failure, Success }
    public NodeState Evaluate();
}
