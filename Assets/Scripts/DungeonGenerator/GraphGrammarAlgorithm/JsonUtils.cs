using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.DungeonGenerator
{
    public static class JsonUtils
    {
        public static T ConvertToEnum<T>(JToken jToken)
        {
            try
            {
                var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(jToken.ToString());
               return (T)Enum.Parse(typeof(T), json["type"], true);
            }
            catch
            {
                return default;
            }
        }

        public static void ForEachIn(JToken jsonField, Action<JToken> action)
        {
            // Newtonsoft.Json code referenced from - https://www.newtonsoft.com/json/help/html/SerializingJSONFragments.htm
            IList<JToken> jsonList = jsonField.Children().ToList();

            foreach (JToken item in jsonList)
            { 
                action(item);
            }
        }

        public static int ToInt(JToken jToken)
        {
            return int.Parse(jToken.ToString());
        }

        public static Dictionary<string, string> FlattenedJsonValues(JToken jParam)
        {
            return jParam.ToObject<Dictionary<string, string>>();
        }
    }
}