
public enum NodeState
{
    Running,
    Success,
    Failure,
}

public abstract class BehaviorNode
{
    protected NodeState state;
    public NodeState State => state;
    public abstract NodeState Evaluate();
}
