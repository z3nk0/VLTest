﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VLTest.Enemies
{
    public class EnemyHealth : EnemyComponent
    {
        #region MEMBERS
        public float health;
        #endregion

        #region PUBLIC METHODS
        public void SetDamage(float damage)
        {
            if (health > 0)
            {
                health = Mathf.Max(0, health - damage);
                enemy.hitEffect.PlayHitEffect();
                if (health == 0)
                {
                    enemy.Kill();
                }
            }
        }

        public void SetHealth(float health)
        {
            this.health = health;
        }
        #endregion

    }
}