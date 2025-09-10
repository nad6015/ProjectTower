using UnityEngine;
using UnityEngine.UIElements;

public class CloseModal : MonoBehaviour
{
    [SerializeField]
    private UIDocument _prevMenu;

    private VisualElement _root;

    void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _root.visible = false;
    }

    private void OnEnable()
    {
        _root.Q<Button>("close").clicked += ShowPreviousMenu;
    }

    private void OnDisable()
    {
        _root.Q<Button>("close").clicked -= ShowPreviousMenu;
    }

    private void ShowPreviousMenu()
    {
        _root.visible = false;
        if (_prevMenu != null)
        {
            _prevMenu.rootVisualElement.visible = true;
        }
    }
}
