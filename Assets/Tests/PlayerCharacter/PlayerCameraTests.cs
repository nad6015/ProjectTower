using System.Collections;
using Assets.PlayerCharacter;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerCameraTests : InputTestFixture
{
    private readonly GameObject _playerObj = Resources.Load<GameObject>("Player");
    private GameObject _player;
    private PlayerCamera _camera;
    private Keyboard _keyboard;
    private Mouse _mouse;

    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/Tests/PlayerMechanics");
        base.Setup();
        _keyboard = InputSystem.AddDevice<Keyboard>();
        _mouse = InputSystem.AddDevice<Mouse>();

        Press(_mouse.rightButton);
        Release(_mouse.rightButton);
    }

    [UnityTest]
    public IEnumerator ShouldLookAtPlayer()
    {
        yield return TestSetup();

        Vector3 camPos = _camera.transform.position;
        Vector3 playerPos = _player.transform.position;

        Vector3 dir = (playerPos - camPos).normalized;
        float distY = playerPos.z - camPos.z;

        Assert.That(camPos.x == 0);
        Assert.That(Mathf.Approximately(distY, _camera.DistanceFromPlayerZ));
        Assert.That(dir.z > 0.3f); // Assert camera is pointing in player's direction

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldFollowPlayer()
    {
        yield return TestSetup();

        Vector3 camPos = _camera.transform.position;
        Vector3 playerPos = _player.transform.position;
        
        Vector3 dir = (playerPos - camPos).normalized;

        float distY = playerPos.z - camPos.z;

        Assert.That(Mathf.Approximately(camPos.x, playerPos.x));
        Assert.That(Mathf.Approximately(distY, _camera.DistanceFromPlayerZ));
        Assert.That(dir.z > 0.3f); // Assert camera is pointing in player's direction

        yield return MovePlayer();


        // Asserts that the player has moved
        
        Assert.That(_player.transform.position != playerPos);
        
        playerPos = _player.transform.position;
        camPos = _camera.transform.position;

        distY = playerPos.z - camPos.z;
        dir = (playerPos - camPos).normalized;

        // Asserts that the distance and direction between the camera and the player hasn't changed
        Assert.That(Mathf.Approximately(playerPos.x, camPos.x));
        Assert.That(Mathf.Approximately(distY, _camera.DistanceFromPlayerZ));
        Assert.That(dir.z > 0.3f); // Assert camera is pointing in player's direction

        yield return null;
    }


    private IEnumerator TestSetup()
    {
        _player = GameObject.Instantiate(_playerObj, new Vector3(0, 1), Quaternion.identity);
        _camera = Camera.main.GetComponent<PlayerCamera>();
        yield return new WaitForSeconds(1);
    }

    private IEnumerator MovePlayer()
    {
        Press(_keyboard.wKey);
        Press(_keyboard.aKey);
        yield return new WaitForSeconds(1f);
        Release(_keyboard.wKey);
        Release(_keyboard.aKey);
        yield return new WaitForSeconds(1f);
    }
}
