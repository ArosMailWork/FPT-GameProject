using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Unity.VisualStudio.Editor;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Script
{
    public class Health : MonoBehaviour
    {
        public UnityEvent eventTrigger;
        [FoldoutGroup("Stats")]
        public float health, invincibleTimer;
        [FoldoutGroup("Stats")]
        public float maxHealth = 100;
        public bool isDead;
        public bool isInvincible;


        void Start()
        {
            health = maxHealth;
        }

        void Update()
        {
            health = Mathf.Clamp(health, 0, maxHealth);
            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                Debug.Log("Invincible in " + invincibleTimer);
                isInvincible = !(invincibleTimer <= 0f) ;
            }
        }

        // Do damage
        public void DoDamage(float damage)
        {
            if (isInvincible)
            {
                Debug.Log("nuh uh");
                return;
            };
            health -= damage;
            Debug.Log("dealt " + damage + "remain " + health);
            if (health <= 0)
            {
                // Dead
                isDead = true;
                eventTrigger.Invoke();
                // Do smt...
            }
            else
            {
                // Hurt
                // Do smt...
            }

        }
        
        // Do Heal 
        public void DoHeal(float healAmount)
        {
            health += healAmount;
            Debug.Log("healed " + healAmount + "Health = " + health);
            // Do smt...
        }

        // Do Invincible
        public void DoInvincible(float time)
        {
            invincibleTimer = time;
            isInvincible = true;
            // Do smt...
        }

        //public void DoKnock(Vector3 direct)
        //{

        //}

        //public void OnCollisionEnter(Collision)
        //{

        //}
    }
}
