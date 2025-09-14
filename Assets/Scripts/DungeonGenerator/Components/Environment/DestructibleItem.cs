using Assets.DungeonGenerator.DataStructures;
using Assets.Interactables;
using Assets.Utilities;
using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    public partial class DestructibleItem : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _itemBreakClip;

        [SerializeField]
        private GameObject _mesh;

        private AudioSource _audioSource;
        private IDungeonResourceManager _manager;
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.clip = _itemBreakClip;
            _audioSource.volume = 0.5f;
            _manager = GameObjectUtilities.FindComponentByTag<IDungeonResourceManager>("ResourceSystem");
        }

        public void OnTriggerEnter(Collider other)
        {
            Transform parent = other.transform.parent;
            if (parent.CompareTag("Player"))
            {
                _audioSource.Play();
                Interactable item = _manager.TakeContainerItem();
                Debug.Log(item);
                if (item != null)
                {
                    SpawnPoint spawnPoint = GetComponent<SpawnPoint>();
                    spawnPoint.SetItemSpawn(item.gameObject);
                    spawnPoint.Spawn();
                }

                GetComponent<Collider>().enabled = false;
                _mesh.SetActive(false);
                enabled = false;
            }
        }
    }
}