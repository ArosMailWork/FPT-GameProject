using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public bool isAttack;
        public int maxHealth = 100;
        
        public int BaseDamage { get; set; }

        private HealthSystem healthSystem;

        private void Awake()
        {
           
            
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var player = other.gameObject.GetComponent<PlayerController>();
                if (player.isAttack == true)
                {
                    healthSystem.ReceiveDamage(player.BaseDamage);
                    // _playerAnimController.GetHit();
                }
            }
        }
    }
}