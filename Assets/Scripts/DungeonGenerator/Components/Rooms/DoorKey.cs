using Assets.PlayerCharacter;
using Assets.PlayerCharacter.Resources;

namespace Assets.DungeonGenerator.Components
{
    public class DoorKey : UsableItem
    {
        public override void Use(PlayerController player)
        {
            player.GetComponent<PlayerInventory>().Add(this);
        }
    }
}