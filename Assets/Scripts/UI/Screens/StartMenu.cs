using UnityEngine;
using UnityEngine.UIElements;
using Assets.GameManager;

namespace Assets.UI
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField]
        private UIDocument _credits;

        private VisualElement _root;
        private SceneTransitionManager _gameManager;

        void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _gameManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneTransitionManager>();
        }

        private void OnEnable()
        {
            _root.Q<Button>("NewGame").clicked += NewGameClicked;
            _root.Q<Button>("Credits").clicked += ShowCredits;
        }
        private void OnDisable()
        {
            _root.Q<Button>("NewGame").clicked -= NewGameClicked;
            _root.Q<Button>("Credits").clicked -= ShowCredits;
        }

        private void ShowCredits()
        {
            _root.visible = false;
            _credits.rootVisualElement.visible = true;
        }

        private void NewGameClicked()
        {
            _gameManager.StartNewGame();
        }
    }
}