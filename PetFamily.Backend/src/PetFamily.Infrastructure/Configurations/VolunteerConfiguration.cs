using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Entities;
using PetFamily.Domain.Entities.Volunteers;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.FullName)
            .IsRequired();

        builder.Property(v => v.Description)
            .IsRequired();

        builder.Property(v => v.Experience)
            .IsRequired();

        builder.Property(v => v.CountOfPetsThatFoundHome)
            .IsRequired();
        
        builder.Property(v => v.CountOfPetsThatLookingForHome)
            .IsRequired();
        
        builder.Property(v => v.CountOfPetsThatGetTreatment)
            .IsRequired();
        
        builder.Property(v => v.PhoneNumber)
            .IsRequired();

        builder.OwnsOne(v => v.SocialMedias, sb =>
        {
            sb.ToJson();

            sb.OwnsMany(s => s.Value, m =>
            {
                m.Property(sm => sm.Title)
                    .IsRequired();

                m.Property(sm => sm.Link)
                    .IsRequired();
            });
        });

        builder.OwnsOne(v => v.Requisites, rb =>
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

        builder.HasMany(v => v.Pets)
            .WithOne()
            .IsRequired();
    }
}