using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat
{
    public abstract class Fighter : MonoBehaviour
    {
        public event Action<FighterStats> OnStatChange;
        public event Action<Fighter> OnDefeat;

        [SerializeField]
        private int health = 5;

        [SerializeField]
        private int attack = 1;

        [SerializeField]
        private int speed = 3;

        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        protected BoxCollider _hitbox;

        [SerializeField]
        private float _attackCooldownDuration = 0.5f;

        protected Animator _animator;
        protected bool _isAttacking = false;
        protected Dictionary<FighterStats, float> _stats = new();
        protected Dictionary<FighterStats, float> _maxStats = new();
        protected bool _isDefending = false;

        private float _attackCooldown;
        private const string _attackParam = "Attack";
        private readonly List<Fighter> _hasAttacked = new();

        public virtual void Awake()
        {
            SetStat(FighterStats.Health, health);
            SetStat(FighterStats.Attack, attack);
            SetStat(FighterStats.Speed, speed);

            _animator = GetComponentInChildren<Animator>();

            _hitbox.enabled = false;
            _hitbox.includeLayers = layerMask;
            // Binary inversion operator referenced from - https://discussions.unity.com/t/ignore-one-layermask-question/186174
            _hitbox.excludeLayers = ~layerMask;
            _attackCooldown = _attackCooldownDuration;
        }

        /// <summary>
        /// Gets the current value of a fighter stat. This can differ from the max stat
        /// if the stat is reduced or increased. For example, a fighter that takes damage will have their current health reduced
        /// but the max health they can have will stay the same.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns>the value of the stat</returns>
        public float GetStat(FighterStats stat) => _stats[stat];

        /// <summary>
        /// Gets the max value of a fighter stat.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns>the max value of the stat</returns>
        public float GetMaxStat(FighterStats stat) => _maxStats[stat];

        /// <summary>
        /// Called when a fighter attacks, but only runs the attack logic if the attack cooldown is equal to or less than zero.
        /// </summary>
        public void Attack()
        {
            if (_attackCooldown <= 0)
            {
                _isAttacking = true;
                _animator.SetTrigger(_attackParam);
                _animator.SetBool(_attackParam, _isAttacking);
                _hitbox.enabled = true;
                var colliders = Physics.OverlapBox(transform.position + transform.forward, Vector3.one, Quaternion.identity, layerMask);
                foreach (var item in colliders)
                {
                    if (item.TryGetComponent<Fighter>(out var target))
                    {
                        target.TakeDamage(this);
                    }
                }
                _attackCooldown = _attackCooldownDuration;
            }

        }

        public void Heal(int amountToHeal)
        {
            _stats[FighterStats.Health] += amountToHeal;
            _stats[FighterStats.Health] = Math.Min(_stats[FighterStats.Health], _maxStats[FighterStats.Health]);

            OnStatChange?.Invoke(FighterStats.Health);
        }

        /// <summary>
        /// Checks if the fighter is dead or not.
        /// </summary>
        /// <returns>true if fighter's health is less than or equal to 0</returns>
        public bool IsDead()
        {
            return _stats[FighterStats.Health] <= 0;
        }

        /// <summary>
        /// Checks if the fighter is attacking based on its current animation state.
        /// </summary>
        /// <returns>whether the Attack animation state is true or not.</returns>
        public bool IsAttacking()
        {
            return _animator.GetBool(_attackParam);
        }

        /// <summary>
        /// Applies damage to the fighter.
        /// </summary>
        /// <param name="attacker">the fighter attacking this fighter</param>
        protected void TakeDamage(Fighter attacker)
        {
            float dmgDealt = attacker.GetStat(FighterStats.Attack) * (_isDefending ? 0.5f : 1f);

            _stats[FighterStats.Health] -= dmgDealt;
            _animator.SetTrigger("Injured");
            OnStatChange?.Invoke(FighterStats.Health);

            if (IsDead())
            {
                OnDefeat?.Invoke(this);
                _animator.SetTrigger("Defeated");
            }
        }

        protected virtual void Update()
        {
            if (_attackCooldown > 0f && !_isAttacking)
            {
                _attackCooldown -= Time.deltaTime;
            }
        }

        /// <summary>
        /// Called when an OnAnimationEnd event is invoked from an animation.
        /// Due to the way Unity animation events work, an external class (GameCharacterController)
        /// must register this callback for the Fighter class.
        /// </summary>
        public void OnAnimationEnd()
        {
            _hitbox.enabled = false;
            _isAttacking = false;
            _attackCooldown = _attackCooldownDuration;
            AnimationEnded();
            _hasAttacked.Clear();
        }

        /// <summary>
        /// A callback for inheriting classes to do some work during the OnAnimationEnd handler.
        /// </summary>
        protected virtual void AnimationEnded()
        {
            // NO-OP
        }

        /// <summary>
        /// Modifies the fighter's specified stat by a given amount. Does not affect the max stat.
        /// </summary>
        /// <param name="stat">the stat to modify</param>
        /// <param name="value">the amount to modify the stat by</param>
        protected void ModifyStat(FighterStats stat, float value)
        {
            _stats[stat] += value;

            if (_stats[stat] < 0)
            {
                _stats[stat] = 0;
            }

            OnStatChange?.Invoke(stat);
        }

        /// <summary>
        /// Sets the given stat's max and current value to a new value.
        /// </summary>
        /// <param name="stat">the stat to set</param>
        /// <param name="value">the amount to set the stat by</param>
        protected void SetStat(FighterStats stat, float value)
        {
            _stats[stat] = value;
            _maxStats[stat] = value;
            OnStatChange?.Invoke(stat);
        }
    }
}