using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace FootballCrimeStatistics.Logic
{
    public abstract class BaseHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;

        private readonly DistributedCacheEntryOptions cacheEntryOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
        };

        protected BaseHttpClient(HttpClient client, IDistributedCache cache)
        {
            _httpClient = client;
            _cache = cache;
        }

        protected async Task<T> Get<T>(string path)
        {
            var key = GenerateCacheKey(path);
            var responseString = await _cache.GetStringAsync(key);

            // If not cached, fetch
            if (responseString == null)
            {
                var errorCount = 0;
                do
                {
                    var response = await _httpClient.GetAsync(path);

                    responseString = await response.Content.ReadAsStringAsync();

                    // If ok, cache and break;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK && responseString != null)
                    {
                        await _cache.SetStringAsync(key, responseString, cacheEntryOptions);
                        break;
                    }

                    // Sleep and try again
                    errorCount++;
                    Thread.Sleep(1 * 2 ^ errorCount);
                } while (errorCount < 5);
            }

            // Couldnt get valid response
            if (responseString == null)
                throw new Exception($"{key} is unreachable");

            return JsonSerializer.Deserialize<T>(responseString);
        }

        protected async Task<T> Post<T>(string path, object data, bool allowCache = false)
        {
            var dataString = JsonSerializer.Serialize(data);

            var key = GenerateCacheKey(path, dataString);
            var responseString = allowCache ? await _cache.GetStringAsync(key) : null;

            // If not cached, fetch
            if (responseString == null)
            {
                var errorCount = 0;
                do
                {
                    var response = await _httpClient.PostAsync(path, new StringContent(dataString, null, "application/json"));

                    responseString = await response.Content.ReadAsStringAsync();
                    // If ok, cache and break;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK && responseString != null)
                    {
                        if (allowCache)
                            await _cache.SetStringAsync(key, responseString, cacheEntryOptions);
                        break;
                    }

                    // Sleep and try again
                    errorCount++;
                    Thread.Sleep(1 * 2 ^ errorCount);
                } while (errorCount < 5);          
            }

            // Couldnt get valid response
            if (responseString == null)
                throw new Exception($"{key} is unreachable");

            return JsonSerializer.Deserialize<T>(responseString);
        }

        private string GenerateCacheKey(string path, string data = "")
        {
            return $"{_httpClient.BaseAddress}{path}{data}";
        }
    }
}
