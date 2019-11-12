using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityFlagr;

class Program
{
    static async Task Main(string[] args)
    {
        var httpClient = new HttpClient();
        var client = new FlagrClient(httpClient, "https://try-flagr.herokuapp.com/api/v1");

        await ExecutePostEvaluation(client);
        await ExecutePostEvaluationBatch(client);
    }

    static async Task ExecutePostEvaluation(FlagrClient client)
    {
        var req = new EvalContext()
        {
            entityID = "127",
            entityType = "user",
            entityContext = new Dictionary<string, object>() { { "state", "NY" } },
            flagID = 1,
            enableDebug = true,
        };

        var resp = await client.PostEvaluation(req);
        Console.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
    }

    static async Task ExecutePostEvaluationBatch(FlagrClient client)
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
            flagIDs = new List<int>() { 1 },
        };

        var resp = await client.PostEvaluationBatch(req);
        Console.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
    }
}