using System.Collections.Generic;

public class SelectorNode : Node
{
    private List<Node> children = new List<Node>();

    public SelectorNode(List<Node> children)
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