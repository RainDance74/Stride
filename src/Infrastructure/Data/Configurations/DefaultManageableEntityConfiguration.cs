using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Stride.Domain.Common;
using Stride.Infrastructure.Identity;

namespace Stride.Infrastructure.Data.Configurations;

public static class DefaultManageableEntityConfiguration
{
    public static void ConfigureManageable<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseManageableEntity
    {
        builder
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(e => e.CreatedBy)
            .IsRequired();

        builder
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(e => e.UpdatedBy);
    }
}
