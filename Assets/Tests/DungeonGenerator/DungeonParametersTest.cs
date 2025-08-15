using Assets.DungeonGenerator.Components;
using Assets.DungeonGenerator;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonParametersTest
{
    DungeonRepresentation parameters;

    [SetUp]
    public void SetUp()
    {
        parameters = new(Resources.Load<TextAsset>("TestParameters"));
    }

    [Test]
    public void ShouldGetAnyParameter()
    {
        Range<int> enemiesPerRoom = parameters.GetParameter<Range<int>>(DungeonParameter.EnemiesPerRoom);
        Range<Vector3> roomSize = parameters.GetParameter<Range<Vector3>>(DungeonParameter.RoomSize);

        Assert.That(6 == parameters.Count);

        Assert.That(enemiesPerRoom.min == 0);
        Assert.That(enemiesPerRoom.max == 3);

        Debug.Log(roomSize.min);
        Debug.Log(roomSize.max);

        Assert.That(roomSize.min == new Vector3(15, 0, 15));
        Assert.That(roomSize.max == new Vector3(35, 0, 35));
    }
}