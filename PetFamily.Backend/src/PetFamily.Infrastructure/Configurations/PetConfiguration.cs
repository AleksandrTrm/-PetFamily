using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Entities.Volunteers;

namespace PetFamily.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nickname)
            .IsRequired();

        builder.Property(p => p.Type)
            .IsRequired();

        builder.Property(p => p.Description)
            .IsRequired();

        builder.Property(p => p.Breed)
            .IsRequired();

        builder.Property(p => p.Color)
            .IsRequired();

        builder.Property(p => p.HealthInfo)
            .IsRequired();

        builder.Property(p => p.Address)
            .IsRequired();

        builder.Property(p => p.Weight)
            .IsRequired();
        
        builder.Property(p => p.Height)
            .IsRequired();
        
        builder.Property(p => p.OwnerPhone)
            .IsRequired();
        
        builder.Property(p => p.IsCastrated)
            .IsRequired();
        
        builder.Property(p => p.DateOfBirth)
            .IsRequired();
        
        builder.Property(p => p.IsVaccinated)
            .IsRequired();

        builder.ComplexProperty(p => p.Status, sb =>
        {
            sb.IsRequired();

            sb.Property(s => s.Value)
                .HasConversion<string>()
                .IsRequired();
        });

        builder.OwnsOne(p => p.Requisites, rb =>
        {
            rb.ToJson();

            rb.OwnsMany(r => r.Value, vb =>
            {
                vb.Property(r => r.Title)
                    .IsRequired();

                vb.Property(r => r.Description)
                    .IsRequired();
            });
        });

        builder.Property(p => p.CreatedAt);
        
        builder.OwnsOne(p => p.PetPhotos, ppb =>
        {
            ppb.ToJson();

            ppb.OwnsMany(pp => pp.Value, pb =>
            {
                pb.Property(p => p.IsMain)
                    .IsRequired();

                pb.Property(p => p.Path)
                    .IsRequired();
            });
        });
    }
}