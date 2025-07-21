using Assets.DungeonGenerator;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.GameManager
{
    public class GameStateManager : MonoBehaviour
    {
        readonly private string _sceneName = "NewGame";
        readonly private string _mainGame = "MainGame";

        private void Awake()
        {
            SceneManager.activeSceneChanged += onSceneChange;
            DontDestroyOnLoad(this); // Game Manager should persist between scenes
        }

        public void StartNewGame()
        {
            SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
        }

        private void onSceneChange(Scene current, Scene next)
        {
            if (next.name == _mainGame) 
            {
                GameObject.FindGameObjectWithTag("DungeonMaster").GetComponent<DungeonMaster>().StartDungeonMaster();
            }
        }
    }
}