﻿using PetFamily.Shared.SharedKernel;
using Microsoft.EntityFrameworkCore;
using PetFamily.Shared.SharedKernel.IDs;
using PetFamily.BreedsManagement.Domain.Entitites;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFamily.BreedsManagement.Infrastructure.Configurations.Write;

public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("breeds");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasConversion(
                b => b.Value,
                v => BreedId.Create(v))
            .IsRequired();

        builder.Property(b => b.Name)
            .HasMaxLength(Constants.MAX_MIDDLE_HIGH_LENGTH)
            .IsRequired()
            .HasColumnName("value");
    }
}