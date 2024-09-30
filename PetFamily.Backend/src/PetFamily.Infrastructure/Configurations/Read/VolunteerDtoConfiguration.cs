using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.DTOs.VolunteerDtos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.DTOs;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations.Read;

public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.ToTable("volunteers");
        
        builder.HasKey(v => v.Id)
            .HasName("id");
        
        builder.ComplexProperty(v => v.FullName, nb =>
        {
            nb.Property(n => n.Name)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name");

            nb.Property(n => n.Surname)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MIDDLE_TEXT_LENGTH)
                .HasColumnName("surname");

            nb.Property(n => n.Patronymic)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MIDDLE_TEXT_LENGTH)
                .HasColumnName("patronymic");
        });

        builder.Property(v => v.Description)
            .HasColumnName("description");

        builder.Property(v => v.Experience)
            .HasColumnName("experience");
        
        builder.Property(v => v.PhoneNumber)
            .HasColumnName("phone_number");

        builder.Property(v => v.SocialMedias)
            .HasConversion(
                sm => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer
                    .Deserialize<IEnumerable<SocialMediaDto>>(json, JsonSerializerOptions.Default)!)
            .HasColumnName("social_medias");
        
        builder.Property(v => v.Requisites)
            .HasConversion(
                sm => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer
                    .Deserialize<IEnumerable<RequisiteDto>>(json, JsonSerializerOptions.Default)!)
            .HasColumnName("requisites");

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey(p => p.VolunteerId);
    }
}