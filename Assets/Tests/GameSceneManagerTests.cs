using System.Collections;
using Assets.DungeonGenerator;
using Assets.GameManager;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class GameSceneManagerTests
{
    SceneTransitionManager gameManager;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("Scenes/Tests/GameManagerTestScene");
        yield return null;
    }

    [UnityTest]
    public IEnumerator GameManagerStartsANewGame()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SceneTransitionManager>();
        gameManager.StartNewGame();
        yield return null;

        Assert.That(SceneManager.GetActiveScene().name == "NewGame");
    }

    [UnityTest]
    public IEnumerator GameManagerPersistsBetweenScenes()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SceneTransitionManager>();
        gameManager.StartNewGame();
        yield return null;

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SceneTransitionManager>();

        Assert.That(gameManager != null);
    }
}
