using UnityEngine;
using UnityEngine.UIElements;
using Assets.GameManager;

namespace Assets.UI
{
    public class StartMenu : MonoBehaviour
    {
        private VisualElement _root;
        private GameSceneManager _gameManager;

        private Button _newGame;
        void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameSceneManager>();
        }

        private void OnEnable()
        {
            _newGame = _root.Q<Button>("NewGame");
            _newGame.clicked += NewGameClicked;
        }

        private void NewGameClicked()
        {
            _gameManager.StartNewGame();
        }
    }
}