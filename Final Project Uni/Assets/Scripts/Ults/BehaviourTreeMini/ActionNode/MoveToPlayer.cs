using UnityEngine;

public class MoveToPlayer : Node
{
    private Transform transform;
    private Transform player;
    private float moveSpeed = 5f;

    private float stoppingDistance = 1f;
    // private float rotationSpeed = 10f;
    // private float attackRange = 2f;


    public MoveToPlayer(Transform transform,Transform player)
    {
        this.transform = transform;
        this.player = player;
    }

    public override NodeState Evaluate()
    {
        if (player == null)
        {
            state = NodeState.Failure;
            return state;
        }

        //nếu chưa đủ gần thì cứ lao vào    
        if (Vector3.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }

        //nếu đủ gần thì thành công
        if (Vector3.Distance(transform.position, player.position) <= stoppingDistance)
        {
            state = NodeState.Success;
            return state;
        }

        //nếu chưa đến thì vẫn lặp lại quá trình trên
        state = NodeState.Running;
        return state;
    }
}