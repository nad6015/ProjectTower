using Assets.GameCharacters;
using Assets.PlayerCharacter;
using UnityEngine;
using static Assets.Utilities.GameObjectUtilities;

namespace Assets.Combat
{
    public class Enemy : Fighter
    {
        [SerializeField]
        private EnemyType _enemyType;

        [SerializeField]
        private Projectile _projectile;

        private GameObject _targetIndicator;

        private void Start()
        {
            AnimationEventsHandler animationEventsHandler = GetComponentInChildren<AnimationEventsHandler>();
            animationEventsHandler.OnHitHandler += OnHit;
            animationEventsHandler.OnAttackStartHandler += OnAttackStart;
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
                    GameObject.Instantiate(_projectile, _targetIndicator.transform.position, Quaternion.LookRotation(transform.forward));
                    Destroy(_targetIndicator);
                    break;
                }
                case EnemyType.Warrior:
                {
                    break;
                }
            }
        }

        private void OnAttackStart(GameObject gameObj)
        {
            if (_enemyType == EnemyType.Spellcaster)
            {
                Vector3 pos = FindComponentByTag<Transform>("Player").position;
                pos.y = 0.1f;
                _targetIndicator = GameObject.Instantiate(gameObj, pos, Quaternion.identity);
            }
        }
    }
}