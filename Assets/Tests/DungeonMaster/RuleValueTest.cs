using Assets.DungeonGenerator;
using NUnit.Framework;
using System.Collections.Generic;

public class RuleValueTest
{
    RuleValue ruleValue;
    
    [SetUp]
    public void SetUp() 
    {
        Dictionary<string, object> data = new()
        {
            { "value", 30f },
            { "type", "number" }
        };
        
        ruleValue = new RuleValue(data);
    }

    [Test]
    public void ShouldReturnFloatValue()
    {
        Assert.That(ruleValue.Type == RuleValue.ValueType.NUMBER);
        Assert.That(ruleValue.GetNumber() == 30);
    }

    [Test]
    public void ShouldReturnRangeValue()
    {
        Assert.That(ruleValue.Type == RuleValue.ValueType.NUMBER);
        Assert.That(ruleValue.GetNumber() == 30);
    }

    [Test]
    public void ShouldReturnNullIfValueIsNotOfTypeT()
    {
        Dictionary<string, object> data = new()
        {
            { "value", "30f" },
            { "type", "number" }
        };

        ruleValue = new RuleValue(data);
        Assert.That(ruleValue.Type != RuleValue.ValueType.STRING);
        Assert.That(ruleValue.GetNumber() == -1f);
    }
}
