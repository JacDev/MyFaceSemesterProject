using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.Services
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> GetFromApiAsync(this HttpClient httpClient, string url)
        {
            try
            {
                return httpClient.GetAsync(url);
            }
            catch
            {
                throw;
            }
        }
        public static Task<HttpResponseMessage> PostToApiAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            try
            {
                StringContent content = AddContent(data);
                return httpClient.PostAsync(url, content);
            }
            catch
            {
                throw;
            }
        }
        public static Task<HttpResponseMessage> PatchToApiAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            try
            {
                StringContent content = AddContent(data);
                return httpClient.PatchAsync(url, content);
            }
            catch
            {
                throw;
            }
        }
        public static Task<HttpResponseMessage> DeleteFromApi(this HttpClient httpClient, string url)
        {
            try
            {
                return httpClient.DeleteAsync(url);
            }
            catch
            {
                throw;
            }
        }
        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            try
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException($"Something went wrong calling the API. HTTP code: {response.StatusCode}");
                }

                string dataAsString = await response.Content.ReadAsStringAsync();
                var responseData =  JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return responseData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        private static StringContent AddContent<T>(T data)
        {
            try
            {
                string dataAsString = JsonSerializer.Serialize(data);
                StringContent content = new StringContent(dataAsString);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return content;
            }
            catch
            {
                throw;
            }
        }
    }
}