using Assets.GameManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class NewGame : MonoBehaviour
{
    [SerializeField]
    private DungeonExit _exit;

    void Awake()
    {
        _exit.DungeonCleared += OnDungeonCleared;
    }

    private void OnEnable()
    {
        //_enterDungeon = _root.Q<Button>("EnterDungeon");
        //_enterDungeon.clicked += EnterDungeonClicked;
    }

    private void OnDungeonCleared()
    {
        SceneManager.LoadScene("MainGame");
    }
}
