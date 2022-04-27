using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynaRAP.Data
{
    public class JsonData
    {
        public int code { get; set; }
        public string message { get; set; }
        public Response response { get; set; }
        public ResponseAt responseAt { get; set; }
        public int resultCount { get; set; }

        public JsonData(int code, string message)
        {
            this.code = code;
            this.message = message;
        }
    }

    public class ParameterJsonData : JsonData
    {
        public ParameterJsonData(int code, string message) : base(code, message) { }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CreatedAt
    {
        public long timestamp { get; set; }
        public string dateFormat { get; set; }
        public string dateTime { get; set; }
    }

    public class Pool
    {
        public int seq { get; set; }
        public int parentDirSeq { get; set; }
        public string uid { get; set; }
        public string dirName { get; set; }
        public string dirType { get; set; }
        public string dirIcon { get; set; }
        public CreatedAt createdAt { get; set; }
        public string refSeq { get; set; }
        public string refSubSeq { get; set; }
    }

    public class Presentation
    {
        public int dirKey { get; set; }
        public List<object> subTree { get; set; }
    }

    public class Response
    {
        public List<Pool> pools { get; set; }
        public List<Presentation> presentation { get; set; }
        public int seq { get; set; }
    }

    public class ResponseAt
    {
        public long timestamp { get; set; }
        public string dateFormat { get; set; }
        public string dateTime { get; set; }
    }

    







}
