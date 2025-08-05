using Assets.DungeonGenerator.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.DungeonGenerator.DataStructures
{
    public class DungeonParameter
    {
        public string Id { get; }
        public ValueType Type { get; private set; }

        private const string valueParam = "value";
        private const string minParam = "min";
        private const string maxParam = "max";

        private readonly Dictionary<string, object> _values;

        public DungeonParameter(Dictionary<string, object> values)
        {
            Id = values["id"].ToString();
            _values = values;

            switch (values["type"].ToString())
            {
                case "range":
                {
                    Type = ValueType.RANGE;
                    JObject range = JObject.Parse(_values[valueParam].ToString());

                    if (_values[valueParam].ToString().Contains("z")) // If the range value is a vector
                    {
                        _values[minParam] = range[minParam].ToObject<Vector3>();
                        _values[maxParam] = range[maxParam].ToObject<Vector3>();
                    }
                    else
                    {
                        _values[minParam] = range[minParam].ToObject<float>();
                        _values[maxParam] = range[maxParam].ToObject<float>();
                    }
                    break;
                }
                case "number":
                {
                    Type = ValueType.NUMBER;
                    //_values["value"] = values["value"].ToObject<float>();
                    break;
                }
                case "vector":
                {
                    Type = ValueType.VECTOR3;
                    _values[valueParam] = JsonConvert.DeserializeObject<Vector3>(_values[valueParam].ToString());
                    break;
                }
            }
        }

        public Range<Vector3> VectorRange()
        {
            if (Type == ValueType.RANGE)
            {
                return new((Vector3)_values[minParam], (Vector3)_values[maxParam]);
            }
            return new();
        }

        public float Value()
        {
            if (Type == ValueType.NUMBER)
            {
                return float.Parse(_values[valueParam].ToString());
            }
            return -1;
        }


        public Range<float> Range()
        {
            if (Type == ValueType.RANGE)
            {
                return new(float.Parse(_values[minParam].ToString()), float.Parse(_values[maxParam].ToString()));
            }
            return new();
        }

        internal void Modify(int value)
        {
            _values[valueParam] = value;
        }

        internal Vector3 Vector()
        {
            if(Type == ValueType.VECTOR3)
            {
                return (Vector3)_values[valueParam];
            }
            return Vector3.zero;
        }
    }
}
