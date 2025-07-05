using System.Collections.Generic;
using UnityEngine;

namespace Assets.CombatSystem
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField]
        private int health = 5;

        [SerializeField]
        private int attack = 1;

        [SerializeField]
        private LayerMask layerMask;

        private Dictionary<FighterStats, int> stats = new();

        private Weapon weapon;
        private float weaponReach = 2f;
        private Vector3 attackPoint;

        private Animator animator;
        private bool isAttacking = false;
        private RaycastHit hit;

        private List<Fighter> hasDamaged = new List<Fighter>();

        void Awake()
        {
            stats[FighterStats.HEALTH] = health;
            stats[FighterStats.ATTACK] = attack;
            weapon = GetComponentInChildren<Weapon>();
            animator = GetComponentInChildren<Animator>();

            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            if (weapon != null)
            {
                weaponReach = weapon.WeaponReach();
                attackPoint = weapon.transform.localPosition;
            }
            else
            {
                attackPoint = transform.position;
            }
        }

        public int GetStat(FighterStats stat) => stats[stat];

        internal void Attack(Fighter fighter)
        {
            fighter.stats[FighterStats.HEALTH] -= stats[FighterStats.ATTACK];
            fighter.animator.SetTrigger("Injured");
            if (fighter.IsDead())
            {
                fighter.gameObject.SetActive(false); // TODO: Death indicator
                hasDamaged.Add(fighter);
            }
        }

        internal bool IsDead()
        {
            return stats[FighterStats.HEALTH] <= 0;
        }

        public void IsAttacking(bool isAttacking)
        {
            this.isAttacking = isAttacking;
        }

        private void Update()
        {
            if (isAttacking)
            {
                UpdateAttackPosition();
                Debug.DrawRay(attackPoint, transform.forward, Color.magenta, 2f);
                if (Physics.Raycast(attackPoint, transform.forward, out hit, weaponReach, layerMask))
                {
                    Fighter target = hit.rigidbody.GetComponent<Fighter>();
                    if (target != null && !hasDamaged.Contains(target))
                    {
                        Attack(target);
                        hasDamaged.Add(target);
                    }
                }
            }
            else
            {
                hasDamaged.Clear();
            }
        }

        private void UpdateAttackPosition()
        {
            if (weapon != null)
            {
                attackPoint = weapon.transform.position;
            }
            else
            {
                attackPoint = transform.position;
            }
        }
    }
}