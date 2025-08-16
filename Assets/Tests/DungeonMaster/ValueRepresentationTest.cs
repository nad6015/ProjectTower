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

        value = new ValueRepresentation(ValueType.Number, data);
    }

    [Test]
    public void ShouldReturnFloatValue()
    {
        Assert.That(value.Type == ValueType.Number);
        Assert.That(value.Value<int>() == 30);
    }

    [Test]
    public void ShouldReturnRangeValue()
    {
        value = new ValueRepresentation(ValueType.Range, new() { { "min", "10" }, { "max", "10" } });
        Range<int> range = new(10, 10);

        Assert.That(value.Type == ValueType.Range);
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

        value = new ValueRepresentation(ValueType.Number, data);
        Assert.That(value.Type != ValueType.String);
        Assert.That(value.Value<int>() == 30);
        Assert.That(value.Value<string>() == null);
    }
}
