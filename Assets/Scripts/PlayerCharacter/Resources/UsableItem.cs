using UnityEngine;

namespace Assets.PlayerCharacter.Resources
{
    public abstract class UsableItem : MonoBehaviour
    {
        [field: SerializeField]
        public int Type { get; }

        public abstract void Use(PlayerController player);
    }
}