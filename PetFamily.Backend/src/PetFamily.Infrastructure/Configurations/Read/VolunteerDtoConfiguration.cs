using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.DTOs.VolunteerDtos;

namespace PetFamily.Infrastructure.Configurations.Read;

public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.Property(v => v.FullName);

        builder.Property(v => v.Description);

        builder.Property(v => v.Experience);
        
        builder.Property(v => v.PhoneNumber);

        builder.Property(v => v.SocialMedias)
            .HasConversion(
                sm => JsonSerializer.Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer
                    .Deserialize<SocialMediaDto[]>(json, JsonSerializerOptions.Default)!);
    }
}