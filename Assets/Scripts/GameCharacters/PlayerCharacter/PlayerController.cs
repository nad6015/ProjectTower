using Assets.Combat;
using Assets.GameCharacters;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.PlayerCharacter
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerMovement))]
    public class PlayerController : GameCharacterController
    {
        public event Action<InputAction.CallbackContext> OnInteract;
        public event Action<InputAction.CallbackContext> OnAttackPerformed;
        public event Action<InputAction.CallbackContext> MovePerformed;
        public event Action<InputAction.CallbackContext> MoveCancelled;
        public event Action<InputAction.CallbackContext> OnBlockPerformed;
        public event Action<InputAction.CallbackContext> OnBlockCancelled;

        [SerializeField]
        private GameObject _camera;

        [SerializeField]
        private PlayerHUD _hud;

        public PlayerCamera Camera { get; private set; }

        private Animator _animator;

        private InputSystemActions _actions;


        // Using both Awake and Start here to first initialise the controller's values (Awake) for the OnEnable and OnDisable
        // and then to set the referenced scripts' values (Start).
        void Awake()
        {
            _animator = GetComponentInChildren<Animator>();

            _actions = new InputSystemActions();
            Camera = Instantiate(_camera).GetComponent<PlayerCamera>();
        }

        protected override void Start()
        {
            base.Start();
        }

        private void OnEnable()
        {
            _actions.Player.Move.Enable();
            _actions.Player.Move.performed += MovePerformed;
            _actions.Player.Move.canceled += MoveCancelled;

            _actions.Player.Attack.Enable();
            _actions.Player.Attack.performed += AttackPerformed;

            _actions.Player.Interact.Enable();
            _actions.Player.Interact.performed += InteractPerformed;

            _actions.Player.Block.Enable();
            _actions.Player.Block.performed += BlockPerformed;
            _actions.Player.Block.canceled += BlockCancelled;

            // Reset animator on enable
            _animator.Play("Idle Walk Run Blend", -1, 0);
            _animator.SetFloat("MotionSpeed", 1);
        }

        private void OnDisable()
        {
            _actions.Player.Move.Disable();
            _actions.Player.Move.performed -= MovePerformed;
            _actions.Player.Move.canceled -= MoveCancelled;

            _actions.Player.Attack.Disable();
            _actions.Player.Attack.performed -= AttackPerformed;

            _actions.Player.Interact.Disable();
            _actions.Player.Interact.performed -= InteractPerformed;

            _actions.Player.Block.Disable();
            _actions.Player.Block.performed -= BlockPerformed;
        }

        /// <summary>
        /// Event handler for attack input.
        /// </summary>
        /// <param name="context">The input context</param>
        private void AttackPerformed(InputAction.CallbackContext context)
        {
            OnAttackPerformed?.Invoke(context);
        }

        /// <summary>
        /// Event handler for interact input.
        /// </summary>
        /// <param name="context">The input context</param>
        private void InteractPerformed(InputAction.CallbackContext context)
        {
            OnInteract?.Invoke(context);
        }

        /// <summary>
        /// Event handler for block input.
        /// </summary>
        /// <param name="context">the context from the input</param>
        private void BlockPerformed(InputAction.CallbackContext context)
        {
            OnBlockPerformed?.Invoke(context);
        }

        /// <summary>
        /// Event handler for block input.
        /// </summary>
        /// <param name="context">the context from the input</param>
        private void BlockCancelled(InputAction.CallbackContext context)
        {
            OnBlockCancelled?.Invoke(context);
        }

        /// <summary>
        /// Unpauses the player controller so the player can control the character.
        /// </summary>
        public void Resume()
        {
            enabled = true;
            GetComponent<CharacterController>().enabled = enabled;
        }

        /// <summary>
        /// Pause the player controller so the player cannot move the character.
        /// </summary>
        public void Pause()
        {
            enabled = false;
            GetComponent<CharacterController>().enabled = false;

            // Event clearing code referenced from - https://stackoverflow.com/questions/153573/how-can-i-clear-event-subscriptions-in-c
            OnInteract = null;
        }

        public void ShowHUD(string prompt)
        {
            _hud.ShowPrompt(prompt);
        }

        public void HideHUD()
        {
            _hud.HidePrompt();
        }
    }
}