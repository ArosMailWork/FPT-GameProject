using System.Collections.Generic;

public class SelectorNode : BehaviorNode
{
    private List<BehaviorNode> children = new List<BehaviorNode>();

    public SelectorNode(List<BehaviorNode> children)
    {
        this.children = children;
    }   

    public override NodeState Evaluate()
    {
        foreach (var child in children) 
        {
            switch (child.Evaluate())
            {
                case NodeState.Running:
                    state = NodeState.Running;
                    return state;
                case NodeState.Success:
                    state = NodeState.Success;
                    return state;
                case NodeState.Failure:
                    break;
            }
        }
        state = NodeState.Failure;
        return state;
    }
}   