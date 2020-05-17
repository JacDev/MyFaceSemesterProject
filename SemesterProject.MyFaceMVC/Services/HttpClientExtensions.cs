using SemesterProject.ApiData.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace SemesterProject.MyFaceMVC.Services
{
	public static class HttpClientExtensions
	{
        public static Task<HttpResponseMessage> PostToApiAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);

            var content = new StringContent(dataAsString);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpClient.PostAsync(url, content);
        }
        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException("Something went wrong calling the API");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        public static Task<HttpResponseMessage> GetFromApi(this HttpClient httpClient, string url)
        {
              return httpClient.GetAsync(url);
        }
        public static Task<HttpResponseMessage> DeleteFromApi(this HttpClient httpClient, string url)
        {
            return httpClient.DeleteAsync(url);
        }
        public async static Task RegisterUser(this HttpClient httpClient, ClaimsPrincipal user)
        {
            UserToSend userToSend = new UserToSend
            {
                Id = Guid.Parse(user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value),
                FirstName = user.Claims.FirstOrDefault(x => x.Type == "FirstName").Value,
                LastName = user.Claims.FirstOrDefault(x => x.Type == "LastName").Value,
            };
            await PostToApiAsJsonAsync(httpClient, "api/users", userToSend);
        }
        public static Task<HttpResponseMessage> PatchToApiAsJsonAsync<T>(this HttpClient httpClient, string url, T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);

            var content = new StringContent(dataAsString);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpClient.PatchAsync(url, content);
        }
    }
}
