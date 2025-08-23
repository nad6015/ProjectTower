using Assets.DungeonGenerator;

namespace Assets.GameManager
{
    public interface IModifiableSystem
    {
        void Modify(DungeonComponents modifier);
    }
}