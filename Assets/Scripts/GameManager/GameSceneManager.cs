using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.GameManager
{
    public class GameSceneManager : MonoBehaviour
    {
        private const string _newGame = "NewGame";
        private const string _gameWon = "GameWon";
        private const string _startScreen = "StartScreen";

        private void Awake()
        {
            DontDestroyOnLoad(this); // Game Manager should persist between scenes
        }

        public void StartNewGame()
        {
            SceneManager.LoadScene(_newGame);
        }

        /// <summary>
        /// Loads a new scene with a transition.
        /// </summary>
        /// <param name="scene">the scene to load.</param>
        public static void SceneTransition(GameScene scene)
        {
            // TODO: Probably want scene transition options like fade to black
            switch (scene)
            {
                case GameScene.START:
                {
                    SceneManager.LoadScene(_startScreen, LoadSceneMode.Single);
                    break;
                }
                case GameScene.NEW_GAME:
                {
                    SceneManager.LoadScene(_newGame, LoadSceneMode.Single);
                    break;
                }
                case GameScene.GAME_WON:
                {
                    SceneManager.LoadScene(_gameWon, LoadSceneMode.Single);
                    break;
                }
            }
        }

        public enum GameScene
        {
            START,
            NEW_GAME,
            GAME_WON,
            GAME_LOST
        }
    }
}