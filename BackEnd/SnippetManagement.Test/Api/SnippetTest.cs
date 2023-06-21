using System.Diagnostics;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SnippetManagement.Common;
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
        var snippet = new CreateSnippetRequest("test", "test", "test", "Chrome", "C#")
        {
            Tags = new[]
            {
                new CreateTagRequest("test")
            }
        };
        
        await _httpRequestMessageHelper.PostAsync(_client, _token, "https://localhost:44395/Snippet",
            JsonSerializer.Serialize(snippet));
        var result = await GetContext().Set<Snippet>().Include(x => x.Tags)
            .SingleAsync(x => x.Name == snippet.Name);
        
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
            .FirstAsync(x => x.Id == new Guid($"07785b4a-04e6-4435-b156-63fce124b314"));
        var snippet = JsonSerializer.Deserialize<SnippetDto>(await response.Content.ReadAsStringAsync(),
            new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
        snippet?.Name.Should().Be(result.Name);
        snippet?.Content.Should().Be(result.Content);
        snippet?.Description.Should().Be(result.Description);
        snippet?.Origin.Should().Be(result.Origin);
        result.Tags.ToList()[0].TagId.Should().NotBeEmpty().Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateSnippet_ShouldBeSuccessful()
    {
        //tạo cai object Snippet
        var snippetUpdate = new UpdateSnippetRequest("test", "test", "test", "Chrome", "C#")
        {
            Id = new Guid($"07785b4a-04e6-4435-b156-63fce124b314"),
            Tags = new List<CreateTagRequest>()
            {
                new(" new test"),
            }
        };

        //goi toi Api
        await _httpRequestMessageHelper.PutAsync(_client, _token, $"https://localhost:44395/Snippet/{snippetUpdate.Id}",
            JsonSerializer.Serialize(snippetUpdate));

        var result = await GetContext().Set<Snippet>().Include(x => x.Tags)
            .FirstAsync(x => x.Id == new Guid($"07785b4a-04e6-4435-b156-63fce124b314"));

        //Expected result
        snippetUpdate.Name.Should().Be(result.Name);
        snippetUpdate.Content.Should().Be(result.Content);
        snippetUpdate.Description.Should().Be(result.Description);
        snippetUpdate.Origin.Should().Be(result.Origin);
        result?.Tags?.ToList()[0].TagId.Should().NotBeEmpty().Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteSnippet_ShouldBeSuccessful()
    {
        //goi toi Api
        await _httpRequestMessageHelper.DeleteAsync(_client, _token,
            $"https://localhost:44395/Snippet?id=07785b4a-04e6-4435-b156-63fce124b314",
            null);
        var result = await GetContext().Set<Snippet>()
            .FirstAsync(x => x.Id == new Guid($"07785b4a-04e6-4435-b156-63fce124b314"));
        result.Deleted.Should().Be(true);
    }

    [Fact]
    public async Task GetSnippetByPage_ShouldBeSuccessful()
    {
        var responseMessage = await _httpRequestMessageHelper.GetAsync(_client, _token,
            "https://localhost:44395/Snippet?startIndex=0&endIndex=7&property=created&orderWay=0",
            null);
        var content = await responseMessage.Content.ReadAsStringAsync();
        var pagedResponse = JsonSerializer.Deserialize<PagedResponse<IEnumerable<SnippetDto>>>(content,
            new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

        var result = await GetContext().Set<Snippet>().Include(x => x.Tags).Skip(1).Take(1).FirstAsync();

        pagedResponse?.Data?.Count().Should().BeGreaterThan(0);
        Debug.Assert(result != null, nameof(result) + " != null");
        Debug.Assert(result.Tags != null, "result.Tags != null");
        pagedResponse?.Data?.ToList()[0].Tags.ToList()[0].Id.Should().Be(result.Tags.ToList()[0].TagId);
        pagedResponse?.TotalRecords.Should().Be(2);
    }

    [Fact]
    public async Task SearchSnippet_ShouldBeSuccessful()
    {
        var responseMessage = await _httpRequestMessageHelper.GetAsync(_client, _token,
            "https://localhost:44395/Snippet/search?TagIds=07785b4a-04e6-4435-b156-63fce124b315&KeyWord=testA&PageNumber=1&PageSize=10",
            null);
        var content = await responseMessage.Content.ReadAsStringAsync();
        var pagedResponse = JsonSerializer.Deserialize<PagedResponse<IEnumerable<SnippetDto>>>(content,
            new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
        pagedResponse?.Data?.Count().Should().BeGreaterThan(0);
        pagedResponse?.Data?.ToList()[0].Tags.ToList()[0].Id.Should().Be("07785b4a-04e6-4435-b156-63fce124b315");
        pagedResponse?.TotalRecords.Should().Be(1);
    }

    private string GetToken()
    {
        var jsonCredentials = JsonSerializer.Serialize(new UserDto("a@a.vn")
        {
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

        var userId = Guid.NewGuid();

        context.Set<User>().Add(new User("a@a.vn")
        {
            Id = userId,
            Password = BCrypt.Net.BCrypt.HashPassword("a")
        });

        context.Set<Snippet>().AddRange(new Snippet(new Guid("07785b4a-04e6-4435-b156-63fce124b313"), "testA", "testA",
            "testA", "testA", "C#", userId)
        {
            Created = DateTimeOffset.UtcNow,
            Deleted = false,
        }, new Snippet(new Guid("07785b4a-04e6-4435-b156-63fce124b314"), "testB", "testB", "testB", "testB", "C#",
            userId)
        {
            Created = DateTimeOffset.UtcNow,
            Deleted = false,
        });

        context.Set<Tag>().AddRange(new Tag(new Guid("07785b4a-04e6-4435-b156-63fce124b315"), "tagA")
            {
                Created = DateTimeOffset.UtcNow,
                Deleted = false,
            },
            new Tag(new Guid("07785b4a-04e6-4435-b156-63fce124b316"), "tagB")
            {
                Created = DateTimeOffset.UtcNow,
                Deleted = false,
            });

        context.Set<SnippetTag>().AddRange(new SnippetTag()
            {
                SnippetId = new Guid("07785b4a-04e6-4435-b156-63fce124b313"),
                TagId = new Guid("07785b4a-04e6-4435-b156-63fce124b315"),
            },
            new SnippetTag()
            {
                SnippetId = new Guid("07785b4a-04e6-4435-b156-63fce124b314"),
                TagId = new Guid("07785b4a-04e6-4435-b156-63fce124b316"),
            });

        context.SaveChanges();
    }

    public SnippetManagementDbContext GetContext()
    {
        var scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<SnippetManagementDbContext>();
    }
}