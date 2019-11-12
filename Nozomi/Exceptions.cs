using System;
using System.Net.Http;

namespace Nozomi
{
    public class HttpException : Exception
    {
        private readonly HttpResponseMessage resp;
        private HttpRequestMessage req { get => resp.RequestMessage; }

        public HttpMethod Method { get => req.Method; }
        public Uri Uri { get => req.RequestUri; }

        public string message { get => base.Message; }

        public readonly int code;
        public readonly string name;

        public int status { get => (int)resp.StatusCode; }

        public HttpException(
            HttpResponseMessage resp,
            string message,
            int code,
            string name
        )
        : base(message)
        {
            this.resp = resp;
            this.code = code;
            this.name = name;
        }

        public HttpException(
            HttpResponseMessage resp,
            ErrorResponse err
        )
        : this(resp, err.message, err.code, err.name)
        {
        }

        public void LogError()
        {
            //Debug.LogErrorFormat(
            //    "{0} {1} {2} code={3} message={4}",
            //    Method,
            //    Uri,
            //    status,
            //    code,
            //    message
            //);
            Console.WriteLine(GetLog());
        }

        public string GetLog()
        {
            return string.Format("{0} {1} {2} code={3} message={4}",
                Method,
                Uri,
                status,
                code,
                message);
        }
    }

    public class TokenExpireException : HttpException
    {
        public TokenExpireException(
            HttpResponseMessage resp,
            ErrorResponse err
        )
        : base(resp, err)
        {
        }
    }

    public class ServerException : HttpException
    {
        public ServerException(
            HttpResponseMessage resp,
            string message,
            int code,
            string name
        ) : base(resp, message, code, name)
        {
        }
    }

    public class ParseException : HttpException
    {
        public ParseException(
            HttpResponseMessage resp,
            Exception ex,
            string body
        )
        : base(resp, body, 0, ex.Message)
        {
        }
    }
}
