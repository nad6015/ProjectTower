using Assets.DungeonGenerator;
using Assets.DungeonGenerator.Components;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonRulesetTest
{
    Dictionary<DungeonParameter, DungeonRule> ruleset;

    [SetUp]
    public void SetUp()
    {
        JObject json = JObject.Parse(Resources.Load<TextAsset>("TestRuleset").text);
        ruleset = RulesetBuilder.BuildDungeonRuleset(json);
    }

    [Test]
    public void ShouldHaveRulesAfterParsingJson()
    {
        DungeonRule rule = null;
        foreach (var r in ruleset.Values)
        {
            if (rule == null)
            {
                rule = r;
                return;
            }
        }

        Assert.That(ruleset.Count == 3);
        Assert.NotNull(rule);
        Assert.That(rule.Parameter == DungeonParameter.ROOM_SIZE);
        Assert.That(rule.GameParameter == GameParameter.ENEMIES_DEFEATED);
        Assert.That(rule.ConditionsMet(new()));
        Assert.NotNull(rule.Value());

        ValueRepresentation value = rule.Value();

        Assert.That(value.Type == ValueType.RANGE);

        var range = value.Value<Range<int>>();
        Assert.That(range.min == 10);
        Assert.That(range.max == 10);
    }

    [Test]
    public void ShouldIgnoreDuplicateRules()
    {
        TextAsset json = Resources.Load<TextAsset>("TestRuleset");
        int count = 0;

        JObject rulesJson = JObject.Parse(json.text);
        IList<JToken> jRules = rulesJson["rules"].Children().ToList();

        foreach (JToken jRule in jRules)
        {
            // If this rule is found, increase the count by one.
            count += jRule["id"].ToString() == "RoomCount_Based_On_Prev_Dungeon" ? 1 : 0;
        }

        Assert.That(ruleset.Count < jRules.Count);
        Assert.That(count == 2);
    }
}
