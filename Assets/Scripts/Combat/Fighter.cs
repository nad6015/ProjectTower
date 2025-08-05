using Assets.Scripts.Character;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.CombatSystem
{
    public abstract class Fighter : MonoBehaviour
    {
        public event Action OnHealthChange;

        [SerializeField]
        private int health = 5;

        [SerializeField]
        private int attack = 1;

        [SerializeField]
        private LayerMask layerMask;

        [SerializeField]
        protected BoxCollider _hitbox;

        [SerializeField]
        private float _attackCooldown = 0.5f;

        private Dictionary<FighterStats, int> stats = new();

        protected Animator _animator;
        protected bool _isAttacking = false;
        private float _attackCooldownDuration = 0.5f;

        private Weapon weapon;

        private AnimationEventsHandler _animationEvents;
        private const string _attackParam = "Attack";

        public void Awake()
        {
            stats[FighterStats.HEALTH] = health;
            stats[FighterStats.ATTACK] = attack;

            weapon = GetComponentInChildren<Weapon>();
            _animator = GetComponentInChildren<Animator>();

            _animationEvents = GetComponent<AnimationEventsHandler>();

            if (_animationEvents == null)
            {
                _animationEvents = GetComponentInChildren<AnimationEventsHandler>();
            }

            _hitbox.includeLayers = layerMask;
            _hitbox.enabled = false;

            _animationEvents.OnAnimationEndHandler += OnAnimationEnd;
        }

        public int GetStat(FighterStats stat) => stats[stat];

        public void Attack()
        {
            if (_attackCooldown <= 0)
            {
                _isAttacking = true;
                _animator.SetTrigger("Attack");
                _animator.SetBool(_attackParam, _isAttacking);
                _hitbox.enabled = true;
            }
        }

        protected void TakeDamage(Fighter attacker)
        {
            stats[FighterStats.HEALTH] -= attacker.stats[FighterStats.ATTACK];
            _animator.SetTrigger("Injured");
            OnHealthChange?.Invoke();

            if (IsDead())
            {
                gameObject.SetActive(false); // TODO: Death indicator
            }
        }

        public void Heal(int amountToHeal)
        {
            stats[FighterStats.HEALTH] += amountToHeal;
            OnHealthChange?.Invoke();

            // TODO: Healling vfx
        }

        public bool IsDead()
        {
            return stats[FighterStats.HEALTH] <= 0;
        }

        private void Update()
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
            Fighter attacker = collider.GetComponentInParent<Fighter>();

            if (attacker != null)
            {
                TakeDamage(attacker);
            }
        }

        private void OnAnimationEnd()
        {
            _hitbox.enabled = false;
            _isAttacking = false;
            _attackCooldown = _attackCooldownDuration;
            AnimationEnded();
            _animator.SetBool(_attackParam, _isAttacking);
        }

        protected void ResetAttackCooldown()
        {
            _attackCooldown = 0;
        }

        protected bool IsAttacking()
        {
            return _animator.GetBool(_attackParam);
        }

        protected abstract void AnimationEnded();
    }
}