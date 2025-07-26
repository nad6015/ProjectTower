using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameObjectToSpawn;

    /// <summary>
    /// Spawns a new gameobject at this gameobject's position.
    /// </summary>
    public void Spawn()
    {
        Vector3 spawnPoint = new(transform.position.x, _gameObjectToSpawn.transform.position.y, transform.position.z);
        GameObject.Instantiate(_gameObjectToSpawn, spawnPoint, Quaternion.identity);
    }

    /// <summary>
    /// Spawns an existing gameobject at this gameobject's position.
    /// </summary>
    public void Spawn(Transform transform)
    {
        Vector3 spawnPoint = new(this.transform.position.x, transform.position.y, this.transform.position.z);
        transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
    }
}
