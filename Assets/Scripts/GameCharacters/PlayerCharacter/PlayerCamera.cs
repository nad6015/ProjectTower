using UnityEngine;

namespace Assets.PlayerCharacter
{
    public class PlayerCamera : MonoBehaviour
    {
        // Field syntax referenced from - https://discussions.unity.com/t/serialize-c-properties-how-to-with-code/683762/4
        [field: SerializeField]
        public float DistanceFromPlayer { get; private set; }
        
        [field: SerializeField]
        public float DistanceFromPlayerZ { get; private set; }

        [SerializeField]
        private float cameraTilt = 15f;

        private Transform _player;
        private Quaternion _initialRotation;
        private Camera _camera;
        void Awake()
        {
            _player = GameObject.FindWithTag("Player").transform;

            transform.Rotate(Vector3.right, cameraTilt);
            _initialRotation = transform.rotation;
            _camera = GetComponent<Camera>();
        }

        void Update()
        {
            transform.SetPositionAndRotation(new Vector3(
                _player.position.x, 
                DistanceFromPlayer,
                _player.position.z - DistanceFromPlayerZ), 
                _initialRotation);
        }

        public void UpdateBackgroundColor(Color newColor)
        {
            _camera.backgroundColor = newColor;
        }
    }
}