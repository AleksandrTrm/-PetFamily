﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.Entitites;

namespace PetFamily.Infrastructure.Configurations.Write;

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

        builder.ComplexProperty(b => b.Value, bb =>
        {
            bb.Property(b => b.Value)
                .HasMaxLength(Constants.MAX_MIDDLE_HIGH_LENGTH)
                .IsRequired()
                .HasColumnName("value");
        });
    }
}