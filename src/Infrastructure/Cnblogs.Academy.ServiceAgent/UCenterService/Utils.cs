using System;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Cnblogs.Academy.ServiceAgent.UCenterService
{
    public static class HttpClientExtensions
    {
        private const string ValidationKey = "validation";

        /// <summary>
        /// 在请求头附加 API 服务验证 Token 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static HttpContent AppendValidation(this HttpContent content, string token)
        {
            content.Headers.Add(ValidationKey, token);
            return content;
        }

        /// <summary>
        /// 在请求头附加 API 服务验证 Token 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static HttpRequestMessage AppendValidation(this HttpRequestMessage request, string token)
        {
            request.Headers.Add(ValidationKey, token);
            return request;
        }

        /// <summary>
        /// 创建验证 API 服务的 Token
        /// </summary>
        /// <param name="serviceId">服务标志</param>
        /// <returns></returns>
        private static string CreateToken(string serviceId)
        {
            string str = serviceId + DateTime.Now.ToFileTimeUtc();
            return ComputeHash(str);
        }

        private static string ComputeHash(string str)
        {
            using (var md5 = MD5.Create())
            {
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }

        /// <summary>
        /// 验证响应是否由指定服务返回
        /// </summary>
        /// <param name="resp"></param>
        /// <param name="token"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>
        public static bool ValidateApiService(this HttpResponseMessage resp, string token, string serviceId)
        {
            if (resp.Headers.TryGetValues("validation", out var values) == false) return false;
            var echoToken = values.FirstOrDefault();
            if (echoToken == null)
            {
                return false;
            }

            return ComputeHash(token + serviceId) == echoToken;
        }

        public static ValidatedResponse ToValidatedResponse(
            this HttpResponseMessage resp,
            string serviceId,
            string token)
        {
            return new ValidatedResponse(resp.ValidateApiService(token, serviceId), resp);
        }

        /// <summary>
        /// 使用匿名对象创建查询字符串，匿名对象的属性名作为键，对应的属性值使用 ToString 方法转换后作为值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string QueryFrom(object obj)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            var props = obj.GetType().GetProperties();
            foreach (var prop in props)
            {
                query[prop.Name] = prop.GetValue(obj).ToString();
            }

            return query.ToString();
        }

        public static async Task<ValidatedResponse> PostAsJsonAsync<T>(
            this HttpClient httpClient,
            string requestUri,
            T value,
            string serviceId)
        {
            var token = CreateToken(serviceId);
            var resp = await httpClient.PostAsync(
                requestUri,
                new StringContent(
                    JsonConvert.SerializeObject(value),
                    Encoding.UTF8,
                    "application/json").AppendValidation(token));
            return resp.ToValidatedResponse(serviceId, token);
        }

        public static async Task<ValidatedResponse> PutAsJsonAsync<T>(
            this HttpClient httpClient,
            string requestUri,
            T value,
            string serviceId)
        {
            var token = CreateToken(serviceId);
            var resp = await httpClient.PutAsync(
                requestUri,
                new StringContent(
                    JsonConvert.SerializeObject(value),
                    Encoding.UTF8,
                    "application/json").AppendValidation(token));
            return resp.ToValidatedResponse(serviceId, token);
        }

        /// <summary>
        /// 使用查询字符串发送 GET 请求
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="requestUri"></param>
        /// <param name="value">查询字符串参数</param>
        /// <param name="serviceId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<ValidatedResponse> GetAsync(
            this HttpClient httpClient,
            string requestUri,
            object value = null,
            string serviceId = "")
        {
            var token = CreateToken(serviceId);
            string qs = value == null ? "" : $"?{QueryFrom(value)}";
            var resp = await httpClient.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, $"{requestUri}{qs}")
                    .AppendValidation(token));
            return resp.ToValidatedResponse(serviceId, token);
        }

        public static async Task<ValidatedResponse> HeadAsync(
            this HttpClient httpClient,
            string requestUri,
            string serviceId)
        {
            var token = CreateToken(serviceId);
            var resp = await httpClient.SendAsync(
                new HttpRequestMessage(HttpMethod.Head, requestUri).AppendValidation(token));
            return resp.ToValidatedResponse(serviceId, token);
        }

        public static async Task<ValidatedResponse> DeleteAsync(
            this HttpClient httpClient,
            string requestUri,
            string serviceId)
        {
            var token = CreateToken(serviceId);
            var resp = await httpClient.SendAsync(
                new HttpRequestMessage(HttpMethod.Delete, requestUri).AppendValidation(token));
            return resp.ToValidatedResponse(serviceId, token);
        }

        public static async Task<ValidatedResponse> PatchAsJsonAsync<T>(
            this HttpClient httpClient,
            string requestUri,
            T value,
            string serviceId)
        {
            var token = CreateToken(serviceId);
            var resp = await httpClient.SendAsync(
                new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(value),
                        Encoding.UTF8,
                        "application/json").AppendValidation(token)
                });
            return resp.ToValidatedResponse(serviceId, token);
        }

        public static async Task<ValidatedResponse> GetAsJsonAsync<T>(
            this HttpClient httpClient,
            string requestUri,
            T value,
            string serviceId)
        {
            var token = CreateToken(serviceId);
            var resp = await httpClient.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, requestUri)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(value),
                        Encoding.UTF8,
                        "application/json").AppendValidation(token),
                    Headers = { { "Accept", "application/json" } }
                });
            return resp.ToValidatedResponse(serviceId, token);
        }
    }

    public class ValidatedResponse
    {
        public ValidatedResponse(bool isValidated, HttpResponseMessage response)
        {
            IsValidated = isValidated;
            Response = response;
        }

        public bool IsValidated { get; }
        public HttpResponseMessage Response { get; }
    }
}
