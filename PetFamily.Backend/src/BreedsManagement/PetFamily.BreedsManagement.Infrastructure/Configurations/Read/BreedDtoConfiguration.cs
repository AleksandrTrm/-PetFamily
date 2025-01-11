using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Shared.SharedKernel.DTOs;

namespace PetFamily.BreedsManagement.Infrastructure.Configurations.Read;

public class BreedDtoConfiguration : IEntityTypeConfiguration<BreedDto>
{
    public void Configure(EntityTypeBuilder<BreedDto> builder)
    {
        builder.ToTable("breeds");
        
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .HasColumnName("value");

        builder.Property(b => b.SpeciesId)
            .HasColumnName("species_id");
    }
}