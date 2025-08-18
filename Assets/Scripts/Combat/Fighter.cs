using Assets.Character;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat
{
    public abstract class Fighter : MonoBehaviour
    {
        public event Action<FighterStats> OnStatChange;

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

        [SerializeField]
        private float _attackCommitment = 0.5f;

        protected Animator _animator;
        protected bool _isAttacking = false;
        protected Dictionary<FighterStats, int> _stats = new();
        protected Dictionary<FighterStats, int> _maxStats = new();

        private float _attackCooldown = 0.5f;
        private AnimationEventsHandler _animationEvents;
        private const string _attackParam = "Attack";
        private List<Fighter> _hasAttacked = new List<Fighter>();

        public void Awake()
        {
            _stats[FighterStats.Health] = health;
            _stats[FighterStats.Attack] = attack;
            _stats[FighterStats.Speed] = speed;

            _maxStats[FighterStats.Health] = health;
            _maxStats[FighterStats.Attack] = attack;
            _maxStats[FighterStats.Speed] = speed;

            _animator = GetComponentInChildren<Animator>();

            _animationEvents = GetComponent<AnimationEventsHandler>();

            if (_animationEvents == null)
            {
                _animationEvents = GetComponentInChildren<AnimationEventsHandler>();
            }

            _hitbox.enabled = false;
            _hitbox.includeLayers = layerMask;
            // Binary inversion operator referenced from - https://discussions.unity.com/t/ignore-one-layermask-question/186174
            _hitbox.excludeLayers = ~layerMask;
            _animationEvents.OnAnimationEndHandler += OnAnimationEnd;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public int GetStat(FighterStats stat) => _stats[stat];

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public int GetMaxStat(FighterStats stat) => _maxStats[stat];

        public void Attack()
        {
            if (_attackCooldown <= 0)
            {
                _isAttacking = true;
                _animator.SetTrigger(_attackParam);
                _animator.SetBool(_attackParam, _isAttacking);
                _hitbox.enabled = true;
            }
        }

        public void Heal(int amountToHeal)
        {
            _stats[FighterStats.Health] += amountToHeal;
            _stats[FighterStats.Health] = Math.Min(_stats[FighterStats.Health], _maxStats[FighterStats.Health]);
            // TODO: Healing vfx

            OnStatChange?.Invoke(FighterStats.Health);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public bool IsDead()
        {
            return _stats[FighterStats.Health] <= 0;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public bool IsAttacking()
        {
            return _isAttacking;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="attacker"></param>
        protected void TakeDamage(Fighter attacker)
        {
            _stats[FighterStats.Health] -= attacker._stats[FighterStats.Attack];
            _animator.SetTrigger("Injured");
            OnStatChange?.Invoke(FighterStats.Health);

            if (IsDead())
            {
                gameObject.SetActive(false); // TODO: Death indicator
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
        /// When a fighter attacks, their hitbox is enabled. If any other fighter is in the hitbox,
        /// this Unity Message is triggered on them. So, each fighter has to call Attack on themselves in order to
        /// apply damage.
        /// </summary>
        /// <param name="collider">The hitbox of the attacking fighter</param>
        private void OnTriggerEnter(Collider collider)
        {
            Fighter target = collider.GetComponentInParent<Fighter>();

            if (target != null && target.IsAttacking() && !target._hasAttacked.Contains(this))
            {
                TakeDamage(target);
                target._hasAttacked.Add(this);
            }
        }

        private void OnAnimationEnd()
        {
            _hitbox.enabled = false;
            _isAttacking = false;
            _attackCooldown = _attackCooldownDuration;
            _stats[FighterStats.Speed] = _maxStats[FighterStats.Speed];
            AnimationEnded();
            _animator.SetBool(_attackParam, _isAttacking);
            _hasAttacked.Clear();
        }

        protected void ResetAttackCooldown()
        {
            _attackCooldown = 0;
        }

        protected abstract void AnimationEnded();

        protected void IncreaseStat(FighterStats stat, int value)
        {
            _stats[stat] += value;

            if (_stats[stat] < 0)
            {
                _stats[stat] = 0;
            }

            OnStatChange?.Invoke(stat);
        }
    }
}