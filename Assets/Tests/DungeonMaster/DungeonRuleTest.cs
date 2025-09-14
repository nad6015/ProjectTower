using Assets.DungeonGenerator;
using Assets.DungeonMaster;
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

        rule = new("id",DungeonParameter.RoomCount,
            GameParameter.EnemiesDefeated, conditons,
            new(ValueType.Range, value));
    }

    [Test]
    public void ShouldReturnTrueIfAllConditionsAreMet()
    {
        gameStatistics.Add(GameParameter.EnemiesDefeated, 6);
        Assert.True(rule.ConditionsMet(gameStatistics));
    }

    [Test]
    public void ShouldReturnFalseIfOneConditionIsNotMet()
    {
        gameStatistics.Add(GameParameter.EnemiesDefeated, 30);
        Assert.False(rule.ConditionsMet(gameStatistics));
    }

    [Test]
    public void ShouldReturnNullRuleValueIfConditionsNotMet()
    {
        gameStatistics.Add(GameParameter.EnemiesDefeated, 30);
        Assert.Null(rule.Value());
        
        rule.ConditionsMet(gameStatistics);
        Assert.Null(rule.Value());
    }

    [Test]
    public void ShouldReturnRuleValueIfConditionsMet()
    {
        gameStatistics.Add(GameParameter.EnemiesDefeated, 6);
        
        rule.ConditionsMet(gameStatistics);
        Assert.NotNull(rule.Value());
    }
}