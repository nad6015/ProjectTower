using Assets.DungeonGenerator.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    [SerializeField]
    private DungeonExit _exit;

    void Start()
    {
        GameObject.FindGameObjectWithTag("DungeonExit").GetComponent<DungeonExit>().DungeonCleared += OnDungeonCleared;
    }

    private void OnDungeonCleared()
    {
        SceneManager.LoadScene("MainGame");
    }
}
