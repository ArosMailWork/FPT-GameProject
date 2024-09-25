using UnityEngine;
using System;

public class HitBox : MonoBehaviour
{
    public int damage = 10;
    public float activeTime = 0.5f;

    private PlayerController playerController;
    //đớp trúng thì player hồi máu
    public event Action<HitBox> OnHit;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found in parent objects.");
        }
    }

    public void TurnOnHitBox()
    {
        Collider hitboxCollider = GetComponent<Collider>();
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = true;
            Invoke(nameof(TurnOffHitBox), activeTime);
        }
        else
        {
            Debug.LogError("Collider component not found on HitBox.");
        }
    }

//force trigger cũng đc
    public void TurnOffHitBox()
    {
        Collider hitboxCollider = GetComponent<Collider>();
        if (hitboxCollider != null)
        {
            hitboxCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // EnemyController enemyController = other.GetComponent<EnemyController>();
            // if (enemyController != null && enemyController.healthSystem != null)
            // {
            //     enemyController.healthSystem.ReceiveDamage(damage);
            //     OnHit?.Invoke(this);
            // }
            // else
            // {
            //     Debug.LogWarning("Enemy hit, but EnemyController or HealthSystem not found.");
            // }
        }
    }
}
