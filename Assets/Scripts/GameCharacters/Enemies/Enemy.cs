using Assets.Combat;
using Assets.GameCharacters;
using UnityEngine;

namespace Assets.Combat
{
    public class Enemy : Fighter
    {
        [SerializeField]
        private EnemyType _enemyType;

        [SerializeField]
        Projectile _projectile;

        private void Start()
        {
            AnimationEventsHandler animationEventsHandler = GetComponentInChildren<AnimationEventsHandler>();
            animationEventsHandler.OnHitHandler += OnHit;
        }

        private void OnHit()
        {
            switch (_enemyType)
            {
                case EnemyType.Ranger:
                {
                    var projectile = GameObject.Instantiate(_projectile, transform.position + Vector3.up, Quaternion.LookRotation(transform.forward));
                    projectile.SetInitialForward(transform.forward);
                    break;
                }
                case EnemyType.Spellcaster:
                {
                    Debug.Log("Spell casted");
                    break;
                }
                case EnemyType.Warrior:
                {
                    break;
                }
            }
        }
    }
}