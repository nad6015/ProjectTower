using Assets.Combat;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.PlayerCharacter
{
    public class PlayableFighter : Fighter
    {
        [SerializeField]
        private int maxCombo = 3;

        [SerializeField]
        private int _stamina = 5;

        [SerializeField]
        private float _staminaRegenRate = 0.1f;

        [SerializeField]
        private float _staminaRegenCooldownDuration = 1f;

        private Weapon _weapon;

        private bool _continueCombo;
        private const string _animCombo = "attack_combo";
        private int _comboCount = 0;
        private float _staminaRegenCooldown;

        public void Start()
        {
            _stats[FighterStats.STAMINA] = _stamina;
            _maxStats[FighterStats.STAMINA] = _stamina;
            _weapon = GetComponentInChildren<Weapon>();

            PlayerController controller = GetComponent<PlayerController>();
            controller.OnAttackPerformed += OnAttackPerformed;
        }

        protected override void Update()
        {
            base.Update();

            _staminaRegenCooldown -= Time.deltaTime;
            if (_stats[FighterStats.STAMINA] < _maxStats[FighterStats.STAMINA] && _staminaRegenCooldown <= 0)
            {
                IncreaseStat(FighterStats.STAMINA, 1);
                _staminaRegenCooldown = _staminaRegenCooldownDuration;
            }
        }

        protected override void AnimationEnded()
        {
            if (_continueCombo && _comboCount < maxCombo)
            {
                Attack();
            }
            else
            {
                _comboCount = 0;
            }

            _animator.SetInteger(_animCombo, _comboCount);
            _continueCombo = false;
        }

        private void OnAttackPerformed(InputAction.CallbackContext obj)
        {
            if (_stats[FighterStats.STAMINA] > 0)
            {
                if (!_isAttacking)
                {
                    Attack();
                    GetComponent<PlayerMovement>().speed = _stats[FighterStats.Speed];
                }
                else if (!_continueCombo)
                {
                    _continueCombo = true;
                    _comboCount++;
                    GetComponent<PlayerMovement>().speed = _stats[FighterStats.Speed];
                }

                IncreaseStat(FighterStats.STAMINA, -1);
            }
        }
    }
}