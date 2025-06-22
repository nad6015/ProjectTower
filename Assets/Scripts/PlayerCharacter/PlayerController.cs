using UnityEngine;

namespace Assets.PlayerCharacter
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerAction), typeof(PlayerMovement))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float speed = 5;

        private InputSystemActions actions;
        private PlayerMovement playerMovement;
        private PlayerAction playerAction;

        void Awake()
        {
            playerMovement = GetComponent<PlayerMovement>();
            playerMovement.speed = speed;

            playerAction = GetComponent<PlayerAction>();
            actions = new InputSystemActions();
        }

        private void OnEnable()
        {
            actions.Player.Move.Enable();
            actions.Player.Move.performed += playerMovement.OnMovePerformed;
            actions.Player.Move.canceled += playerMovement.OnMoveCancelled;

            actions.Player.Attack.Enable();
            actions.Player.Attack.performed += playerAction.OnAttackPerformed;
            actions.Player.Attack.canceled += playerAction.OnAttackCancelled;
        }

        private void OnDisable()
        {
            actions.Player.Move.Disable();
            actions.Player.Move.performed -= playerMovement.OnMovePerformed;
            actions.Player.Move.canceled -= playerMovement.OnMoveCancelled;
            
            actions.Player.Attack.Disable();
            actions.Player.Attack.performed -= playerAction.OnAttackPerformed;
            actions.Player.Attack.canceled -= playerAction.OnAttackCancelled;
        }
    }
}