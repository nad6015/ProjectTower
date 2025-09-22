using System.Collections;
using Assets.PlayerCharacter;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerControllerTests
{
    private PlayerCamera _camera;

    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("Scenes/Tests/PlayerMechanics");
    }

    [UnityTest]
    public IEnumerator ShouldSpawnCamera()
    {
        yield return TestSetup();

        Assert.NotNull(_camera);
        Assert.That(Camera.allCamerasCount == 1);
    }

    private IEnumerator TestSetup()
    {
        _camera = Camera.main.GetComponent<PlayerCamera>();
        yield return new WaitForSeconds(1);
    }
}
