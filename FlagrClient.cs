using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace UnityFlagr
{

    class FlagrClient
    {
        readonly HttpClient client = new HttpClient();
        readonly string host;

        public FlagrClient(string host)
        {
            this.host = host;
        }

        public async Task<EvalResult> PostEvaluation(EvalContext evalContext)
        {
            var content = await SerializeAsync(evalContext);
            var resp = await client.PostAsync($"{this.host}/evaluation", content);
            var respContent = resp.Content;
            return await DeserializeAsync<EvalResult>(respContent);
        }

        public async Task<BatchEvalResult> PostBatchEvaluation(BatchEvalContext evalContext)
        {
            var content = await SerializeAsync(evalContext);
            var resp = await client.PostAsync($"{this.host}/evaluation/batch", content);
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
                var _serializer = JsonSerializer.Create();
                _serializer.Serialize(jsonWriter, data);
                jsonWriter.Flush();

                json = writer.ToString();
            }

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return Task.FromResult<HttpContent>(content);
        }

        async Task<T> DeserializeAsync<T>(HttpContent content)
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