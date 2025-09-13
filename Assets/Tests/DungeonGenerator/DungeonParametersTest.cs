using Assets.DungeonGenerator.Components;
using Assets.DungeonGenerator;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Assets.Utilities.GameObjectUtilities;
using UnityEngine.TestTools;
using System.Collections;

public class DungeonParametersTest
{
    DungeonRepresentation parameters;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("Scenes/Tests/DungeonParameters");
        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldGetAnyParameter()
    {
        yield return new WaitForSeconds(1);
        parameters = new(FindComponentByTag<ParameterSupport>("TestSupport").ParamFile, null);
        Range<int> enemiesPerRoom = parameters.Parameter<Range<int>>(DungeonParameter.EnemiesPerRoom);
        Range<Vector3> roomSize = parameters.Parameter<Range<Vector3>>(DungeonParameter.RoomSize);

        Assert.That(6 == parameters.Count);

        Assert.That(enemiesPerRoom.min == 0);
        Assert.That(enemiesPerRoom.max == 3);

        Debug.Log(roomSize.min);
        Debug.Log(roomSize.max);

        Assert.That(roomSize.min == new Vector3(15, 0, 15));
        Assert.That(roomSize.max == new Vector3(35, 0, 35));
    }
}