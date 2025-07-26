using System.Collections;
using Assets.PlayerCharacter;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerControllerTests
{
    private readonly GameObject _playerObj = Resources.Load<GameObject>("Player");
    private GameObject _player;
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
        _player = GameObject.Instantiate(_playerObj, new Vector3(0, 1), Quaternion.identity);
        _camera = Camera.main.GetComponent<PlayerCamera>();
        yield return new WaitForSeconds(1);
    }
}
