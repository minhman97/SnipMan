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
    private Guid _snippetBId;
    private Guid _tagAId;

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
            NewTags = new[]
            {
                new CreateTagRequest("test")
            },
            TagsExisted = new List<ExistedTagRequest>()
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
            $"https://localhost:44395/Snippet/{_snippetBId}",
            null);
        var result = await GetContext().Set<Snippet>().Include(x => x.Tags)
            .FirstAsync(x => x.Id == _snippetBId);
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
        var snippetUpdate = new UpdateSnippetRequest(_snippetBId,"test", "test", "test", "Chrome", "C#")
        {
            NewTags = new List<CreateTagRequest>()
            {
                new(" new test"),
            },
            TagsExisted = new List<ExistedTagRequest>()
        };

        await _httpRequestMessageHelper.PutAsync(_client, _token, $"https://localhost:44395/Snippet/{snippetUpdate.Id}",
            JsonSerializer.Serialize(snippetUpdate));

        var result = await GetContext().Set<Snippet>().Include(x => x.Tags)
            .FirstAsync(x => x.Id == _snippetBId);

        snippetUpdate.Name.Should().Be(result.Name);
        snippetUpdate.Content.Should().Be(result.Content);
        snippetUpdate.Description.Should().Be(result.Description);
        snippetUpdate.Origin.Should().Be(result.Origin);
        result?.Tags?.ToList()[0].TagId.Should().NotBeEmpty().Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteSnippet_ShouldBeSuccessful()
    {
        await _httpRequestMessageHelper.DeleteAsync(_client, _token,
            $"https://localhost:44395/Snippet?id={_snippetBId}",
            null);
        var result = await GetContext().Set<Snippet>()
            .FirstAsync(x => x.Id == _snippetBId);
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
            $"https://localhost:44395/Snippet/search?TagIds={_tagAId}&KeyWord=testA&PageNumber=1&PageSize=10",
            null);
        var content = await responseMessage.Content.ReadAsStringAsync();
        var pagedResponse = JsonSerializer.Deserialize<PagedResponse<IEnumerable<SnippetDto>>>(content,
            new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
        pagedResponse?.Data?.Count().Should().BeGreaterThan(0);
        pagedResponse?.Data?.ToList()[0].Tags.ToList()[0].Id.Should().Be(_tagAId);
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
        var snippetAId = Guid.NewGuid();
        var tagBId = Guid.NewGuid();
        _snippetBId = Guid.NewGuid();
        _tagAId = Guid.NewGuid();

        context.Set<User>().Add(new User("a@a.vn")
        {
            Id = userId,
            Password = BCrypt.Net.BCrypt.HashPassword("a")
        });

        context.Set<Snippet>().AddRange(new Snippet(snippetAId, "testA", "testA",
            "testA", "testA", "C#", userId)
        {
            Created = DateTimeOffset.UtcNow,
            Deleted = false,
        }, new Snippet(_snippetBId, "testB", "testB", "testB", "testB", "C#",
            userId)
        {
            Created = DateTimeOffset.UtcNow,
            Deleted = false,
        });

        context.Set<Tag>().AddRange(new Tag(_tagAId, "tagA")
            {
                Created = DateTimeOffset.UtcNow,
                Deleted = false,
            },
            new Tag(tagBId, "tagB")
            {
                Created = DateTimeOffset.UtcNow,
                Deleted = false,
            });

        context.Set<SnippetTag>().AddRange(new SnippetTag()
            {
                SnippetId = snippetAId,
                TagId = _tagAId,
            },
            new SnippetTag()
            {
                SnippetId = _snippetBId,
                TagId = tagBId,
            });

        context.SaveChanges();
    }

    public SnippetManagementDbContext GetContext()
    {
        var scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<SnippetManagementDbContext>();
    }
}