using UnityEngine;

namespace Assets.PlayerCharacter
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        private float distanceFromPlayer = 10f;
        [SerializeField]
        private float distanceFromPlayerZ = -4f;
        [SerializeField]
        private float cameraTilt = 15f;

        Transform player;
        void Start()
        {
            player = GameObject.FindWithTag("Player").transform;

            transform.Rotate(Vector3.right, cameraTilt);
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = new Vector3(player.position.x, distanceFromPlayer, distanceFromPlayerZ + player.position.z);
        }
    }
}