using System;
using UnityEngine;

namespace Assets.Combat
{
    public class CombatSystem : MonoBehaviour
    {
        public event Action<NpcFighter> EnemyDefeated;
        public event Action<Fighter> PlayerDefeated;

        public void Awake()
        {
        }

        private void Update()
        {
        
        }
    }
}