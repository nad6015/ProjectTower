using Assets.PlayerCharacter;
using UnityEngine;

namespace Assets.Combat
{
    internal class Projectile : MonoBehaviour
    {
        [SerializeField]
        private float _projectileSpeedMultipler = 1.5f;

        [SerializeField]
        private int _dmg = 1;

        [SerializeField]
        private float _timeToRemoval = 1.5f;

        [SerializeField]
        private bool _isSpell = false;

        private bool _shouldDestroy = false;
        private float _projectileTimedown = 5f;

        private Vector3 _initialForward;

        public void SetInitialForward(Vector3 initialForward)
        {
            _initialForward = initialForward;

        }

        private void Update()
        {
            if (_shouldDestroy && _isSpell)
            {
                if (_timeToRemoval <= 0)
                {
                    Destroy(gameObject);
                }
                else
                {
                    _timeToRemoval -= Time.deltaTime;
                }
            }
            else
            {
                transform.position += _initialForward * (Time.deltaTime * _projectileSpeedMultipler);
                _projectileTimedown -= Time.deltaTime;
                // If the projectile exists too long, then destroy it
                if (_projectileTimedown <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayableFighter>().TakeProjectileDamage(_dmg);
            }
            _shouldDestroy = true;
        }
    }
}