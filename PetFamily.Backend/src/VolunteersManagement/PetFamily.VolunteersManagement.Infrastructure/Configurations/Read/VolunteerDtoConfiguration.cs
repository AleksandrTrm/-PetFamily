using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Shared.Core.DTOs;
using PetFamily.Shared.Core.DTOs.VolunteerDtos;

namespace PetFamily.VolunteersManagement.Infrastructure.Configurations.Read;

public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(v => v.Id)
            .HasName("id");
        
        builder.Property(n => n.Name)
            .IsRequired()
            .HasMaxLength(Shared.SharedKernel.Constants.MAX_LOW_TEXT_LENGTH)
            .HasColumnName("name");

        builder.Property(n => n.Surname)
            .IsRequired()
            .HasMaxLength(Shared.SharedKernel.Constants.MAX_MIDDLE_TEXT_LENGTH)
            .HasColumnName("surname");

        builder.Property(n => n.Patronymic)
            .IsRequired()
            .HasMaxLength(Shared.SharedKernel.Constants.MAX_MIDDLE_TEXT_LENGTH)
            .HasColumnName("patronymic");

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
                    .Deserialize<IEnumerable<SocialNetworkDto>>(json, JsonSerializerOptions.Default)!)
            .HasColumnName("social_networks");

        builder.Property(v => v.Requisites)
            .HasConversion(
                r => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<List<RequisiteDto>>(json, JsonSerializerOptions.Default)!);
        
        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey(p => p.VolunteerId);
    }
}