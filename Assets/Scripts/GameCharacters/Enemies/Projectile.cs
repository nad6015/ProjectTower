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

        private Vector3 _initialForward;

        public void SetInitialForward(Vector3 initialForward)
        {
            _initialForward = initialForward;
        }

        private void Update()
        {
            transform.position += _initialForward * (Time.deltaTime * _projectileSpeedMultipler);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayableFighter>().TakeProjectileDamage(_dmg);
            }
            Destroy(gameObject);
        }
    }
}