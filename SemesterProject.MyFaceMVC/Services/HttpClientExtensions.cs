using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.Services
{
	public static class HttpClientExtensions
	{
        public static Task<HttpResponseMessage> GetFromApi(this HttpClient httpClient, string url)
        {
            return httpClient.GetAsync(url);
        }
        public static Task<HttpResponseMessage> PostToApiAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return httpClient.PostAsync(url, content);
        }
        public static Task<HttpResponseMessage> PatchToApiAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return httpClient.PatchAsync(url, content);
        }
        public static Task<HttpResponseMessage> DeleteFromApi(this HttpClient httpClient, string url)
        {
            return httpClient.DeleteAsync(url);
        }
        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException("Something went wrong calling the API");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
