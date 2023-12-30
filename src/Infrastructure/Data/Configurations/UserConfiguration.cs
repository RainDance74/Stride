using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stride.Domain.Entities;
using Stride.Infrastructure.Identity;

namespace Stride.Infrastructure.Data.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<ApplicationUser>(u => u.Id);
    }
}
