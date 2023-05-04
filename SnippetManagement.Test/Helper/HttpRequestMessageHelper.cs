using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using SnippetManagement.Service.Model;

namespace SnippetManagement.Test.Helper;

public interface IHttpRequestMessageHelper
{
    Task<HttpResponseMessage> PostAsync(HttpClient client, string? token, string uri, string data);
    Task<HttpResponseMessage> PutAsync(HttpClient client, string? token, string uri, string data);
    Task<HttpResponseMessage> GetAsync(HttpClient client, string? token, string uri, string? data);
    Task<HttpResponseMessage> DeleteAsync(HttpClient client, string? token, string uri, string data);
}

public class HttpRequestMessageHelper: IHttpRequestMessageHelper
{
    private readonly IHttpRequestMessageHelper _httpRequestMessageHelper;

    public HttpRequestMessageHelper(IHttpRequestMessageHelper httpRequestMessageHelper)
    {
        _httpRequestMessageHelper = httpRequestMessageHelper;
    }

    public HttpRequestMessage InitRequestMessage(string? jsonToken, string uri, HttpMethod method, string? data)
    {
        var token = JsonConvert.DeserializeObject<TokenDto>(jsonToken);
        return new HttpRequestMessage()
        {
            Headers = { Authorization =  string.IsNullOrEmpty(token.Token) ? null : new AuthenticationHeaderValue("Bearer", token.Token) },
            RequestUri = new Uri(uri),
            Method = method,
            Content = string.IsNullOrEmpty(data) ? null : new StringContent(data, Encoding.UTF8, "application/json")
        };
    }

    public async Task<HttpResponseMessage> PostAsync(HttpClient client, string? token, string uri, string? data)
    {
        return await client.SendAsync(InitRequestMessage(token, uri, HttpMethod.Post, data));
    }

    public async Task<HttpResponseMessage> PutAsync(HttpClient client, string? token, string uri, string? data)
    {
        return await client.SendAsync(InitRequestMessage(token, uri, HttpMethod.Put, data));
    }
    
    public async Task<HttpResponseMessage> GetAsync(HttpClient client, string? token, string uri, string? data)
    {
        return await client.SendAsync(InitRequestMessage(token, uri, HttpMethod.Get, data));
    }
    
    public async Task<HttpResponseMessage> DeleteAsync(HttpClient client, string? token, string uri, string? data)
    {
        return await client.SendAsync(InitRequestMessage(token, uri, HttpMethod.Delete, data));
    }
}