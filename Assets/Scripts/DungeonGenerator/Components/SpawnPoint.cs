using UnityEngine;

namespace Assets.DungeonGenerator.Components {
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField]
        private GameObject _gameObjectToSpawn;

        [SerializeField]
        private bool _shouldSpawnOnStart = false;

        private void Start()
        {
            if (_shouldSpawnOnStart)
            {
                Spawn();
            }
        }

        /// <summary>
        /// Spawns a new gameobject at this gameobject's position.
        /// </summary>
        /// <returns>the object spawned.</returns>
        public GameObject Spawn()
        {
            Vector3 spawnPoint = new(transform.position.x, transform.position.y, transform.position.z);
            return GameObject.Instantiate(_gameObjectToSpawn, spawnPoint, Quaternion.identity);
        }

        /// <summary>
        /// Spawns an existing gameobject at this gameobject's position.
        /// </summary>
        public void Spawn(Transform transform)
        {
            Vector3 spawnPoint = new(this.transform.position.x, transform.position.y, this.transform.position.z);
            transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
        }

        /// <summary>
        /// Sets the item to spawn.
        /// </summary>
        public void SetItemSpawn(GameObject newItemSpawn)
        {
            _gameObjectToSpawn = newItemSpawn;
        }
    }
}
