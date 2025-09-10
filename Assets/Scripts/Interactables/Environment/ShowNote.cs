using Assets.Interactables;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ShowNote : Interactable
{
    private VisualElement _root;
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _root.visible = false;
        prompt = "Read";
    }

    protected override void HandleInteract(InputAction.CallbackContext context)
    {
        _root.visible = true;
    }
}
