﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.DTOs.Species;
using PetFamily.Domain.Shared.IDs;

namespace PetFamily.Infrastructure.Configurations.Read;

public class SpeciesDtoConfiguration : IEntityTypeConfiguration<SpeciesDto>
{
    public void Configure(EntityTypeBuilder<SpeciesDto> builder)
    {
        builder.ToTable("species");
        
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Value)
            .HasColumnName("value");

        builder.HasMany(s => s.Breeds)
            .WithOne()
            .HasForeignKey(b => b.SpeciesId)
            .HasConstraintName("species_id");
    }
}