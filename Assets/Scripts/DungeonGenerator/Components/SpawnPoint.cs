using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObjectToSpawn;

    public void Spawn(Vector3 position)
    {
        Debug.Log(position);
        GameObject.Instantiate(gameObjectToSpawn, position, Quaternion.identity);
    }
}
