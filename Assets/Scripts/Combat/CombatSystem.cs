using System;
using UnityEngine;

namespace Assets.Combat
{
    public class CombatSystem : MonoBehaviour
    {
        public event Action<Fighter> EnemyDefeated;
        public event Action<Fighter> PlayerDefeated;

        public void Awake()
        {
        }

        public bool IsPlayerDead()
        {
            throw new NotImplementedException();
        }

        private void Update()
        {
        
        }
    }
}