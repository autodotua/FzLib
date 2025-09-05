using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace FzLib.Net
{
    public class HttpRequester : IDisposable
    {
        protected readonly HttpClient httpClient;

        private readonly bool canDisposeHttpClient;

        public HttpRequester()
        {
            canDisposeHttpClient = true;
            httpClient = new HttpClient();
        }

        public HttpRequester(HttpClient httpClient)
        {
            canDisposeHttpClient = false;
            this.httpClient = httpClient;
        }

        protected virtual string BaseUrl { get; } = "";

        public void Dispose()
        {
            if (canDisposeHttpClient)
            {
                httpClient.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        public Task<T> GetObjectAsync<T>(string endpoint, JsonTypeInfo<T> jsonTypeInfo,
            ICollection<(string Key, string Value)> query, CancellationToken ct = default) where T : class
        {
            var queries = query.Select(p => $"{p.Key}={p.Value}");
            var queryString = query.Count == 0
                ? string.Empty
                : "?" + string.Join('&', query.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));

            return GetObjectAsync(endpoint + queryString, jsonTypeInfo, ct);
        }

        public async Task<T> GetObjectAsync<T>(string endpoint, JsonTypeInfo<T> jsonTypeInfo,
            CancellationToken ct = default)
        {
            var obj = await GetAsync(endpoint, ct);
            if (obj == null)
            {
                throw new Exception("请求的资源返回为空");
            }

            await using var responseStream = await obj.ReadAsStreamAsync(ct);
            return await JsonSerializer.DeserializeAsync(responseStream, jsonTypeInfo, ct);
        }

        public async Task PostAsync<TData>(string endpoint, object data, JsonTypeInfo<TData> jsonTypeInfo,
            CancellationToken ct = default)
        {
            OnSending();
            ArgumentException.ThrowIfNullOrWhiteSpace(endpoint);
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(jsonTypeInfo);
            var jsonContent = new StringContent(JsonSerializer.Serialize(data, jsonTypeInfo), Encoding.UTF8,
                "application/json");
            var response = await HttpPostAsync(endpoint, jsonContent, ct);
            await ProcessError(response, endpoint);
        }

        public async Task PostAsync(string endpoint, CancellationToken ct = default)
        {
            OnSending();
            var response = await HttpPostAsync(endpoint, null, ct);
            await ProcessError(response, endpoint);
        }

        public async Task<TResult> PostAsync<TData, TResult>(string endpoint, JsonTypeInfo<TResult> jsonResultTypeInfo,
            object data, JsonTypeInfo<TData> jsonDataTypeInfo, CancellationToken ct = default)
        {
            OnSending();
            ArgumentException.ThrowIfNullOrWhiteSpace(endpoint);
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(jsonResultTypeInfo);
            ArgumentNullException.ThrowIfNull(jsonDataTypeInfo);
            var jsonContent = new StringContent(JsonSerializer.Serialize(data, jsonDataTypeInfo), Encoding.UTF8,
                "application/json");
            var response = await HttpPostAsync(endpoint, jsonContent, ct);

            await ProcessError(response, endpoint);
            if (response.Content.Headers.ContentLength is null or 0)
            {
                return default;
            }

            return await JsonSerializer.DeserializeAsync(await response.Content.ReadAsStreamAsync(ct),
                jsonResultTypeInfo, ct);
        }

        public async Task<TResult> PostAsync<TResult>(string endpoint, JsonTypeInfo<TResult> jsonResultTypeInfo,
            CancellationToken ct = default)
        {
            OnSending();
            ArgumentException.ThrowIfNullOrWhiteSpace(endpoint);
            ArgumentNullException.ThrowIfNull(jsonResultTypeInfo);
            var response = await HttpPostAsync(endpoint, null, ct);

            await ProcessError(response, endpoint);
            if (response.Content.Headers.ContentLength is null or 0)
            {
                return default;
            }

            return await JsonSerializer.DeserializeAsync(await response.Content.ReadAsStreamAsync(ct),
                jsonResultTypeInfo, ct);
        }

        protected virtual void OnSending()
        {
        }

        private static async Task ProcessError(HttpResponseMessage response, string url)
        {
            ArgumentNullException.ThrowIfNull(response);
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            url = response.RequestMessage.RequestUri.AbsolutePath;

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                throw new HttpStatusCodeException($"404 资源不存在（{url}）", System.Net.HttpStatusCode.NotFound);
            }

            var message = await response.Content.ReadAsStringAsync();
            message = message.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault() ?? string.Empty;
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new HttpStatusCodeException($"500 服务器错误（{url}）：{message}", response.StatusCode);
            }

            throw new HttpStatusCodeException(
                $"{(int)response.StatusCode} {response.StatusCode}（{url}）：{message}", response.StatusCode);
        }

        private async Task<HttpContent> GetAsync(string endpoint, CancellationToken ct = default)
        {
            OnSending();

            HttpResponseMessage response = await HttpGetAsync(endpoint, ct);


            if (response.IsSuccessStatusCode)
            {
                return response.Content;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            await ProcessError(response, endpoint);
            throw new Exception("未知错误");
        }

        private string GetUrl(string endpoint)
        {
            return string.IsNullOrWhiteSpace(BaseUrl) ? endpoint : ($"{BaseUrl.TrimEnd('/')}/{endpoint}");
        }

        private async Task<HttpResponseMessage> HttpGetAsync(string endpoint, CancellationToken ct = default)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(GetUrl(endpoint), ct);
                return response;
            }
            catch (TaskCanceledException ex)
            {
                throw new TimeoutException("服务器请求超时", ex);
            }
        }

        private async Task<HttpResponseMessage> HttpPostAsync(string endpoint, HttpContent content,
            CancellationToken ct = default)
        {
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(GetUrl(endpoint), content, ct);
                return response;
            }
            catch (TaskCanceledException ex)
            {
                throw new TimeoutException("服务器请求超时", ex);
            }
        }
    }
}