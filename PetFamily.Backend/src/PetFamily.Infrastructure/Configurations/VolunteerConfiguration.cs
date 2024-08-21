using PetFamily.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using PetFamily.Domain.Entities.Volunteers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.HasKey(v => v.Id);

        builder.OwnsOne(v => v.FullName, nb =>
        {
            nb.ToJson();

            nb.Property(n => n.FirstName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            
            nb.Property(n => n.LastName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MIDDLE_TEXT_LENGTH);
            
            nb.Property(n => n.Patronymic)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MIDDLE_TEXT_LENGTH);
        });
            

        builder.OwnsOne(p => p.Description, db =>
        {
            db.ToJson();

            db.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        });

        builder.Property(v => v.Experience)
            .IsRequired();

        builder.Property(v => v.CountOfPetsThatFoundHome)
            .IsRequired();
        
        builder.Property(v => v.CountOfPetsThatLookingForHome)
            .IsRequired();
        
        builder.Property(v => v.CountOfPetsThatGetTreatment)
            .IsRequired();
        
        builder.OwnsOne(p => p.PhoneNumber, pb =>
        {
            pb.ToJson();

            pb.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_PHONE_NUMBER_LENGTH);
        });

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

                vb.OwnsOne(r => r.Description, db =>
                {
                    db.Property(d => d.Value)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
                });
            });
        });

        builder.HasMany(v => v.Pets)
            .WithOne(p => p.Volunteer)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}