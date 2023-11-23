using Newtonsoft.Json;

namespace ConvertJsonToWord.Model
{
   
    public class MyOpenApiObject
    {
        
        public string openapi { get; set; }
        public Info info { get; set; }
        public Dictionary<string, PathItem> paths { get; set; }
        public Dictionary<string, PathItem> newPath { get; set; }
        public Components components { get; set; }


        public class PathItem
        {
            public Operations get {  get; set; }
            public Operations post { get; set; }
            public Operations put { get; set; }
            public Operations delete { get; set; }
            // methods存放以上4種operations中唯一一個有值的 避免要分別寫4次針對Operations的操作
            public Dictionary<string, Operations> methods { get; set; }
        }

        public class Operations
        {
            public string summary {  get; set; }
            public List<Parameter> parameters { get; set; }
            public string operationId {  get; set; }
            public Dictionary<string, Responses> responses { get; set; }
        }
        public class Info
        {
            public string title { get; set; }
            public string version { get; set; }
        }

        public class Parameter
        {
            public string result { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            // in是保留字 不能取這個名字
            [JsonProperty("in")]
            public string pin {get; set;}
            public bool require {  get; set;}

        }

        public class Responses
        {
            public string description { get; set; }
            public Dictionary<string, Content> content { get; set; }
        }
        public class Content
        {
            public Schema schema { get; set; }
        }

        public class Schema
        {
            public Dictionary<string, Schemas> schemas { get; set; }
            public string Type { get; set; }
            public Items Items { get; set; }

            [JsonProperty("$ref")]
            public string Ref { get; set; }
            public string newRef { get; set; }
        }
        public class Items
        {
            public string result { get; set; }
            public string type { get; set; }
            public Dictionary<string, Property> properties { get; set; }
            //public AdditionalProperties additionalProperties { get; set; }
            public string description { get; set; }
            public Components components { get; set; }
            public Dictionary<string, Schemas> schemas { get; set; }
            [JsonProperty("$ref")]
            public string Ref { get; set; }
            public string newRef { get; set; }
        }

        public class Components
        {
            public Dictionary<string, Schemas> schemas { get; set; }
        }

        public class Schemas
        {
            public string type { get; set; }
            public Dictionary<string, Property> properties { get; set; }
            //public AdditionalProperties additionalProperties { get; set; }
            public string description { get; set; }
        }

        //public class AdditionalProperties
        //{
        //    [JsonProperty("$ref")]
        //    public string Ref { get; set; }
        //    public string newRef { get; set; }
        //}


        public class Property
        {
            // properties中加入properties，讓json在DeserializeObject的時候能把物件底下的物件也寫進去
            // 直到properties = null為止
            public Dictionary<string, Property> properties { get; set; }
            public string type { get; set; }
            public string format { get; set; }
            public string description { get; set; }
            public bool readOnly { get; set; }
            public bool nullable { get; set; }
        }

    }

}
