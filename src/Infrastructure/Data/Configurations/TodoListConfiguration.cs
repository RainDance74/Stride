﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Stride.Domain.Entities;
using Stride.Infrastructure.Identity;

namespace Stride.Infrastructure.Data.Configurations;

public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
{
    public void Configure(EntityTypeBuilder<TodoList> builder)
    {
        builder.Property(i => i.Title)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(e => e.Owner)
            .IsRequired();

        builder.ConfigureManageable();
    }
}
