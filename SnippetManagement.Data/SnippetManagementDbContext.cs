using Microsoft.EntityFrameworkCore;
using SnippetManagement.Data.Configurations;

namespace SnippetManagement.Data;

public class SnippetManagementDbContext: DbContext
{
    public SnippetManagementDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new SnippetConfiguration());
        builder.ApplyConfiguration(new TagConfiguration());
        builder.ApplyConfiguration(new SnippetTagConfiguration());
        
    }
}