using Assets.Interactables;

namespace Assets.DungeonGenerator.DataStructures
{
    public interface IDungeonResourceManager
    {
        public Interactable TakeContainerItem();
    }
}