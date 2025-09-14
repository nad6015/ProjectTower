using Assets.DungeonGenerator;
using Assets.DungeonGenerator.Components;
using Assets.DungeonMaster;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using static Assets.Utilities.GameObjectUtilities;

public class DungeonRulesetTest
{
    Dictionary<DungeonParameter, DungeonRule> ruleset;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        SceneManager.LoadScene("Scenes/Tests/DungeonParameters");
        yield return null;
    }


    [UnityTest]
    public IEnumerator ShouldHaveRulesAfterParsingJson()
    {
        yield return new WaitForSeconds(1);
        JObject json = JObject.Parse(FindComponentByTag<ParameterSupport>("TestSupport").RulesetFile.text);
        ruleset = DungeonMasterDeserializationUtil.BuildDungeonRuleset(json);



        Assert.That(ruleset.Count == 3);
        Assert.That(ruleset.ContainsKey(DungeonParameter.RoomCount), Is.True);
        Assert.That(ruleset.ContainsKey(DungeonParameter.EnemiesPerRoom), Is.True);
        Assert.That(ruleset.ContainsKey(DungeonParameter.ItemsPerRoom), Is.True);

        Assert.That(ruleset[DungeonParameter.RoomCount].GameParameter == GameParameter.ClearTime);
        Assert.That(ruleset[DungeonParameter.CorridorSize].GameParameter == GameParameter.EnemiesDefeated);
        Assert.That(ruleset[DungeonParameter.ItemsPerRoom].GameParameter == GameParameter.TotalHealthLost);

        DungeonRule rule = ruleset[DungeonParameter.RoomCount];
        var gameData = new Dictionary<GameParameter, int>()
        {
            {GameParameter.ClearTime, 3 }
        };

        Assert.True(rule.ConditionsMet(gameData));
        Assert.NotNull(rule);
        ValueRepresentation value = rule.Value();

        Assert.That(value.Type == ValueType.Number);
        Assert.That(value.Value<int>() == 3);
    }

    [UnityTest]
    public IEnumerator ShouldIgnoreDuplicateRules()
    {
        yield return new WaitForSeconds(1);
        TextAsset json = FindComponentByTag<ParameterSupport>("TestSupport").RulesetFile;

        int count = 0;

        JObject rulesJson = JObject.Parse(json.text);
        IList<JToken> jRules = rulesJson["dungeonRules"].Children().ToList();

        foreach (JToken jRule in jRules)
        {
            // If this rule is found, increase the count by one.
            count += jRule["parameter"].ToString() == "ItemsPerRoom" ? 1 : 0;
        }

        Assert.That(ruleset.Count < jRules.Count);
        Assert.That(count == 2);
    }
}
