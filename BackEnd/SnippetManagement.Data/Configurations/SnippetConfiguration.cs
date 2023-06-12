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
        builder.Property(x => x.Deleted);
        builder.Property(x => x.Language);
        builder.Property(x => x.UserId).HasDefaultValue(new Guid("3b094ed2-5eb3-4b6e-150c-08db05b7cf56"));

        builder.HasOne<User>(x => x.User).WithMany(x => x.Snippets).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);

    }
}