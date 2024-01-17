using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Stride.Domain.Entities;
using Stride.Infrastructure.Identity;

namespace Stride.Infrastructure.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasOne(u => u.StrideUser)
            .WithOne()
            .HasForeignKey<StrideUser>(u => u.Id);
    }
}

