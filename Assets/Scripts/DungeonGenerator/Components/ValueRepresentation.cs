using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DungeonGenerator.Components
{
    public class ValueRepresentation
    {
        public ValueType Type { get; }
        private object _value;

        private const string valueParam = "value";
        private const string minParam = "min";
        private const string maxParam = "max";

        public ValueRepresentation(ValueType type, Dictionary<string, string> values)
        {
            Type = type;
            switch (type)
            {
                case ValueType.Range:
                {
                    _value = new Range<int>(ToInt(values[minParam]), ToInt(values[maxParam]));
                    break;
                }
                case ValueType.String:
                {
                    _value = values[valueParam];
                    break;
                }
                case ValueType.Number:
                {
                    _value = ToInt(values[valueParam]);
                    break;
                }
                case ValueType.Vector:
                {
                    _value = new Vector3(ToInt(values["x"]), 0, ToInt(values["z"]));
                    break;
                }
                case ValueType.VectorRange:
                {
                    _value = new Range<Vector3>(
                        new(ToInt(values["minX"]), 0, ToInt(values["minZ"])),
                        new(ToInt(values["maxX"]), 0, ToInt(values["maxZ"]))
                        );
                    break;
                }
            }
        }

        /// <summary>
        /// Gets the value as T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Value<T>()
        {
            try
            {
                return (T)_value;

            }
            catch (InvalidCastException)
            {
                return default;
            }
        }

        private int ToInt(string value)
        {
            return int.Parse(value);
        }

        public void Modify(ValueRepresentation value)
        {
            if (Type != value.Type)
            {
                return;
            }
            switch (value.Type)
            {
                case ValueType.String:
                {
                    _value = value.Value<string>();
                    break;
                }
                case ValueType.Number:
                {
                    _value = (int)_value + value.Value<int>();
                    break;
                }
                case ValueType.Range:
                {
                    Range<int> range = (Range<int>)_value;
                    Range<int> newRange = value.Value<Range<int>>();
                    range.min += newRange.min;
                    range.max += newRange.max;
                    _value = range;
                    break;
                }
            }
        }

        public override string ToString()
        {
            switch (Type)
            {

                case ValueType.String:
                case ValueType.Number:
                return _value.ToString();

                case ValueType.Range:
                Range<int> range = Value<Range<int>>();
                return "Range{max: " + range.max + ", min: " + range.min + "}";
                default:
                return "Unknown";
            }
        }
    }
}