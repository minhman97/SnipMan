using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnippetManagement.DataModel;

namespace SnippetManagement.Data.Configurations;

public class SnippetConfiguration : IEntityTypeConfiguration<Snippet>
{
    public void Configure(EntityTypeBuilder<Snippet> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(255);
        builder.Property(x => x.Content);
        builder.Property(x => x.Description);
        builder.Property(x => x.Origin).HasMaxLength(255);
        builder.Property(x => x.Created).HasDefaultValueSql("getutcdate()");
        builder.Property(x => x.Modified);
    }
}