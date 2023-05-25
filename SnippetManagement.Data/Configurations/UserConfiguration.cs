using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SnippetManagement.DataModel;

namespace SnippetManagement.Data.Configurations;

public class UserConfiguration: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Email);
        builder.Property(x => x.Password);
        builder.Property(x => x.Created).HasDefaultValueSql("getutcdate()");
        builder.Property(x => x.Modified);
        builder.Property(x => x.Deleted);
        builder.Property(x => x.SocialProvider);
    }
}