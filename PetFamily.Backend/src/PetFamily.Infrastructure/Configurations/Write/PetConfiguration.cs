﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.VolunteersManagement.Entities.Pets;
using PetFamily.Domain.VolunteersManagement.Entities.Pets.Enums;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Shared;

namespace PetFamily.Infrastructure.Configurations.Write;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                p => p.Value,
                v => PetId.Create(v))
            .IsRequired();
        
        builder.ComplexProperty(p => p.Nickname, nb =>
        {
            nb.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MIDDLE_TEXT_LENGTH);
        });

        builder.ComplexProperty(p => p.SpeciesBreed, sbb =>
        {
            sbb.Property(s => s.SpeciesId)
                .HasConversion(
                    s => s.Value,
                    v => SpeciesId.Create(v))
                .IsRequired();
            
            sbb.Property(b => b.BreedId)
                .IsRequired();
        });

        builder.ComplexProperty(p => p.Description, db =>
        {
            db.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        });
        
        builder.ComplexProperty(p => p.SerialNumber, db =>
        {
            db.Property(t => t.Value)
                .IsRequired();
        });

        builder.ComplexProperty(p => p.Color, db =>
        {
            db.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });

        builder.ComplexProperty(p => p.HealthInfo, db =>
        {
            db.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MIDDLE_HIGH_LENGTH);
        });

        builder.ComplexProperty(p => p.Address, ab =>
        {
            ab.Property(t => t.District)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MIDDLE_TEXT_LENGTH);
            
            ab.Property(t => t.Settlement)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MIDDLE_TEXT_LENGTH);
            
            ab.Property(t => t.Street)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MIDDLE_TEXT_LENGTH);
            
            ab.Property(t => t.House)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MIDDLE_TEXT_LENGTH);
        });

        builder.Property(p => p.Weight)
            .IsRequired();
        
        builder.Property(p => p.Height)
            .IsRequired();
        
        builder.ComplexProperty(p => p.OwnerPhone, pb =>
        {
            pb.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(PhoneNumber.MAX_PHONE_NUMBER_LENGTH);
        });
        
        builder.Property(p => p.IsCastrated)
            .IsRequired();
        
        builder.Property(p => p.DateOfBirth)
            .IsRequired();
        
        builder.Property(p => p.IsVaccinated)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasConversion<string>(
                s => s.ToString(),
                s => (Status)Enum.Parse(typeof(Status), s));

        builder.OwnsOne(p => p.Requisites, rb =>
        {
            rb.ToJson();

            rb.OwnsMany(r => r.Values, vb =>
            {
                vb.Property(r => r.Title)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

                vb.OwnsOne(r => r.Description, db =>
                {
                    db.Property(d => d.Value)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
                });
            });
        });

        builder.Property(p => p.CreatedAt);
        
        builder.OwnsOne(p => p.PetPhotos, ppb =>
        {
            ppb.ToJson();

            ppb.OwnsMany(pp => pp.Values, pb =>
            {
                pb.Property(p => p.IsMain)
                    .IsRequired();

                pb.Property(p => p.Path)
                    .IsRequired();
            });
        });
        
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}