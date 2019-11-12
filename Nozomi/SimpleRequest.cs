using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nozomi
{
    public class SimpleRequest : IDisposable
    {
        HttpClient Client { get; }
        HttpRequestMessage Message { get; set; }
        IContentSerializer RequestSerializer { get; set; }
        IContentSerializer ResponseSerializer { get; set; }
        CancellationToken CancellationToken { get; set; }

        public SimpleRequest(
            HttpClient client,
            HttpRequestMessage message,
            CancellationToken cancellationToken,
            IContentSerializer requestSerializer,
            IContentSerializer responseSerializer
        )
        {
            Client = client;
            Message = message;
            CancellationToken = cancellationToken;
            RequestSerializer = requestSerializer;
            ResponseSerializer = responseSerializer;

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(ResponseSerializer.ContentType));
        }

        public async Task<TResp> Request<TResp>()
        {
            using (var response = await Client.SendAsync(Message, CancellationToken))
            {
                return await GetResponse<TResp>(response);
            }
        }

        public async Task<string> Request()
        {
            using (var response = await Client.SendAsync(Message, CancellationToken))
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<TResp> Request<TReq, TResp>(TReq req)
        {
            if (Message.Method == HttpMethod.Get)
            {
                var querystring = req.ToQueryString();
                Message.RequestUri = new Uri(Message.RequestUri.ToString() + querystring);
            }
            else
            {
                Message.Content = await RequestSerializer.SerializeAsync<TReq>(req);
                var body = await Message.Content.ReadAsStringAsync();
            }
            using (var response = await Client.SendAsync(Message, CancellationToken))
            {
                return await GetResponse<TResp>(response);
            }
        }

        private async Task<string> ReadMessage(HttpContent content)
        {
            using (var s = await content.ReadAsStreamAsync().ConfigureAwait(false))
            using (var sr = new StreamReader(s))
            {
                var body = sr.ReadToEnd();
                return body;
            }
        }

        private async Task<ErrorResponse> ParseError(HttpResponseMessage resp)
        {
            try
            {
                var body = await ReadMessage(resp.Content);
                Console.WriteLine(body);

                var err = await ResponseSerializer.DeserializeAsync<ErrorResponse>(resp.Content);
                // var body = await ReadMessage(resp.Content);
                // UnityEngine.Debug.LogError(body);
                return err;
            }
            catch (JsonReaderException ex)
            {
                var body = await ReadMessage(resp.Content);
                throw new ParseException(resp, ex, body);
            }
            catch (Exception ex)
            {
                throw new ServerException(resp, ex.Message, ex.HResult, ex.StackTrace);
            }
        }

        private async Task HandleError(HttpResponseMessage resp)
        {
            var err = await ParseError(resp);

            if ((int)resp.StatusCode == 500)
            {
                throw new ServerException(resp, err.message, err.code, err.name);
            }
            else if (err.code == 102)
            {
                // 서버에서 하드코딩된 에러 코드 중 일부는 따로 대응 가능
                throw new TokenExpireException(resp, err);
            }
            else
            {
                throw new HttpException(resp, err.message, err.code, err.name);
            }
        }

        private async Task<TResp> GetResponse<TResp>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                await HandleError(response);

            var resp = await ResponseSerializer.DeserializeAsync<TResp>(response.Content);
            return resp;
        }
        public void Dispose()
        {
        }
    }
}
