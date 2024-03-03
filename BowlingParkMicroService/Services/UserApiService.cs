using System.Net;
using BowlingParkMicroService.Helpers;

namespace BowlingParkMicroService.Services;

public interface IUserApiService
{
    bool UserExists(string userId);
}

public class UserApiService : IUserApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UserApiService(IHttpClientFactory httpClientFactory) =>
        _httpClientFactory = httpClientFactory;

    private HttpClient CreateClient() =>
        _httpClientFactory.CreateClient("UserApi");

    public bool UserExists(string userId)
    {
        var client = CreateClient();
        var response = client.GetAsync($"/user/{userId}").Result;

        if (response.IsSuccessStatusCode)
            return true;
        
        if (response.StatusCode == HttpStatusCode.NotFound)
            return false;

        throw new AppException("Error while checking if user exists", (int)response.StatusCode);
    }
}