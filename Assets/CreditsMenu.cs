using Assets.GameManager;
using UnityEngine;
using UnityEngine.UIElements;

public class CreditsMenu : MonoBehaviour
{
    [SerializeField]
    private UIDocument _prevMenu;

    private VisualElement _root;
    private SceneTransitionManager _gameManager;

    void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _gameManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneTransitionManager>();
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
        _prevMenu.rootVisualElement.visible = true;
    }
}
