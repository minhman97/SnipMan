using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SnippetManagement.Data;
using SnippetManagement.DataModel;
using SnippetManagement.Service.Model;
using SnippetManagement.Service.Requests;
using SnippetManagement.Test.Helper;

namespace SnippetManagement.Test.API;

public class SnippetTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IServiceProvider _serviceProvider;
    private readonly string _token;
    private readonly HttpRequestMessageHelper _httpRequestMessageHelper;
    private HttpRequestMessageHelper _a;

    public SnippetTest(CustomWebApplicationFactory<Program> factory)
    {
        _serviceProvider = factory.Services;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        Seed(GetContext());
        _token = GetToken();
        _httpRequestMessageHelper = new HttpRequestMessageHelper(new Mock<IHttpRequestMessageHelper>().Object);
    }

    [Fact]
    public async Task CreateSnippet_ShouldBeSuccessful()
    {
        //tạo cai object Snippet
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
        //goi toi Api
        await _httpRequestMessageHelper.PostAsync(_client, _token, "https://localhost:44395/Snippet",
            JsonSerializer.Serialize(snippet));
        var result = await GetContext().Set<Snippet>().Include(x => x.Tags)
            .SingleOrDefaultAsync(x => x.Name == snippet.Name);
        //Expected result
        snippet.Name.Should().Be(result.Name);
        snippet.Content.Should().Be(result.Content);
        snippet.Description.Should().Be(result.Description);
        snippet.Origin.Should().Be(result.Origin);
        result.Tags.ToList()[0].TagId.Should().NotBeEmpty().Should().NotBeNull();
    }

    [Fact]
    public async Task GetSnippet_ShouldBeSuccessful()
    {
        var response = await _httpRequestMessageHelper.GetAsync(_client, _token,
            "https://localhost:44395/Snippet/07785b4a-04e6-4435-b156-63fce124b314",
            null);
        var result = await GetContext().Set<Snippet>().Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == new Guid($"07785b4a-04e6-4435-b156-63fce124b314"));
        var snippet = JsonSerializer.Deserialize<SnippetDto>(await response.Content.ReadAsStringAsync(),
            new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
        snippet.Name.Should().Be(result.Name);
        snippet.Content.Should().Be(result.Content);
        snippet.Description.Should().Be(result.Description);
        snippet.Origin.Should().Be(result.Origin);
        result.Tags.ToList()[0].TagId.Should().NotBeEmpty().Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateSnippet_ShouldBeSuccessful()
    {
        //tạo cai object Snippet
        var snippetUpdate = new UpdateSnippetRequest()
        {
            Id = new Guid($"07785b4a-04e6-4435-b156-63fce124b314"),
            Name = "updated",
            Content = "updated",
            Description = "updated",
            Origin = "updated",
            Tags = new List<CreateTagRequest>()
            {
                new()
                {
                    TagName = " new test"
                },
            }
        };

        //goi toi Api
        await _httpRequestMessageHelper.PutAsync(_client, _token, $"https://localhost:44395/Snippet/{snippetUpdate.Id}",
            JsonSerializer.Serialize(snippetUpdate));

        var result = await GetContext().Set<Snippet>().Include(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Id == new Guid($"07785b4a-04e6-4435-b156-63fce124b314"));

        //Expected result
        snippetUpdate.Name.Should().Be(result.Name);
        snippetUpdate.Content.Should().Be(result.Content);
        snippetUpdate.Description.Should().Be(result.Description);
        snippetUpdate.Origin.Should().Be(result.Origin);
        result.Tags.ToList()[0].TagId.Should().NotBeEmpty().Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteSnippet_ShouldBeSuccessful()
    {
        //goi toi Api
        await _httpRequestMessageHelper.DeleteAsync(_client, _token,
            $"https://localhost:44395/Snippet?id=07785b4a-04e6-4435-b156-63fce124b314",
            null);
        var result = await GetContext().Set<Snippet>()
            .FirstOrDefaultAsync(x => x.Id == new Guid($"07785b4a-04e6-4435-b156-63fce124b314"));
        result?.Deleted.Should().Be(true);
    }

    [Fact]
    public async Task SearchSnippet_ShouldBeSuccessful()
    {
        var result = await _httpRequestMessageHelper.GetAsync(_client, _token,
            "https://localhost:44395/Snippet/search?TagIds=07785b4a-04e6-4435-b156-63fce124b315&KeyWord=testA",
            null);
        var content = await result.Content.ReadAsStringAsync();
        var snippets = JsonSerializer.Deserialize<List<SnippetDto>>(content, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });
        snippets?.Count.Should().BeGreaterThan(0);
        snippets?[0].Tags.ToList()[0].Id.Should().Be("07785b4a-04e6-4435-b156-63fce124b315");
    }

    private string GetToken()
    {
        var jsonCredentials = JsonSerializer.Serialize(new UserCredentials()
        {
            Email = "a@a.vn",
            Password = "a"
        });

        var response = _client.PostAsync("https://localhost:44395/Authentication",
            new StringContent(jsonCredentials, Encoding.UTF8, "application/json")).Result;
        return response.Content.ReadAsStringAsync().Result;
    }

    private void Seed(SnippetManagementDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Set<User>().Add(new User()
        {
            Id = Guid.NewGuid(),
            Email = "a@a.vn",
            Password = BCrypt.Net.BCrypt.HashPassword("a")
        });

        context.Set<Snippet>().Add(new Snippet()
        {
            Id = new Guid("07785b4a-04e6-4435-b156-63fce124b314"),
            Name = "testA",
            Content = "testA",
            Description = "testA",
            Origin = "TestA",
            Created = DateTimeOffset.UtcNow,
            Deleted = false,
        });

        context.Set<Tag>().Add(new Tag()
        {
            Id = new Guid("07785b4a-04e6-4435-b156-63fce124b315"),
            TagName = "tagA",
            Created = DateTimeOffset.UtcNow,
            Deleted = false,
        });

        context.Set<SnippetTag>().Add(new SnippetTag()
        {
            SnippetId = new Guid("07785b4a-04e6-4435-b156-63fce124b314"),
            TagId = new Guid("07785b4a-04e6-4435-b156-63fce124b315"),
        });

        context.SaveChanges();
    }

    public SnippetManagementDbContext GetContext()
    {
        var scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<SnippetManagementDbContext>();
    }
}