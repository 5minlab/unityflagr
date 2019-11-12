using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace testtest
{
    class EvalContext
    {
        public string entityID;
        public string entityType;
        public Dictionary<string, object> entityContext;
        public bool enableDebug;
        public int? flagID;
        public string flagKey;
    }

    class EvalBatchReq
    {
        public List<Entity> entities;
        public bool enableDebug;
        public List<int> flagIDs;
        public List<string> flagKeys;
    }

    class Entity
    {
        public string entityID;
        public string entityType;
        public Dictionary<string, object> entityContext;
    }

    class EvalBatchResult
    {
        public List<EvalResult> evaluationResults;
    }

    class EvalResult
    {
        public int flagID;
        public string flagKey;
        public int flagSnapshotID;
        public int segmentID;
        public int variantID;
        public string variantKey;
        public Dictionary<string, object> variantAttachment;
        public EvalContext evalContext;
        public string timestamp;
    }

    class Program
    {
        public static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            var req = new EvalContext();
            req.entityID = "127";
            req.entityType = "user";
            req.entityContext = new Dictionary<string, object>(){
                {"state", "NY"}
            };
            // req.flagID = 1;
            req.flagKey = "kmmcd1nsd6ze56chh";
            req.enableDebug = true;


            var content = await SerializeAsync(req);
            var resp = await client.PostAsync("https://try-flagr.herokuapp.com/api/v1/evaluation", content);
            var respContent = resp.Content;
            var result = await DeserializeAsync<EvalResult>(respContent);
            Console.WriteLine(JsonConvert.SerializeObject(result));
        }


        public static Task<HttpContent> SerializeAsync<T>(T data)
        {
            if (data == null)
            {
                return null;
            }

            string json;

            using (var writer = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                var _serializer = JsonSerializer.Create();
                _serializer.Serialize(jsonWriter, data);
                jsonWriter.Flush();

                json = writer.ToString();
            }

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return Task.FromResult<HttpContent>(content);
        }

        public static async Task<T> DeserializeAsync<T>(HttpContent content)
        {
            var _serializer = JsonSerializer.Create();
            using (var s = await content.ReadAsStreamAsync().ConfigureAwait(false))
            using (var sr = new StreamReader(s))
            {
                return (T)_serializer.Deserialize(sr, typeof(T));
            }
        }
    }
}