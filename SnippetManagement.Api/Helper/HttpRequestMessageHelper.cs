using System.Net.Http.Headers;
using System.Text;

namespace SnippetManagement.Api.Helper;

public interface IHttpRequestMessageHelper
{
    Task<HttpResponseMessage> PostAsync(HttpClient client, string? token, string uri, string data);
    Task<HttpResponseMessage> PutAsync(HttpClient client, string? token, string uri, string data);
    Task<HttpResponseMessage> GetAsync(HttpClient client, string? token, string uri, string data);
    Task<HttpResponseMessage> DeleteAsync(HttpClient client, string? token, string uri, string data);
}
public class HttpRequestMessageHelper: IHttpRequestMessageHelper
{
    private HttpRequestMessage InitRequestMessage(string? token, string uri, HttpMethod method, string? data)
    {
        return new HttpRequestMessage()
        {
            Headers = { Authorization =  string.IsNullOrEmpty(token) ? null : new AuthenticationHeaderValue("Bearer", token) },
            RequestUri = new Uri(uri),
            Method = method,
            Content = string.IsNullOrEmpty(data) ? null : new StringContent(data, Encoding.UTF8, "application/json")
        };
    }

    public async Task<HttpResponseMessage> PostAsync(HttpClient client, string? token, string uri, string data)
    {
        return await client.SendAsync(InitRequestMessage(token, uri, HttpMethod.Post, data));
    }

    public async Task<HttpResponseMessage> PutAsync(HttpClient client, string? token, string uri, string data)
    {
        return await client.SendAsync(InitRequestMessage(token, uri, HttpMethod.Put, data));
    }
    
    public async Task<HttpResponseMessage> GetAsync(HttpClient client, string? token, string uri, string data)
    {
        return await client.SendAsync(InitRequestMessage(token, uri, HttpMethod.Get, data));
    }
    
    public async Task<HttpResponseMessage> DeleteAsync(HttpClient client, string? token, string uri, string data)
    {
        return await client.SendAsync(InitRequestMessage(token, uri, HttpMethod.Delete, data));
    }
}