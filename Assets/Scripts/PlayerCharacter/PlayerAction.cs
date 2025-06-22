using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Assets.CombatSystem;

namespace Assets.PlayerCharacter
{
    public class PlayerAction : MonoBehaviour
    {
        public event EventHandler PlayerAttacks;
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        internal void OnAttackCancelled(InputAction.CallbackContext context)
        {
            animator.SetBool("Attack", false);
            GetComponent<Fighter>().IsAttacking(false);

        }

        internal void OnAttackPerformed(InputAction.CallbackContext context)
        {
            animator.SetBool("Attack", true);
            GetComponent<Fighter>().IsAttacking(true);
        }

        public void Hit()
        {
            // TODO: Play audio
        }
    }
}