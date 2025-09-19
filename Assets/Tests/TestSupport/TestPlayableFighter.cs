using Assets.Combat;
using Assets.PlayerCharacter;
using System;
using UnityEngine;

namespace Tests.Support
{
    public class TestPlayableFighter : PlayableFighter
    {
        public override void Awake()
        {
            base.Awake();
        }

        public void DefeatSelf()
        {
            while (GetStat(FighterStats.Health) > 0)
            {
                ModifyStat(FighterStats.Health, -5);
                TakeDamage(this);
            }
        }

        public void DefeatRandomEnemy()
        {
            var enemies = GameObject.FindObjectsByType<TestNpcFighter>(FindObjectsSortMode.None);
            foreach (var enemy in enemies)
            {
                if (enemy != null && !enemy.IsDead())
                {
                    Vector3 enemyPosition = new(transform.position.x, 1, transform.position.z + 1);
                    enemy.transform.SetPositionAndRotation(enemyPosition, Quaternion.identity);
                    enemy.TakeDamage(this);
                }
            }
        }

        internal void DamageSelf(int value)
        {
            float originalAtk = GetMaxStat(FighterStats.Attack);
            ModifyStat(FighterStats.Health, -value);
        }
    }
}