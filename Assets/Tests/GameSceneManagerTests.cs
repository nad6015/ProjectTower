using System.Collections;
using static Assets.Utilities.GameObjectUtilities;
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
        gameManager = FindComponentByTag<SceneTransitionManager>("SceneManager");
        gameManager.StartNewGame();
        yield return new WaitForSeconds(1f);

        Assert.That(SceneManager.GetActiveScene().name == "Tutorial");
    }
}
