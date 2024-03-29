using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Threading;
using Fiveminlab.Newtonsoft.Json;

namespace UnityFlagr
{

    public class FlagrClient
    {
        readonly HttpClient client;
        readonly string host;

        public FlagrClient(HttpClient client, string host)
        {
            this.client = client;
            this.host = host;
        }

        public async Task<EvalResult> PostEvaluation(EvalContext evalContext)
        {
            return await PostEvaluation(evalContext, CancellationToken.None);
        }

        public async Task<EvalResult> PostEvaluation(EvalContext evalContext, CancellationToken cancellationToken)
        {
            var content = await SerializeAsync(evalContext);
            var resp = await client.PostAsync($"{this.host}/evaluation", content, cancellationToken);
            var respContent = resp.Content;
            return await DeserializeAsync<EvalResult>(respContent);
        }

        public async Task<BatchEvalResult> PostEvaluationBatch(BatchEvalContext evalContext)
        {
            return await PostEvaluationBatch(evalContext, CancellationToken.None);
        }

        public async Task<BatchEvalResult> PostEvaluationBatch(BatchEvalContext evalContext, CancellationToken cancellationToken)
        {
            var content = await SerializeAsync(evalContext);
            var resp = await client.PostAsync($"{this.host}/evaluation/batch", content, cancellationToken);
            var respContent = resp.Content;
            return await DeserializeAsync<BatchEvalResult>(respContent);
        }

        Task<HttpContent> SerializeAsync<T>(T data)
        {
            if (data == null)
            {
                return null;
            }

            string json;

            using (var writer = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                var serializer = JsonSerializer.Create();
                serializer.Serialize(jsonWriter, data);
                jsonWriter.Flush();

                json = writer.ToString();
            }

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return Task.FromResult<HttpContent>(content);
        }

        async Task<T> DeserializeAsync<T>(HttpContent content)
        {
            var serializer = JsonSerializer.Create();
            using (var s = await content.ReadAsStreamAsync().ConfigureAwait(false))
            using (var sr = new StreamReader(s))
            {
                return (T)serializer.Deserialize(sr, typeof(T));
            }
        }
    }
}