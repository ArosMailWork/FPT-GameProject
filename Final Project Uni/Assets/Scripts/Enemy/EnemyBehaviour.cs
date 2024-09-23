using UnityEngine;
using System.Collections.Generic;
public class EnemyBehaviour : MonoBehaviour
{
    private BehaviourTree behaviourTree;
    Transform player;

    private void Start()
    {
        //action note
        Node moveToPlayer = new MoveToPlayer(this.transform, player);

        //condition note
    }

    private void Update()
    {
        behaviourTree.Update();
    }
}

