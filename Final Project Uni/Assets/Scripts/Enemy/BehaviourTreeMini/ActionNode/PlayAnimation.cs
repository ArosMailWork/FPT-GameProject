using UnityEngine;

public class PlayAnimation : BehaviorNode
{
    private Animator animator;
    private string animationName;

    public PlayAnimation(Animator animator, string animationName)
    {
        this.animator = animator;
        this.animationName = animationName;
    }

    public override NodeState Evaluate()
    {
        animator.Play(animationName);
        state = NodeState.Success;
        return state;

    }
}