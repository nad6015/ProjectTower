using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 3;

    private CharacterController characterController;
    private InputSystemActions actions;
    private Vector3 newPos;
 

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        actions = new InputSystemActions();
    }

    private void OnEnable()
    {
        actions.Player.Move.Enable();
        actions.Player.Move.performed += OnMovePerformed;
        actions.Player.Move.canceled += OnMoveCancelled;

        actions.Player.Attack.Enable();
        actions.Player.Attack.performed += OnAttackPerformed;
        actions.Player.Attack.canceled += OnAttackCancelled;
    }

    private void OnDisable()
    {
        actions.Player.Move.Disable();
        actions.Player.Move.performed -= OnMovePerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        newPos = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        newPos = Vector3.zero;
    }

    private void OnAttackCancelled(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }
    void Update()
    {
        transform.LookAt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        characterController.SimpleMove(speed * (newPos + transform.forward));
        
    }
}
