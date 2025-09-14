using Assets.Combat;
using Assets.PlayerCharacter.Resources;

namespace Assets.PlayerCharacter
{
    public class StatIncreaser : UsableItem
    {
        public FighterStats Stats { get; protected set; }
        private const int _statIncrease = 1;
        public override void Use(PlayerController player)
        {
            player.GetComponent<PlayableFighter>().IncreaseStat(Stats, _statIncrease);
        }
    }
}