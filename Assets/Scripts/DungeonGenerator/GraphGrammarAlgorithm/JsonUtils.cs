using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

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
    }
}