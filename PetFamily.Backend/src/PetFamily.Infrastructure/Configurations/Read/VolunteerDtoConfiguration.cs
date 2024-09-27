using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PetFamily.Application.DTOs.VolunteerDtos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
                    .Deserialize<IEnumerable<SocialMediaDto>>(json, JsonSerializerOptions.Default)!);
    }
}