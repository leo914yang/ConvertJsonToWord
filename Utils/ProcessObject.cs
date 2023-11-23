using ConvertJsonToWord.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static ConvertJsonToWord.Model.MyOpenApiObject;

namespace ConvertJsonToWord.Utils
{
    public class ProcessObject
    {
        // 指定撈取Items底下的properties
        // 再將資料按照格式轉換成json
        public void ProcessFormat(MyOpenApiObject myOpenApiObject)
        {
            var trans = new Transform();
            foreach (var path in myOpenApiObject.paths)
            {
                foreach (var method in path.Value.methods)
                {
                    
                    foreach (var response in method.Value.responses)
                    {
                        if (response.Value.content != null)
                        {
                            foreach (var con in response.Value.content)
                            {
                                var schema = con.Value.schema;

                                if (schema != null && schema.Items != null && schema.Items.properties != null)
                                {
                                    var properties = schema.Items.properties;

                                    var responseJson = JsonConvert.SerializeObject(properties, Formatting.Indented);
                                    var jsonResponse = JsonConvert.DeserializeObject<JObject>(responseJson);
                                    
                                    string jsonResponseResult = string.Empty;
                                    var finalResponseResult = trans.TransformJson(jsonResponse, jsonResponseResult);
                                    schema.Items.result = "{\n" + finalResponseResult.Substring(0, finalResponseResult.Length - 2) + "\n}";

                                }
                            }
                        }
                        
                    }
                }
            }
        }
        // 處理api的methods，理論上一次只會有一種method
        public void ProcessMethod(MyOpenApiObject myOpenApiObject)
        {
            foreach (var path in myOpenApiObject.paths)
            {
                path.Value.methods = new Dictionary<string, Operations>();
                if (path.Value.get != null)
                {
                    path.Value.methods.Add("get", path.Value.get);
                }
                else if (path.Value.post != null)
                {
                    path.Value.methods.Add("post", path.Value.post);
                }
                else if (path.Value.put != null)
                {
                    path.Value.methods.Add("put", path.Value.put);
                }
                else if (path.Value.delete != null)
                {
                    path.Value.methods.Add("delete", path.Value.delete);
                }
            }
        }
        // 替換原始json格式中的&red flag，轉換成將指定的物件貼到Items底下
        // 以下程式都是用來實現這個功能
        public string ExpandRefs(string openApiJson)
        {
            JObject openApi = JObject.Parse(openApiJson);
            JObject expandedOpenApi = ExpandRefs(openApi);
            return expandedOpenApi.ToString();
        }

        private JObject ExpandRefs(JObject root)
        {
            foreach (var token in root.DescendantsAndSelf().OfType<JProperty>())
            {
                if (IsRefProperty(token))
                {
                    string refPath = token.Value.ToString();
                    JToken referencedObject = FindRef(root, refPath);
                    token.Parent.Replace(referencedObject);
                }
            }
            return root;
        }

        private bool IsRefProperty(JProperty property)
        {
            return property.Name == "$ref" && property.Value.Type == JTokenType.String;
        }

        private JToken FindRef(JObject root, string refPath)
        {
            string[] pathSegments = refPath.Split('/');
            JToken current = root;

            foreach (string segment in pathSegments)
            {
                if (segment == "#")
                    continue;

                current = NavigateToSegment(current, segment);
            }

            return current;
        }

        public JToken NavigateToSegment(JToken current, string segment)
        {
            if (current is JObject obj)
            {
                return obj[segment];
            }

            if (current is JArray array)
            {
                if (int.TryParse(segment, out int index) && index >= 0 && index < array.Count)
                {
                    return array[index];
                }
                else
                {
                    throw new InvalidOperationException($"Invalid array index: {segment}");
                }
            }

            throw new InvalidOperationException($"Invalid $ref path segment: {segment}");
        }

    }
}

