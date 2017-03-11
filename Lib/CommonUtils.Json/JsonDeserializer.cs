using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace CommonUtils.Json
{
    public class JsonDeserializer
    {
        public static T DeserializeJson<T>(string JsonString)
        {
            T message = default(T);
            JObject jsonResult = (JObject)JsonConvert.DeserializeObject(JsonString);
            message = jsonResult.ToObject<T>();
            return message;
        }

        public static string JsonIndent(string json)
        {
            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter())
            using (var jsonReader = new JsonTextReader(stringReader))
            using (var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented })
            {
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }
    }
}
