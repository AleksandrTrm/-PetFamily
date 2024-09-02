using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.SpeciesManagement.Species;

namespace PetFamily.Infrastructure.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");
        
        builder.HasKey(s => s.Id);
        
        builder.Property(s => s.Id)
            .HasColumnName("id")
            .HasConversion(
                s => s.Value,
                v => SpeciesId.Create(v));
        
        builder.Property(s => s.Value)
            .HasMaxLength(Constants.MAX_MIDDLE_HIGH_LENGTH)
            .IsRequired();
        
        builder.HasMany(s => s.Breeds)
            .WithOne()
            .IsRequired();
    }
}