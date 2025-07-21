using Assets.GameManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class NewGame : MonoBehaviour
{
    private VisualElement _root;
    private Button _enterDungeon;

    void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        _enterDungeon = _root.Q<Button>("EnterDungeon");
        _enterDungeon.clicked += EnterDungeonClicked;
    }

    private void EnterDungeonClicked()
    {
        SceneManager.LoadScene("MainGame");
    }
}
