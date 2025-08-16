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
                    _value = new Range<int>(int.Parse(values[minParam]), int.Parse(values[maxParam]));
                    break;
                }
                case ValueType.String:
                {
                    _value = values[valueParam];
                    break;
                }
                case ValueType.Number:
                {
                    _value = int.Parse(values[valueParam]);
                    break;
                }
                case ValueType.Vector:
                {
                    _value = new Vector3(ToInt(values["x"]), ToInt(values["y"]), ToInt(values["z"]));
                    break;
                }
                case ValueType.VectorRange:
                {
                    _value = new Range<Vector3>(
                        new(ToInt(values["minX"]), ToInt(values["minY"]), ToInt(values["minZ"])),
                        new(ToInt(values["maxX"]), ToInt(values["maxY"]), ToInt(values["maxZ"]))
                        );
                    break;
                }
            }
        }

        /// <summary>
        /// TODO
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
                    _value = value.Value<int>();
                    break;
                }
            }
        }
    }
}