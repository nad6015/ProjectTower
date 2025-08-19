using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.GameManager
{
    public class SceneTransitionManager : MonoBehaviour
    {
        [SerializeField]
        private string _nextScene = "GameWon";

        [field: SerializeField]
        public TransitionAnimator TransitionAnimator { get; private set; }

        private GameScene _sceneToTransitionTo;

        private const string _newGame = "Tutorial";
        private const string _startScreen = "StartScreen";

        private void Awake()
        {
            TransitionAnimator.TransitionComplete += OnTransitionComplete;
        }

        public void StartNewGame()
        {
            SceneManager.LoadScene(_newGame);
        }

        /// <summary>
        /// Loads a new scene with a transition.
        /// </summary>
        /// <param name="scene">the scene to load.</param>
        public void SceneTransition(GameScene scene)
        {
            // TODO: Probably want scene transition options like fade to black
            TransitionAnimator.Play();
            _sceneToTransitionTo = scene;
        }

        private void OnTransitionComplete()
        {
            switch (_sceneToTransitionTo)
            {
                case GameScene.Start:
                {
                    SceneManager.LoadScene(_startScreen, LoadSceneMode.Single);
                    break;
                }
                case GameScene.NewGame:
                {
                    SceneManager.LoadScene(_newGame, LoadSceneMode.Single);
                    break;
                }
                case GameScene.NextScene:
                {
                    SceneManager.LoadScene(_nextScene, LoadSceneMode.Single);
                    break;
                }
                case GameScene.None:
                {
                    break;
                }
            }
        }
    }
}