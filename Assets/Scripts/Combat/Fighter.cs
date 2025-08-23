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

        private float _attackCooldown;
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

            _hitbox.enabled = false;
            _hitbox.includeLayers = layerMask;
            // Binary inversion operator referenced from - https://discussions.unity.com/t/ignore-one-layermask-question/186174
            _hitbox.excludeLayers = ~layerMask;
            _attackCooldown = _attackCooldownDuration;
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
                var colliders = Physics.OverlapBox(transform.position, Vector3.one, Quaternion.identity, layerMask);
                foreach (var item in colliders)
                {
                    Fighter target = item.GetComponent<Fighter>();
                    if (target != null)
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
        /// TODO: Explain why this method is public
        /// </summary>
        public void OnAnimationEnd()
        {
            _hitbox.enabled = false;
            _isAttacking = false;
            _attackCooldown = _attackCooldownDuration;
            _stats[FighterStats.Speed] = _maxStats[FighterStats.Speed];
            AnimationEnded();
            _animator.SetBool(_attackParam, _isAttacking);
            _hasAttacked.Clear();
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