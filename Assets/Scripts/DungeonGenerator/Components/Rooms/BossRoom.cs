using UnityEngine;

namespace Assets.DungeonGenerator.Components.Rooms
{
    public class BossRoom : DungeonRoom
    {
        internal override void Populate(DungeonRepresentation dungeon)
        {
            float spawnRate = dungeon.Parameter<int>(DungeonParameter.EnemySpawnRate) / 100f;
            Range<int> enemiesPerRoom = dungeon.Parameter<Range<int>>(DungeonParameter.EnemiesPerRoom);

            if (Random.value > spawnRate || dungeon.Components.enemies.Count == 0)
            {
                return;
            }
            int count = Random.Range(enemiesPerRoom.min, enemiesPerRoom.max);
            GameObject enemy = dungeon.Components.enemies[0];
            Contents.Add(new(enemy, enemy.transform.position + PointUtils.RandomPointWithinBounds(Bounds)));
        }
    }
}