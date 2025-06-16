using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 3;
    [SerializeField]
    private BoxCollider attackHitbox;

    private CharacterController characterController;
    private InputSystemActions actions;

    private Vector3 newPos;
    private Quaternion currentRotation;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        characterController = GetComponent<CharacterController>();
        actions = new InputSystemActions();
        currentRotation = transform.rotation;
        
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
        var value = context.ReadValue<Vector2>();
        newPos.x = value.x;
        newPos.z = value.y;
    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        newPos = Vector3.zero;
    }

    private void OnAttackCancelled(InputAction.CallbackContext context)
    {
        attackHitbox.enabled = false;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        attackHitbox.enabled = true;
    }

    void Update()
    {
        // Code referenced from - https://discussions.unity.com/t/make-a-player-model-rotate-towards-mouse-location/125354/3

        Vector2 transformOnScreen = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());

        float x = Unity.Mathematics.math.remap(0, 1, -1, 1, mouseOnScreen.x);
        float y = Unity.Mathematics.math.remap(0, 1, -1, 1, mouseOnScreen.y);
        currentRotation = Quaternion.LookRotation(new Vector3(x, 0, y), Vector3.up);


        float angle = AngleBetweenTwoPoints(transformOnScreen, mouseOnScreen);

        transform.rotation = currentRotation;

        characterController.SimpleMove(speed * newPos);
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
