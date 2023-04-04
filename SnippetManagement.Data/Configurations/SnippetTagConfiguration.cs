using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnippetManagement.DataModel;

namespace SnippetManagement.Data.Configurations;

public class SnippetTagConfiguration : IEntityTypeConfiguration<SnippetTag>
{
    public void Configure(EntityTypeBuilder<SnippetTag> builder)
    {
        builder.HasKey(x => new { x.SnippetId, x.TagId });
        builder.HasOne<Tag>(st => st.Tag).WithMany(t => t.Snippets).HasForeignKey(st => st.TagId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Snippet>(st => st.Snippet).WithMany(t => t.Tags).HasForeignKey(st => st.SnippetId).OnDelete(DeleteBehavior.Restrict);
        builder.Ignore(x => x.Id).Ignore(x => x.Created).Ignore(x => x.Modified);
    }
}