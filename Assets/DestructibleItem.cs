using UnityEngine;

public class DestructibleItem : MonoBehaviour
{
    [SerializeField]
    private GameObject _destroyedItem;

    public void OnTriggerEnter(Collider other)
    {
        Transform parent = other.transform.parent;
        if (parent.CompareTag("Player"))
        {
            GetComponent<SpawnPoint>().Spawn();
            Destroy(gameObject);
        }
    }
}
