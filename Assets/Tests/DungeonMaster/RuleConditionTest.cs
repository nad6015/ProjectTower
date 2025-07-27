using Assets.DungeonGenerator;
using NUnit.Framework;
using System.Collections.Generic;

public class RuleConditionTest
{
    RuleCondition ruleCondition;
    
    [SetUp]
    public void SetUp()
    {
        ruleCondition = new RuleCondition(">", "4");
    }

    [Test]
    public void ShouldReturnTrueIfConditionIsMet()
    {
        Assert.True(ruleCondition.IsMet(30f));
    }

    [Test]
    public void ShouldReturnFalseIfConditionIsNotMet()
    {
        Assert.False(ruleCondition.IsMet(1f));
    }

    [Test]
    public void ShouldHandleGreaterThanOperator()
    {
        Assert.True(ruleCondition.IsMet(5f));
        Assert.False(ruleCondition.IsMet(1f));
    }

    [Test]
    public void ShouldHandleLessThanOperator()
    {
        ruleCondition = new RuleCondition("<", "10");
        Assert.True(ruleCondition.IsMet(5f));
        Assert.False(ruleCondition.IsMet(10f));
    }
}
