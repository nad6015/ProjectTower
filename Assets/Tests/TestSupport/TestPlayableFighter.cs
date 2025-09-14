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
            TestNpcFighter enemy = GameObject.FindFirstObjectByType<TestNpcFighter>();
            if (enemy != null)
            {
                Vector3 enemyPosition = new(transform.position.x, 1, transform.position.z + 1);
                enemy.transform.SetPositionAndRotation(enemyPosition, Quaternion.identity);
                enemy.TakeDamage(this);
                Debug.Log(enemy.GetStat(FighterStats.Health));
            }
        }

        internal void DamageSelf(int value)
        {
            float originalAtk = GetMaxStat(FighterStats.Attack);
            SetStat(FighterStats.Attack, 0);
            ModifyStat(FighterStats.Health, -value);
            TakeDamage(this);
            SetStat(FighterStats.Attack, originalAtk);
        }
    }
}