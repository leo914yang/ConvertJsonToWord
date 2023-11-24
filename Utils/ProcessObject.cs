using ConvertJsonToWord.Model;
using DocumentFormat.OpenXml.Vml.Office;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using static ConvertJsonToWord.Model.MyOpenApiObject;

namespace ConvertJsonToWord.Utils
{
    public class ProcessObject
    {
        //public void showStructure(object obj, int indentLevel = 0)
        //{
        //    Type type = obj?.GetType();

        //    if (type != null && type.GetProperties().Any())
        //    {
        //        foreach (var property in type.GetProperties())
        //        {
        //            // 取得屬性值
        //            var value = property?.GetValue(obj);
        //            var newIndentLevel = indentLevel + 1;
        //            // 如果屬性的類型是自定義類型，遞迴呼叫 showStructure
        //            if (property?.PropertyType.Namespace != null && property.PropertyType.Namespace != "System")
        //            {
        //                if (IsDictionaryType(property.PropertyType))
        //                {
        //                    // 如果是 Dictionary 類型，印出 key，然後遞迴處理 value
        //                    var dictionary = value as IDictionary;
        //                    if (dictionary != null)
        //                    {
        //                        foreach (DictionaryEntry entry in dictionary)
        //                        {
        //                            Debug.WriteLine($"{new string('\t', indentLevel)}屬性名稱: {property.Name}");
        //                            Debug.WriteLine($"{new string('\t', indentLevel)}Dictionary Key: {entry.Key}");
        //                            if (entry.Value != null)
        //                            {
        //                                showStructure(entry.Value, newIndentLevel);
        //                            }

        //                        }
        //                    }
        //                }
        //                else if (IsListType(property.PropertyType))
        //                {
        //                    // 如果是 List 類型，遞迴處理每個元素
        //                    var list = value as IList;
        //                    if (list != null)
        //                    {
        //                        foreach (var listItem in list)
        //                        {
        //                            Debug.WriteLine($"{new string('\t', indentLevel)}屬性名稱: {property.Name}");
        //                            showStructure(listItem, newIndentLevel);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    Debug.WriteLine($"{new string('\t', indentLevel)}屬性名稱: {property.Name}");
        //                    showStructure(value, newIndentLevel);
        //                }
        //            }
        //        }
        //    }

        //}

        private bool IsDictionaryType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        private bool IsListType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>);
        }


        public void showStructure(object obj, string target, int indentLevel = 0)
        {
            Type type = obj?.GetType();
            var trans = new Transform();
            if (type != null && type.GetProperties().Any())
            {
                foreach (var property in type.GetProperties())
                {
                    // 取得屬性值
                    var value = property?.GetValue(obj);
                    var newIndentLevel = indentLevel + 1;
                    // 如果屬性的類型是自定義類型，遞迴呼叫 showStructure
                    if (property?.PropertyType.Namespace != null && property.PropertyType.Namespace != "System")
                    {
                        if (IsDictionaryType(property.PropertyType))
                        {
                            // 如果是 Dictionary 類型，印出 key，然後遞迴處理 value
                            var dictionary = value as IDictionary;
                            if (dictionary != null)
                            {
                                foreach (DictionaryEntry entry in dictionary)
                                {
                                    Debug.WriteLine($"{new string('\t', indentLevel)}屬性名稱: {property.Name}");
                                    Debug.WriteLine($"{new string('\t', indentLevel)}Dictionary Key: {entry.Key}");
                                    
                                    if (entry.Value != null)
                                    {
                                        showStructure(entry.Value, target, newIndentLevel);
                                    }

                                }
                            }
                        }
                        else if (IsListType(property.PropertyType))
                        {
                            // 如果是 List 類型，遞迴處理每個元素
                            var list = value as IList;

                            if (list != null)
                            {
                                foreach (var listItem in list)
                                {
                                    Debug.WriteLine($"{new string('\t', indentLevel)}屬性名稱: {property.Name}");
                                    showStructure(listItem, target, newIndentLevel);
                                }
                            }
                        }
                        else
                        {
                            if (property?.Name.Equals(target) == true)
                            {
                                // 建立Items物件 藉此拿到Items的屬性
                                var items = property.GetValue(obj) as Items;
                                if (items != null)
                                {
                                    // 取得Items底下的properties
                                    var properties = items.properties;

                                    var responseJson = JsonConvert.SerializeObject(properties, Formatting.Indented);
                                    var jsonResponse = JsonConvert.DeserializeObject<JObject>(responseJson);

                                    string jsonResponseResult = string.Empty;
                                    var finalResponseResult = trans.TransformJson(jsonResponse, jsonResponseResult);

                                    // 資料寫入result
                                    items.result = "{\n" + finalResponseResult.Substring(0, finalResponseResult.Length - 2) + "\n}";
                                }
                                
                            }
                            Debug.WriteLine($"{new string('\t', indentLevel)}屬性名稱: {property.Name}");
                            showStructure(value, target, newIndentLevel);
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

