using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace AuthTutorial.Http
{
    public abstract class HttpClientBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<HttpClientBase> _logger;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public HttpClientBase(HttpClient httpClient, ILogger<HttpClientBase> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task PostAsync(
            string route,
            object? body = null,
            Dictionary<string, string>? headers = null,
            CancellationToken cancellationToken = default)
        {
            using var request = BuildRequest(HttpMethod.Post, route, headers, body);
            using var response = await ExecuteAsync(request, cancellationToken);
        }

        public async Task<TResult> PostAsync<TResult>(
            string route,
            object? body = null,
            Dictionary<string, string>? headers = null,
            CancellationToken cancellationToken = default)
            where TResult : class
        {
            using var request = BuildRequest(HttpMethod.Post, route, headers, body);

            using var response = await ExecuteAsync(request, cancellationToken);

            return await response.Content.ReadFromJsonAsync<TResult>(_jsonOptions, cancellationToken);
        }

        public async Task<TResult> GetAsync<TResult>(
            string route,
            Dictionary<string, string>? query = null,
            Dictionary<string, string>? headers = null,
            CancellationToken cancellationToken = default)
            where TResult : class
        {
            var queryString = query is null ? route : QueryHelpers.AddQueryString(route, query);
            
            using var request = BuildRequest(HttpMethod.Get, queryString, headers);

            using var response = await ExecuteAsync(request, cancellationToken);

            return await response.Content.ReadFromJsonAsync<TResult>(_jsonOptions, cancellationToken);
        }

        public async Task PutAsync(
            string route,
            object? body = null,
            Dictionary<string, string>? headers = null,
            CancellationToken cancellationToken = default)
        {
            using var request = BuildRequest(HttpMethod.Put, route, headers, body);

            using var _ = ExecuteAsync(request, cancellationToken);
        }

        public async Task DeleteAsync(
            string route,
            Dictionary<string, string>? headers = null,
            CancellationToken cancellationToken = default)
        {
            using var request = BuildRequest(HttpMethod.Delete, route, headers);

            using var _ = await ExecuteAsync(request, cancellationToken);
        }

        public async Task PatchAsync(
            string route,
            object? body,
            Dictionary<string, string>? headers = null,
            CancellationToken cancellationToken = default)
        {
            using var request = BuildRequest(HttpMethod.Patch, route, headers, body);

            using var _ = await ExecuteAsync(request, cancellationToken);
        }

        private HttpRequestMessage BuildRequest(
            HttpMethod method,
            string route,
            Dictionary<string, string>? headers = null,
            object? body = null)
        {
            var request = new HttpRequestMessage(method, route);

            if (headers is not null && headers.Any())
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }

            HttpContent? content = null;

            if (body is not null)
            {
                var memoryStream = new MemoryStream();
                JsonSerializer.Serialize(memoryStream, body, _jsonOptions);
                memoryStream.Seek(0, SeekOrigin.Begin);

                content = new StreamContent(memoryStream);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            request.Content = content;

            return request;
        }

        private async Task<HttpResponseMessage> ExecuteAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Executing {Method} request to {BaseAddress}", request.Method, _httpClient.BaseAddress);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            
            _logger.LogInformation("Got response with status code {Code}", response.StatusCode);
            
            return response;
        }
    }
}
