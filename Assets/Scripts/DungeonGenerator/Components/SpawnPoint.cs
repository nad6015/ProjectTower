using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObjectToSpawn;

    public void Spawn()
    {
        GameObject.Instantiate(gameObjectToSpawn, gameObjectToSpawn.transform.position + transform.position, Quaternion.identity);
    }
}
