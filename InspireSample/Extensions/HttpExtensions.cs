using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace InspireSample.Extensions
{

    public static class HttpRequestMessageExtensions
    {
        public static HttpResponseMessage Redirect(this HttpRequestMessage self, string url)
        {
            var response = new HttpResponseMessage(HttpStatusCode.Redirect);
            response.Headers.Location = new Uri(url);
            return response;
        }

        public static HttpResponseMessage Text(this HttpRequestMessage self, string content, string mediaType = "text/plain")
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(content);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            return response;
        }
    }

    public static class StringExtensions
    {
        public static string ToJson(this object self, bool prettyPrint = false)
        {
            if (self == null)
                return null;

            var formatting = Formatting.None;
            if (prettyPrint)
                formatting = Formatting.Indented;

            var result = JsonConvert.SerializeObject(self, formatting);
            return result;
        }

        public static T FromJson<T>(this string self)
        {
            return (T)FromJson(self, typeof(T));
        }

        public static object FromJson(this string self, Type type)
        {
            if (self.IsEmpty())
                return null;

            return JsonConvert.DeserializeObject(self, type);
        }

        public static string FormatText(this string self, params object[] args)
        {
            return string.Format(self, args);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> self)
        {
            return self == null || !self.Any();
        }

        public static bool IsEmpty(this string self)
        {
            return string.IsNullOrEmpty(self);
        }
    }

    public static class HttpClientExtensions
    {
        public static HttpResponseMessage Get(this HttpClient self, string url, Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,

            };

            if (headers != null)
                foreach (var h in headers)
                    request.Headers.Add(h.Key, h.Value);

            return self.Send(request, null);
        }

        public static HttpResponseMessage Delete(this HttpClient self, string url, Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Delete,

            };

            if (headers != null)
                foreach (var h in headers)
                    request.Headers.Add(h.Key, h.Value);

            return self.Send(request, null);
        }

        public static HttpResponseMessage Post(this HttpClient self, string url, HttpContent content, Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post
            };

            if (headers != null)
                foreach (var h in headers)
                    request.Headers.Add(h.Key, h.Value);

            return self.Send(request, content);
        }

        public static HttpResponseMessage PostForm(this HttpClient self, string url, Dictionary<string, string> nvp, Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post
            };

            if (headers != null)
                foreach (var h in headers)
                    request.Headers.Add(h.Key, h.Value);

            return self.Send(request, nvp == null ? null : new FormUrlEncodedContent(nvp));
        }

        public static HttpResponseMessage PostMultipartForm(this HttpClient self, string url, Dictionary<string, string> nvp, Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post
            };

            if (headers != null)
                foreach (var h in headers)
                    request.Headers.Add(h.Key, h.Value);

            var content = new MultipartFormDataContent("----WebKitFormBoundary7MA4YWxkTrZu0gW");
            return self.Send(request, nvp == null ? null : content);
        }

        public static HttpResponseMessage PostJson(this HttpClient self, string url, object content, Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
            };

            if (headers != null)
                foreach (var h in headers)
                    request.Headers.Add(h.Key, h.Value);


            return self.PostJson(request, content);
        }

        public static HttpResponseMessage PostJsonAndEncodeNonAscii(this HttpClient self, string url, object content, Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
            };

            if (headers != null)
                foreach (var h in headers)
                    request.Headers.Add(h.Key, h.Value);


            return self.PostJsonAndEncodeNonAscii(request, content);
        }

        public static HttpResponseMessage PostJson(this HttpClient self, HttpRequestMessage request, object content)
        {
            return self.Send(request,
                content == null ? null : new StringContent(content.ToJson(), Encoding.UTF8, "application/json"));
        }

        public static HttpResponseMessage PostJsonAndEncodeNonAscii(this HttpClient self, HttpRequestMessage request, object content)
        {
            var stringContent = JsonConvert.SerializeObject(content, new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
            return self.Send(request,
                content == null ? null : new StringContent(stringContent, Encoding.UTF8, "application/json"));
        }

        public static HttpResponseMessage PutJson(this HttpClient self, string url, object content, Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Put
            };

            if (headers != null)
                foreach (var h in headers)
                    request.Headers.Add(h.Key, h.Value);

            return self.PutJson(request, content);
        }

        public static HttpResponseMessage PutJsonAndEncodeNonAscii(this HttpClient self, string url, object content, Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Put
            };

            if (headers != null)
                foreach (var h in headers)
                    request.Headers.Add(h.Key, h.Value);

            return self.PutJsonAndEncodeNonAscii(request, content);
        }

        public static HttpResponseMessage PutJson(this HttpClient self, HttpRequestMessage request, object content)
        {
            return self.Send(request,
                content == null ? null : new StringContent(content.ToJson(), Encoding.UTF8, "application/json"));
        }

        public static HttpResponseMessage PutJsonAndEncodeNonAscii(this HttpClient self, HttpRequestMessage request, object content)
        {
            var stringContent = JsonConvert.SerializeObject(content, new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii });
            return self.Send(request,
                content == null ? null : new StringContent(stringContent, Encoding.UTF8, "application/json"));
        }

        public static HttpResponseMessage PatchJson(this HttpClient self, string url, object content, Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = new HttpMethod("PATCH")
            };

            if (headers != null)
                foreach (var h in headers)
                    request.Headers.Add(h.Key, h.Value);

            return self.PatchJson(request, content);
        }

        public static HttpResponseMessage PatchJson(this HttpClient self, HttpRequestMessage request, object content)
        {
            return self.Send(request,
                content == null ? null : new StringContent(content.ToJson(), Encoding.UTF8, "application/json"));
        }

        public static HttpResponseMessage Send(this HttpClient self, HttpRequestMessage request, HttpContent content,
            int[] allowedNonSuccessStatusCodes = null)
        {
            if (content != null)
                request.Content = content;

            var result = self.SendAsync(request).Result;

            if (result.IsSuccessStatusCode)
                return result;

            if (!allowedNonSuccessStatusCodes.IsNullOrEmpty() &&
                allowedNonSuccessStatusCodes.Contains((int)result.StatusCode))
                return result;

            if (result.StatusCode != HttpStatusCode.InternalServerError)
                throw new HttpException(request.RequestUri, result.StatusCode, result);

            if (result.Content.Headers.ContentType.MediaType == "application/json")
            {
                var response = result.ReadContentAs<ErrorResponse>();
                if (response != null && !response.ExceptionType.IsEmpty() && !response.ExceptionMessage.IsEmpty())
                {
                    var exceptionType = Type.GetType(response.ExceptionType);
                    if (exceptionType != null)
                        throw (Exception)Activator.CreateInstance(exceptionType, response.ExceptionMessage);
                }
            }

            throw new HttpException(request.RequestUri, result.StatusCode, result);
        }

        public static TResult DownloadJson<TResult>(this HttpClient self, string url, Dictionary<string, string> headers = null)
            where TResult : class
        {
            return self.Get(url, headers).ReadContentAsString().FromJson<TResult>();
        }

        public static string SendRequest(this HttpClient self, string apiUrl, HttpMethod method, object content = null,
            Dictionary<string, string> headers = null)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(apiUrl),
                Method = method
            };

            if (headers != null)
                foreach (var h in headers)
                    request.Headers.Add(h.Key, h.Value);

            if (content != null && method == HttpMethod.Post)
                request.Content = new StringContent(content.ToJson(), Encoding.UTF8, "application/json");

            var result = self.SendAsync(request).Result;

            if (result.IsSuccessStatusCode)
                return result.Content.ReadAsStringAsync().Result;

            if (result.StatusCode != HttpStatusCode.InternalServerError)
                throw new InvalidOperationException("Service '{0}' failed with status code {1}.".FormatText(apiUrl,
                    result.StatusCode));

            var response = result.Content.ReadAsStringAsync().Result.FromJson<ErrorResponse>();
            var exceptionType = Type.GetType(response.ExceptionType);
            if (exceptionType != null)
                throw (Exception)Activator.CreateInstance(exceptionType, response.ExceptionMessage);

            throw new InvalidOperationException(response.ExceptionMessage);
        }
    }



    public class ErrorResponse
    {
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string StackTrace { get; set; }
    }

    public static class HttpResponseMessageExtensions
    {
        public static string ReadContentAsString(this HttpResponseMessage self)
        {
            return self.Content.ReadAsStringAsync().Result;
        }

        public static T ReadContentAs<T>(this HttpResponseMessage self) where T : class
        {
            return self.ReadContentAsString().FromJson<T>();
        }

        public static object ReadContentAs(this HttpResponseMessage self, Type type)
        {
            return self.ReadContentAsString().FromJson(type);
        }
    }

    public class HttpException : Exception
    {
        public Uri RequestUri { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public HttpResponseMessage Response { get; private set; }

        public HttpException(Uri requestUri, HttpStatusCode statusCode, HttpResponseMessage response)
            : base("HTTP request to '{0}' has failed with status code '{1}'.".FormatText(requestUri, statusCode))
        {
            RequestUri = requestUri;
            StatusCode = statusCode;
            Response = response;
        }

        public string GetResponseAsString()
        {
            return Response.ReadContentAsString();
        }
    }

    public class GridEvent
    {
        public string Id { get; set; }
        public string EventType { get; set; }
        public string Subject { get; set; }
        public DateTime EventTime { get; set; }
        public dynamic Data { get; set; }
        public string DataVersion { get; set; }
        public string Topic { get; set; }
    }
}