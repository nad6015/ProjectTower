using Assets.Combat;
using Assets.PlayerCharacter.Resources;
using UnityEngine;

namespace Assets.PlayerCharacter
{
    public class StatIncreaser : UsableItem
    {
        [field: SerializeField]
        public FighterStats Stats { get; private set; }
        private const int _statIncrease = 1;
        public override void Use(PlayerController player)
        {
            player.GetComponent<PlayableFighter>().IncreaseStat(Stats, _statIncrease);
        }
    }
}