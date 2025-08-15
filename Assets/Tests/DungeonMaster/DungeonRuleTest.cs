using Assets.DungeonGenerator;
using NUnit.Framework;
using System.Collections.Generic;

public class DungeonRuleTest
{
    DungeonRule rule;
    Dictionary<GameParameter, int> gameStatistics;

    [SetUp]
    public void SetUp()
    {
        gameStatistics = new Dictionary<GameParameter, int>();
        Dictionary<string, string> value = new()
        {
            {"min", "1" },
            {"max", "5" }
        };
        List<ICondition> conditons = new()
        {
            new GreaterThanCondition(4),
            new LessThanCondition(10)
        };

        rule = new(DungeonParameter.ROOM_COUNT,
            GameParameter.ENEMIES_DEFEATED, conditons,
            new(ValueType.RANGE, value));
    }

    [Test]
    public void ShouldReturnTrueIfAllConditionsAreMet()
    {
        gameStatistics.Add(GameParameter.ENEMIES_DEFEATED, 6);
        Assert.True(rule.ConditionsMet(gameStatistics));
    }

    [Test]
    public void ShouldReturnFalseIfOneConditionIsNotMet()
    {
        gameStatistics.Add(GameParameter.ENEMIES_DEFEATED, 30);
        Assert.False(rule.ConditionsMet(gameStatistics));
    }

    [Test]
    public void ShouldReturnNullRuleValueIfConditionsNotMet()
    {
        gameStatistics.Add(GameParameter.ENEMIES_DEFEATED, 30);
        Assert.Null(rule.Value());
        
        rule.ConditionsMet(gameStatistics);
        Assert.Null(rule.Value());
    }

    [Test]
    public void ShouldReturnRuleValueIfConditionsMet()
    {
        gameStatistics.Add(GameParameter.ENEMIES_DEFEATED, 6);
        
        rule.ConditionsMet(gameStatistics);
        Assert.NotNull(rule.Value());
    }
}