using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Fiveminlab.Newtonsoft.Json;
using Fiveminlab.Newtonsoft.Json.Linq;
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
        var entityContext = new JObject();
        entityContext.Add("state", "NY");

        var req = new EvalContext()
        {
            entityID = "127",
            entityType = "user",
            entityContext = entityContext,
            flagID = 1,
            enableDebug = true,
        };

        var resp = await client.PostEvaluation(req);
        Console.WriteLine(JsonConvert.SerializeObject(resp, Formatting.Indented));
    }

    static async Task ExecutePostEvaluationBatch(FlagrClient client)
    {
        var entityContext = new JObject();
        entityContext.Add("state", "NY");

        var ent = new Entity()
        {
            entityID = "127",
            entityType = "user",
            entityContext = entityContext,
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