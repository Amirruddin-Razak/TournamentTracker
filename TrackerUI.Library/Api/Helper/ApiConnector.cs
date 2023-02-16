using System.Net.Http.Headers;

namespace TrackerUI.Library.Api.Helper;

public class ApiConnector : IApiConnector
{
    private HttpClient _apiClient;

    public ApiConnector(string apiUrl)
    {
        _apiClient = new HttpClient
        {
            BaseAddress = new Uri(apiUrl),
            Timeout = TimeSpan.FromSeconds(1000)
        };

        _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public HttpClient ApiClient => _apiClient;
}
