using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;

namespace SnippetManagement.Test.API;

public class UnitTest1: IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program>
        _factory;

    public UnitTest1(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task CreateSnippet_ShouldBeSuccessful()
    {
        using var scope = _factory.Services.CreateScope();

        var snippet = new CreateSnippetRequest
        {
            Name = "test",
            Content = "test",
            Description = "test",
            Origin = "Chrome",
            Tags = new[]
            {
                new CreateTagRequest()
                {
                    TagName = "test"
                }
            }
        };
        
        //tạo cai oebject Snippet
        var jsonSnippet = JsonSerializer.Serialize(snippet);
        var result1 = JsonSerializer.Deserialize<CreateSnippetRequest>(jsonSnippet);

        
        //goi toi Api
        var jsonCredentials = JsonSerializer.Serialize(new UserCredentials()
        {
            Email = "a@a.vn",
            Password = "a"
        });
        
        var response = await _client.PostAsync("https://localhost:44395/Authentication", new StringContent(jsonCredentials, Encoding.UTF8, "application/json"));
        var token = await response.Content.ReadAsStringAsync();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);       
        var responseSnippet = await _client.PostAsync("https://localhost:44395/Snippet",
            new StringContent(jsonSnippet, Encoding.UTF8, "application/json"));
        
        var jsonResult = await responseSnippet.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<SnippetDto>(jsonResult, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });
        //Expected result
        Assert.Equal(HttpStatusCode.OK, responseSnippet.StatusCode);
        Assert.Equal(snippet.Name, result?.Name);
        Assert.Equal(snippet.Content, result?.Content);
        Assert.Equal(snippet.Description, result?.Description);
        Assert.Equal(snippet.Origin, result?.Origin);
        Assert.True(result?.Tags.ToList()[0].Id != null && result.Tags.ToList()[0].Id != Guid.Empty);
    }

    [Fact]
    public void FailingTest()
    {
        Assert.Equal(5, Add(2, 2));
    }

    int Add(int x, int y)
    {
        return x + y;
    }
}