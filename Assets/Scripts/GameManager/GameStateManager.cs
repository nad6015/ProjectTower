using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.GameManager
{
    public class GameStateManager : MonoBehaviour
    {
        readonly private string _sceneName = "NewGame";

        private void Awake()
        {
            DontDestroyOnLoad(this); // Game Manager should persist between scenes
        }

        public void StartNewGame()
        {
            SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
        }
    }
}