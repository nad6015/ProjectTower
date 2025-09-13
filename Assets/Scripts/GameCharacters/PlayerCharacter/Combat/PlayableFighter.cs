using Assets.Combat;
using Assets.GameCharacters;
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

        private bool _continueCombo;
        private const string _animCombo = "attack_combo";
        private int _comboCount = 0;
        private float _staminaRegenCooldown;

        public override void Awake()
        {
            base.Awake();
            SetStat(FighterStats.Stamina, _stamina);

            PlayerController controller = GetComponent<PlayerController>();
            controller.OnAttackPerformed += OnAttackPerformed;
            controller.OnBlockPerformed += OnBlockPerformed;
            controller.OnBlockCancelled += BlockCancelled;
        }

        protected override void Update()
        {
            base.Update();

            _staminaRegenCooldown -= Time.deltaTime;
            if (_stats[FighterStats.Stamina] < _maxStats[FighterStats.Stamina] && _staminaRegenCooldown <= 0)
            {
                ModifyStat(FighterStats.Stamina, _staminaRegenRate);
                _staminaRegenCooldown = _staminaRegenCooldownDuration;
            }
        }

        /// <summary>
        /// Event handler for block input.
        /// </summary>
        /// <param name="context">the context from the input</param>
        private void OnBlockPerformed(InputAction.CallbackContext context)
        {
            _animator.SetBool("Block", true);
            _isDefending = true;
        }

        /// <summary>
        /// Event handler for when block input is cancelled.
        /// </summary>
        /// <param name="context">the context from the input</param>
        private void BlockCancelled(InputAction.CallbackContext context)
        {
            _animator.SetBool("Block", false);
            _isDefending = false;
        }


        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void AnimationEnded()
        {
            if (_continueCombo && _comboCount < maxCombo)
            {
                Attack();
                ModifyStat(FighterStats.Stamina, -1);
            }
            else
            {
                _comboCount = 0;
                _animator.SetBool("Attack", false);
            }

            _animator.SetInteger(_animCombo, _comboCount);
            _continueCombo = false;
        }

        private void OnAttackPerformed(InputAction.CallbackContext obj)
        {
            if (_stats[FighterStats.Stamina] > 0)
            {
                if (!_isAttacking)
                {
                    Attack();
                    ModifyStat(FighterStats.Stamina, -1);
                }
                else if (!_continueCombo)
                {
                    _continueCombo = true;
                    _comboCount = Mathf.Min(_comboCount + 1, maxCombo);
                }
            }
        }
    }
}