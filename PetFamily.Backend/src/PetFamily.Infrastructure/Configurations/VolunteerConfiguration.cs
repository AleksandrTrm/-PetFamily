using PetFamily.Domain.Shared;
using PetFamily.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.VolunteersManagement.Volunteer;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");
        
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(
                v => v.Value,
                v => VolunteerId.Create(v))
            .IsRequired();
        
        builder.ComplexProperty(v => v.FullName, nb =>
        {
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
            

        builder.ComplexProperty(p => p.Description, db =>
        {
            db.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH);
        });

        builder.Property(v => v.Experience)
            .IsRequired();
        
        builder.ComplexProperty(p => p.PhoneNumber, pb =>
        {
            pb.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(PhoneNumber.MAX_PHONE_NUMBER_LENGTH);
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
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}