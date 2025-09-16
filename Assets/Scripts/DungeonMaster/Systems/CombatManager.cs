using Assets.PlayerCharacter;
using System;
using UnityEngine;
using static Assets.Utilities.GameObjectUtilities;

namespace Assets.Combat
{
    public class CombatManager : MonoBehaviour
    {
        public event Action<Enemy> EnemyDefeated;
        public event Action<PlayableFighter> PlayerDefeated;

        public void OnNewDungeon()
        {
            // Listen to all enemies defeat event.
            foreach (var enemy in GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None))
            {
                enemy.GetComponent<Fighter>().OnDefeat += HandleEnemyDefeated;
            }

            var player = FindComponentByTag<PlayableFighter>("Player");

            // Reset player OnDefeat event handler.
            player.OnDefeat -= HandlePlayerDefeated;
            player.OnDefeat += HandlePlayerDefeated;
        }

        private void HandleEnemyDefeated(Fighter fighter)
        {
            EnemyDefeated?.Invoke((Enemy)fighter);
        }

        private void HandlePlayerDefeated(Fighter fighter)
        {
            PlayerDefeated?.Invoke((PlayableFighter)fighter);
        }
    }
}