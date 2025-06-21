using CombatSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
        PlayerAttacks.Invoke(this, new EventArgs());
    }
}
