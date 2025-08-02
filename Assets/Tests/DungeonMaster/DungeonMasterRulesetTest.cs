using Assets.DungeonGenerator;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonMasterRulesetTest
{
    DungeonMasterRuleset ruleset;

    [SetUp]
    public void SetUp()
    {
        ruleset = new("TestRuleset");
    }

    [Test]
    public void ShouldLoopThroughRuleset()
    {
        int count = 0;
        ruleset.ForEach(r => count++);
        Assert.That(count == ruleset.Count);
    }

    [Test]
    public void ShouldHaveRulesAfterParsingJson()
    {
        DungeonMasterRule rule = null;
        ruleset.ForEach(r =>
        {
            if (rule == null)
            {
                rule = r;
                return;
            }
        });

        Assert.That(ruleset.Count == 3);
        Assert.NotNull(rule);
        Assert.That(rule.Id == "RoomCount_Based_On_Prev_Dungeon");
        Assert.That(rule.ParamName == "RoomCount");
        Assert.That(rule.ConditionsMet(3));
        Assert.NotNull(rule.RuleValue());

        RuleValue ruleValue = rule.RuleValue();

        Debug.Log(ruleValue.Type);

        Assert.That(ruleValue.Type == RuleValue.ValueType.RANGE);
        Debug.Log(ruleValue.GetRange());

        var range = ruleValue.GetRange();
        Assert.That(range.Item1 == 10);
        Assert.That(range.Item2 == 10);
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
