namespace Assets.DungeonGenerator.Components.Rooms
{
    public class RestPointRoom : DungeonRoom
    {
        internal override void Populate(DungeonRepresentation dungeon)
        {
           PlaceProps(dungeon);
           PlaceProps(dungeon);
        }
    }
}