using Assets.GameManager;
using UnityEngine;
using UnityEngine.UIElements;

public class RetryScreen : MonoBehaviour
{
    private VisualElement _root;
    private SceneTransitionManager _gameManager;

    void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _gameManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneTransitionManager>();
    }

    private void OnEnable()
    {
        _root.Q<Button>("MainMenu").clicked += GoToStartScreen;
        _root.Q<Button>("TryAgainNoTutorial").clicked += GoToTowerB1;
        _root.Q<Button>("MainMenu").clicked += GoToTutorial;
    }

    private void GoToTutorial()
    {
        _gameManager.SceneTransition(GameScene.NewGame);
    }

    private void GoToTowerB1()
    {
        _gameManager.SceneTransition(GameScene.NextScene);
    }

    private void GoToStartScreen()
    {
        _gameManager.SceneTransition(GameScene.Start);
    }
}
