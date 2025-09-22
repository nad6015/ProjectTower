using Assets.DungeonGenerator.Components;
using Assets.DungeonGenerator;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Assets.Utilities.GameObjectUtilities;
using UnityEngine.TestTools;
using System.Collections;
using Assets.DungeonMaster;
using Newtonsoft.Json.Linq;

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
        var param = DungeonMasterDeserializationUtil.BuildDungeonParameters(JObject.Parse(FindComponentByTag<ParameterSupport>("TestSupport").ParamFile.text));
        parameters = new(null, null, null, param);
        Range<int> enemiesPerRoom = parameters.Parameter<Range<int>>(DungeonParameter.EnemiesPerRoom);
        Range<Vector3> roomSize = parameters.Parameter<Range<Vector3>>(DungeonParameter.RoomSize);

        Assert.That(6 == parameters.Count);

        Assert.That(enemiesPerRoom.min == 1);
        Assert.That(enemiesPerRoom.max == 2);

        Assert.That(roomSize.min == new Vector3(15, 0, 10));
        Assert.That(roomSize.max == new Vector3(20, 0, 15));
    }
}