using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace UnityFlagr
{
    class Program
    {
        public static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {

            var client = new FlagrClient("https://try-flagr.herokuapp.com/api/v1");

            {
                var req = new EvalContext()
                {
                    entityID = "127",
                    entityType = "user",
                    entityContext = new Dictionary<string, object>() { { "state", "NY" } },
                    flagKey = "kmmcd1nsd6ze56chh",
                    enableDebug = true,
                };

                var resp = await client.PostEvaluation(req);
                Console.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
            }

            {
                var ent = new Entity()
                {
                    entityID = "127",
                    entityType = "user",
                    entityContext = new Dictionary<string, object>() { { "state", "NY" } },
                };

                var req = new BatchEvalContext()
                {
                    entities = new List<Entity>() { ent, },
                    enableDebug = true,
                    flagKeys = new List<string>() { "kmmcd1nsd6ze56chh" },
                };

                var resp = await client.PostBatchEvaluation(req);
                Console.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
            }
        }
    }
}