using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stride.Domain.Entities;
using Stride.Infrastucture.Identity;

namespace Stride.Infrastucture.Data.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasOne<ApplicationUser>()
            .WithOne()
            .HasForeignKey<ApplicationUser>(u => u.Id);
    }
}
