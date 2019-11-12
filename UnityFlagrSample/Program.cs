using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Text;

using UnityFlagr;

using Newtonsoft.Json;

class Program
{
    public static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        var httpClient = new HttpClient();

        var client = new FlagrClient(httpClient, "https://try-flagr.herokuapp.com/api/v1");

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

            var resp = await client.PostEvaluationBatch(req);
            Console.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
        }
    }
}