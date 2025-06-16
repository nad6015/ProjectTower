using Assets.DungeonGenerator;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider collider)
    {
        collider.gameObject.SetActive(false);
    }
}
