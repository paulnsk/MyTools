﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace CoreAppHelpers.Services
{
    public abstract class ABasicApiClient(HttpClient httpClient, ILoggerFactory loggerFactory)
    {
        public ILogger Logger { get; set; } = loggerFactory.CreateLogger<ABasicApiClient>();

        public async Task<TResponse?> Get<TResponse>(string url)
        {
            var response = await ApiCall(HttpMethod.Get, url);
            if (typeof(TResponse) == typeof(string)) return (TResponse)(object)response!;
            return DeserializeResponse<TResponse>(response);
        }

        public async Task<TResponse?> Post<TRequest, TResponse>(string url, TRequest request)
        {
            var postData = typeof(TRequest) == typeof(string) ? $"\"{(string)(object)request!}\"" : Serialize(request);
            //Logger.LogCritical("УРЛ " + url);
            //Logger.LogCritical("ПОСТИНГ " + postData);
            var response = await ApiCall(HttpMethod.Post, url, postData);
            if (typeof(TResponse) == typeof(string)) return (TResponse)(object)response!;
            return DeserializeResponse<TResponse>(response);
        }

        public Task Delete(string url)
        {
            return ApiCall(HttpMethod.Delete, url);
        }

        private async Task<string> ApiCall(HttpMethod method, string url, string? body = default)
        {
            using var request = new HttpRequestMessage(method, url);
            if (body != default) request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            using var response = await httpClient.SendAsync(request);
            var responseStr = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode) return responseStr;
            var statusCode = string.IsNullOrWhiteSpace(responseStr) ? response.StatusCode.ToString() : ((int)response.StatusCode).ToString();
            throw new Exception($"Server error: [{statusCode}] {responseStr}");
        }


        private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web) { Converters = { new JsonStringEnumConverter() }, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        private static string Serialize<T>(T x)
        {
            return JsonSerializer.Serialize(x, JsonSerializerOptions);
        }

        private static T DeserializeResponse<T>(string response)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(response)) throw new Exception("Response is empty");
                var result = JsonSerializer.Deserialize<T>(response, JsonSerializerOptions);
                if (result == null) throw new Exception("Deserialization of response produced null");
                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"{e.Message}; <<<{response}>>>");
            }
        }

        

    }
}
