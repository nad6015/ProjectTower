using Assets.Combat;
using UnityEngine;

namespace Assets.Scripts.Combat.Resources
{
    public abstract class Item : MonoBehaviour
    {
        [field: SerializeField]
        public int Type { get; }

        public abstract void Use(Fighter player);

        private float _rotationSpeed = 1f;
        private void Update()
        {
            transform.Rotate(Vector3.up, _rotationSpeed);
        }
    }
}