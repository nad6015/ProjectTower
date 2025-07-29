using Assets.CombatSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.PlayerCharacter
{
    public class PlayableFighter : Fighter
    {
        [SerializeField]
        private int maxCombo = 3;

        private bool _continueCombo;
        private const string _animCombo = "attack_combo";
        private int comboCount = 0;

        void Start()
        {
            PlayerController controller = GetComponent<PlayerController>();
            controller.OnAttackPerformed += OnAttackPerformed;
        }

        private void OnAttackPerformed(InputAction.CallbackContext obj)
        {
            if (!_isAttacking)
            {
                Attack();
              
            }
            else if (!_continueCombo)
            {
                _continueCombo = true;
                comboCount++;
            }
        }

        protected override void AnimationEnded()
        {
            if (_continueCombo && comboCount < maxCombo)
            {
                ResetAttackCooldown();
                Attack();
            }
            else
            {
                comboCount = 0;
            }

            _animator.SetInteger(_animCombo, comboCount);
            _continueCombo = false;
        }
    }
}