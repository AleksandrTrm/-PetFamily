using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Shared.Core.DTOs;
using PetFamily.Shared.Core.DTOs.Pets;

namespace PetFamily.VolunteersManagement.Infrastructure.Configurations.Read;

public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.ToTable("pets");
        
        builder.HasKey(p => p.Id)
            .HasName("id");

        builder.Property(p => p.Nickname)
            .HasColumnName("nickname_value");

        builder.ComplexProperty(v => v.SpeciesBreedDto, bb =>
        {
            bb.Property(b => b.SpeciesId)
                .HasColumnName("species_breed_species_id");
            
            bb.Property(b => b.BreedId)
                .HasColumnName("species_breed_breed_id");
        });

        builder.Property(p => p.Description)
            .HasColumnName("description_value");

        builder.Property(p => p.SerialNumber)
            .HasColumnName("serial_number_value");

        builder.Property(p => p.Color)
            .HasColumnName("color_value");
        
        builder.Property(p => p.HealthInfo)
            .HasColumnName("health_info_value");
        
        builder.ComplexProperty(p => p.Address, ab =>
        {
            ab.Property(a => a.Settlement)
                .HasColumnName("address_settlement");
            
            ab.Property(a => a.District)
                .HasColumnName("address_district");
            
            ab.Property(a => a.Street)
                .HasColumnName("address_street");
            
            ab.Property(a => a.House)
                .HasColumnName("address_house");
        });
        
        builder.Property(p => p.Weight)
            .HasColumnName("weight");
        
        builder.Property(p => p.Height)
            .HasColumnName("height");

        builder.Property(p => p.OwnerPhone)
            .HasColumnName("owner_phone_value");

        builder.Property(p => p.IsCastrated)
            .HasColumnName("is_castrated");

        builder.Property(p => p.DateOfBirth)
            .HasColumnName("date_of_birth");
        
        builder.Property(p => p.IsVaccinated)
            .HasColumnName("is_vaccinated");
        
        builder.Property(p => p.Status)
            .HasColumnName("status");
        
        builder.Property(v => v.Requisites)
            .HasConversion(
                sm => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer
                    .Deserialize<IEnumerable<RequisiteDto>>(json, JsonSerializerOptions.Default)!)
            .HasColumnName("requisites");
        
        builder.Property(v => v.PetPhotos)
            .HasConversion(
                sm => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer
                    .Deserialize<IEnumerable<PetPhotoDto>>(json, JsonSerializerOptions.Default)!)
            .HasColumnName("pet_photos");
        
        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at");
    }
}