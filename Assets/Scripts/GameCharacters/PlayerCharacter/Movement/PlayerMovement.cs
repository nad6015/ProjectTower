using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.PlayerCharacter
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        float speedReduction = 0.5f;
        internal float Speed { get; set; }

        private CharacterController _characterController;
        private PlayableFighter _fighter;
        private Vector3 _newPos;
        private Vector3 _lastPos;
        private Quaternion _currentRotation;
        private Animator _animator;
        private bool _isDefending = false;

        void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _fighter = GetComponent<PlayableFighter>();
            Speed = _fighter.GetMaxStat(Combat.FighterStats.Speed);

            PlayerController playerController = GetComponent<PlayerController>();
            playerController.OnMovePerformed += OnMovePerformed;
            playerController.OnMoveCancelled += OnMoveCancelled;
            playerController.OnBlockPerformed += OnBlockPerformed;
            playerController.OnBlockCancelled += OnBlockCancelled;

            _animator = GetComponentInChildren<Animator>();
            _animator.SetFloat("MotionSpeed", 1);

            Cursor.lockState = CursorLockMode.Confined;
            _currentRotation = transform.rotation;
        }

        /// <summary>
        /// Event handler for block input.
        /// </summary>
        /// <param name="context">the context from the input</param>
        private void OnBlockPerformed(InputAction.CallbackContext context)
        {
            _isDefending = true;
            Speed *= speedReduction;
            _lastPos = _newPos;
        }

        /// <summary>
        /// Event handler for when block input is cancelled.
        /// </summary>
        /// <param name="context">the context from the input</param>
        private void OnBlockCancelled(InputAction.CallbackContext context)
        {
            _isDefending = false;
            Speed = _fighter.GetMaxStat(Combat.FighterStats.Speed);
        }

        /// <summary>
        /// Event handler for movement input.
        /// </summary>
        /// <param name="context">the context from the input</param>
        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            _newPos.x = value.x;
            _newPos.z = value.y;
            _animator.SetFloat("Speed", Speed);
        }

        /// <summary>
        /// Event handler for when movement is cancelled.
        /// </summary>
        /// <param name="context">the context from the input</param>
        private void OnMoveCancelled(InputAction.CallbackContext context)
        {
            _newPos = Vector3.zero;
            _animator.SetFloat("Speed", 0);
        }

        void Update()
        {
            if (!_isDefending)
            { 
            _currentRotation = Quaternion.LookRotation(_newPos == Vector3.zero ? transform.forward : _newPos, Vector3.up);
            }
            else
            {
                _currentRotation = Quaternion.LookRotation(_lastPos == Vector3.zero? transform.forward : _lastPos, Vector3.up);
            }

            transform.rotation = _currentRotation;
            _characterController.SimpleMove(Speed * _newPos);
        }
    }
}
