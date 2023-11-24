using Newtonsoft.Json.Linq;

namespace ConvertJsonToWord.Utils
{
    public class Transform
    {
        // 處理response body要呈現的json格式
        public string TransformJson(JObject data, string result = "", int indentLevel = 0)
        {
            foreach (var property in data.Properties())
            {
                result += $"{new string('\t', indentLevel)}  \"{property.Name}\": <{property.Value["description"]}>";

                if (property.Value["properties"].HasValues)
                {
                    var newIndentLevel = indentLevel + 1;
                    result += "\n" + new string('\t', newIndentLevel) + "{\n";
                    var properties = property.Value["properties"];

                    // Recursive call with increased indent level
                    result = TransformJson(properties.ToObject<JObject>(), result, newIndentLevel);

                    result += $"{new string('\t', newIndentLevel)}}},\n";
                }
                else
                {
                    result += ",\n";
                }

            }

            return result;
        }

      
    }
}

