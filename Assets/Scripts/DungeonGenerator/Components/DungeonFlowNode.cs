using System;

namespace Assets.Scripts.DungeonGenerator.Components
{
    public class DungeonFlowNode
    {
     
        public RoomType Type { get; }

        public DungeonFlowNode(string type)
        {
            switch (type.ToLower())
            {
                case "explore":
                {
                    Type = RoomType.EXPLORE;
                    break;
                }
                case "combat":
                {
                    Type = RoomType.COMBAT;
                    break;
                }
                case "mini-boss":
                {
                    Type = RoomType.MINI_BOSS;
                    break;
                }
                case "rest-point":
                {
                    Type = RoomType.REST_POINT;
                    break;
                }
                case "item":
                {
                    Type = RoomType.ITEM;
                    break;
                }
                case "start":
                {
                    Type = RoomType.START;
                    break;
                }
                case "end":
                {
                    Type = RoomType.END;
                    break;
                }
            }
        }

        public override bool Equals(object obj)
        {
            return obj is DungeonFlowNode room &&
                   Type == room.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type);
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}