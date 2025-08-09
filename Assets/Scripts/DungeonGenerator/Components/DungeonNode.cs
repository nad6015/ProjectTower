using System;
using System.Collections.Generic;

namespace Assets.Scripts.DungeonGenerator.Components
{
    public class DungeonNode
    {
        private static int _nodeId = 0;
        public RoomType Type { get; private set; }
        public int Id { get; private set; }
        public List<DungeonNode> LinkedNodes { get; private set; }

        public DungeonNode(string type)
        {
            LinkedNodes = new List<DungeonNode>();
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
            Id = _nodeId++;
        }

        public DungeonNode(RoomType type)
        {
            LinkedNodes = new List<DungeonNode>();
            Type = type;
            Id = _nodeId++;
        }

        public override bool Equals(object obj)
        {
            return obj is DungeonNode room &&
                   Type == room.Type && Id == room.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Id);
        }

        public override string ToString()
        {
            return "Node ID: " + Id + ", Node Type: " + Type.ToString();
        }

        public bool IsSameType(DungeonNode other)
        {
            return Type == other.Type;
        }

        public void Copy(DungeonNode other)
        {
            Type = other.Type;
        }

        public static void Reset()
        {
            _nodeId = 0;
        }
    }
}