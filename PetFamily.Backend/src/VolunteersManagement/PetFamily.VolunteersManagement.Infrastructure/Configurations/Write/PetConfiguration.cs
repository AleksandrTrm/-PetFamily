using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel.DTOs;
using PetFamily.Shared.SharedKernel.DTOs.Pets;
using PetFamily.Shared.SharedKernel.IDs;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Pets;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;
using PetFamily.VolunteersManagement.Domain.Entities.Pets;
using PetFamily.VolunteersManagement.Domain.Entities.Pets.Enums;

namespace PetFamily.VolunteersManagement.Infrastructure.Configurations.Write;

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
                .HasMaxLength(Shared.SharedKernel.Constants.MAX_MIDDLE_TEXT_LENGTH);
        });

        builder.ComplexProperty(v => v.SpeciesBreed, bb =>
        {
            bb.Property(s => s.SpeciesId)
                .HasConversion(
                    p => p.Value,
                    value => SpeciesId.Create(value))
                .IsRequired();
            
            bb.Property(b => b.BreedId);
        });
        
        builder.ComplexProperty(p => p.Description, db =>
        {
            db.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(Shared.SharedKernel.Constants.MAX_HIGH_TEXT_LENGTH);
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
                .HasMaxLength(Shared.SharedKernel.Constants.MAX_LOW_TEXT_LENGTH);
        });

        builder.ComplexProperty(p => p.HealthInfo, db =>
        {
            db.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(Shared.SharedKernel.Constants.MAX_MIDDLE_HIGH_LENGTH);
        });

        builder.ComplexProperty(p => p.Address, ab =>
        {
            ab.Property(t => t.District)
                .IsRequired()
                .HasMaxLength(Shared.SharedKernel.Constants.MAX_MIDDLE_TEXT_LENGTH);
            
            ab.Property(t => t.Settlement)
                .IsRequired()
                .HasMaxLength(Shared.SharedKernel.Constants.MAX_MIDDLE_TEXT_LENGTH);
            
            ab.Property(t => t.Street)
                .IsRequired()
                .HasMaxLength(Shared.SharedKernel.Constants.MAX_MIDDLE_TEXT_LENGTH);
            
            ab.Property(t => t.House)
                .IsRequired()
                .HasMaxLength(Shared.SharedKernel.Constants.MAX_MIDDLE_TEXT_LENGTH);
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

        builder.Property(p => p.Requisites)
            .HasValueObjectsJsonConversion(
                value => new RequisiteDto(value.Title, value.Description),
                dto => Requisite.Create(dto.Title, dto.Description).Value);

        builder.Property(p => p.PetPhotos)
            .HasValueObjectsJsonConversion(
                value => new PetPhotoDto(value.Path, value.IsMain),
                dto => PetPhoto.Create(dto.Path, dto.IsMain).Value);
        
        builder.Property(p => p.CreatedAt);
        
        builder.Property(p => p.IsDeleted)
            .IsRequired();
    }
}