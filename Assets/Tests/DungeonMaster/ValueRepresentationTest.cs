using Assets.DungeonGenerator;
using Assets.DungeonGenerator.Components;
using NUnit.Framework;
using System.Collections.Generic;

public class ValueRepresentationTest
{
    ValueRepresentation value;

    [SetUp]
    public void SetUp()
    {
        Dictionary<string, string> data = new()
        {
            { "value", "30" }
        };

        value = new ValueRepresentation(ValueType.NUMBER, data);
    }

    [Test]
    public void ShouldReturnFloatValue()
    {
        Assert.That(value.Type == ValueType.NUMBER);
        Assert.That(value.Value<int>() == 30);
    }

    [Test]
    public void ShouldReturnRangeValue()
    {
        var range = new Range<int>(10, 10);
        Assert.That(value.Type == ValueType.RANGE);
        Assert.That(value.Value<Range<int>>().min == range.min);
        Assert.That(value.Value<Range<int>>().max == range.max);
    }

    [Test]
    public void ShouldReturnNullIfValueIsNotOfTypeT()
    {
        Dictionary<string, string> data = new()
        {
            { "value", "30" }
        };

        value = new ValueRepresentation(ValueType.NUMBER, data);
        Assert.That(value.Type != ValueType.STRING);
        Assert.That(value.Value<int>() == 30);
        Assert.That(value.Value<string>() == null);
    }
}
