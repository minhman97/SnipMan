using Microsoft.EntityFrameworkCore;

namespace SnippetManagement.Data;

public class SnippetManagementDbContext: DbContext
{
    public SnippetManagementDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
       
    }
}