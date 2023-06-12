using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SnippetManagement.Data;

namespace SnippetManagement.Test;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<SnippetManagementDbContext>));

            if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(SnippetManagementDbContext));

            if (dbConnectionDescriptor != null) services.Remove(dbConnectionDescriptor);

            // var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            // services.AddDbContext<SnippetManagementDbContext>(options =>
            //     options.UseSqlServer(config.GetConnectionString("XUnitDbTestConnection")));
            
            // Add a database context (ApplicationDbContext) using an in-memory 
            // database for testing.
            services.AddDbContext<SnippetManagementDbContext>(options => 
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });
            
        });

        builder.UseEnvironment("Development");
    }
}