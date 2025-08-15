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
        Range<int> enemiesPerRoom = parameters.GetParameter<Range<int>>(DungeonParameter.ENEMIES_PER_ROOM);
        Range<Vector3> roomSize = parameters.GetParameter<Range<Vector3>>(DungeonParameter.ROOM_SIZE);

        Assert.That(7 == parameters.Count);

        Assert.That(enemiesPerRoom.min == 0);
        Assert.That(enemiesPerRoom.max == 3);

        Assert.That(roomSize.min == new Vector3(15, 0, 15));
        Assert.That(roomSize.max == new Vector3(35, 0, 35));
    }

    [Test]
    public void ShouldIgnoreDuplicateParameters()
    {
        TextAsset json = Resources.Load<TextAsset>("TestParameters");
        int count = 0;

        JObject rulesJson = JObject.Parse(json.text);
        IList<JToken> jRules = rulesJson["params"].Children().ToList();

        foreach (JToken jRule in jRules)
        {
            // If this rule is found, increase the count by one.
            count += jRule["id"].ToString() == "RoomCount_Based_On_Prev_Dungeon" ? 1 : 0;
        }

        Assert.That(parameters.Count < jRules.Count);
        Assert.That(count == 2);
    }
}