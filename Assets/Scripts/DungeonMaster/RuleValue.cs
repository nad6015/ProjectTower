using System;
using System.Collections.Generic;

namespace Assets.DungeonGenerator
{
    /// <summary>
    /// Represents the value of rule.
    /// </summary>
    public class RuleValue
    {
        public ValueType Type { get; private set; }

        // Commonly used value names
        public const string min = "min";
        public const string max = "max";

        private readonly Dictionary<string, object> _values;

        public RuleValue(Dictionary<string, object> values)
        {
            string type = values["type"].ToString();
            switch (type.ToLower())
            {
                case "range":
                {
                    Type = ValueType.RANGE;
                    break;
                }
            }

            values.Remove("type"); // Needs to be removed as it should not be accessed via the Dictionary outside the constructor
            _values = values;
        }

        /// <summary>
        /// Get the actual value of a rule as T. Throws a runtime exception if the types don't match.
        /// </summary>
        /// <typeparam name="T">the type of the value</typeparam>
        /// <param name="valueName">the name of the value to get</param>
        /// <returns>the value as T</returns>
        public T GetValue<T>(string valueName)
        {
            try
            {
                return (T)_values[valueName];
            }
            catch (InvalidCastException)
            {
                return default;
            }
        }

        public enum ValueType
        {
            NUMBER = 0,
            RANGE = 1,
            STRING = 2
        }
    }
}