using Assets.DungeonMaster;
using NUnit.Framework;

public class GreaterThanConditionTest
{
    GreaterThanCondition condition;
    
    [SetUp]
    public void SetUp()
    {
        condition = new GreaterThanCondition(4);
    }

    [Test]
    public void ShouldReturnTrueIfConditionIsMet()
    {
        Assert.True(condition.IsMet(30));
    }

    [Test]
    public void ShouldReturnFalseIfConditionIsNotMet()
    {
        Assert.False(condition.IsMet(1));
    }
}
