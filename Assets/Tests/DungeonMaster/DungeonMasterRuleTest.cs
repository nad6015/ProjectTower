using Assets.DungeonGenerator;
using NUnit.Framework;
using System.Collections.Generic;

public class DungeonMasterRuleTest
{
    DungeonMasterRule rule;

    [SetUp]
    public void SetUp()
    {
        Dictionary<string, object> ruleValue = new()
        {
            { "type", "range" },
            {"min", "1" },
            {"max", "5" }
        };
        List<RuleCondition> conditons = new()
        {
            new RuleCondition(">", "4"),
            new RuleCondition("<", "10")
        };

        rule = new("test", "dungeonRoom", conditons, new(ruleValue));
    }

    [Test]
    public void ShouldReturnTrueIfAllConditionsAreMet()
    {
        Assert.True(rule.ConditionsMet(6f));
    }

    [Test]
    public void ShouldReturnFalseIfOneConditionIsNotMet()
    {
        Assert.False(rule.ConditionsMet(30f));
    }

    [Test]
    public void ShouldReturnNullRuleValueIfConditionsNotMet()
    {
        Assert.Null(rule.RuleValue());
        rule.ConditionsMet(30f);
        Assert.Null(rule.RuleValue());
    }

    [Test]
    public void ShouldReturnRuleValueIfConditionsMet()
    {
        rule.ConditionsMet(6f);
        Assert.NotNull(rule.RuleValue());
    }
}