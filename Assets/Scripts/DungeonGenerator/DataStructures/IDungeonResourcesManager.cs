using Assets.Interactables;
using Assets.PlayerCharacter.Resources;

namespace Assets.DungeonGenerator.DataStructures
{
    public interface IDungeonResourceManager
    {
        public Interactable TakeContainerItem();
        public UsableItem TakeTreasureChestItem();
    }
}