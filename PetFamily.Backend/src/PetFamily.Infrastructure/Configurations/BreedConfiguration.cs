﻿using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using Microsoft.EntityFrameworkCore;
using PetFamily.Domain.SpeciesManagement.Breeds;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFamily.Infrastructure.Configurations;

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

        builder.Property(s => s.Value)
            .HasMaxLength(Constants.MAX_MIDDLE_HIGH_LENGTH)
            .IsRequired();
    }
}