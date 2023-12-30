using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stride.Domain.Common;
using Stride.Domain.Entities;
using Stride.Infrastructure.Identity;

namespace Stride.Infrastructure.Data.Configurations;

public static class DefaultManageableEntityConfiguration
{
    public static void ConfigureManageable<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseManageableEntity
    {
        builder
            .HasOne(e => e.CreatedBy)
            .WithMany()
            .HasForeignKey("CreatedByUserId")
            .IsRequired();

        builder
            .HasOne(e => e.UpdatedBy)
            .WithMany()
            .HasForeignKey("UpdatedByUserId");
    }
}
