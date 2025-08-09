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

        /// <summary>
        /// Checks if this dungeon node has the same room type as the given room type.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsSameType(RoomType type)
        {
            return Type == type;
        }

        /// <summary>
        /// Changes this dungeon node's room type.
        /// </summary>
        /// <param name="type">the new type</param>
        public void ChangeType(RoomType type)
        {
            Type = type;
        }

        /// <summary>
        /// Reset the class' static id. Useful when regenerating a dungeon.
        /// </summary>
        public static void Reset()
        {
            _nodeId = 0;
        }
    }
}