using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Shared.Core.DTOs;
using PetFamily.Shared.Core.DTOs.VolunteerDtos;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel.IDs;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;
using PetFamily.VolunteersManagement.Domain.AggregateRoot;

namespace PetFamily.VolunteersManagement.Infrastructure.Configurations.Write;

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
            .IsRequired()
            .HasColumnName("id");

        builder.ComplexProperty(v => v.FullName, nb =>
        {
            nb.Property(n => n.Name)
                .IsRequired()
                .HasMaxLength(Shared.SharedKernel.Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("name");

            nb.Property(n => n.Surname)
                .IsRequired()
                .HasMaxLength(Shared.SharedKernel.Constants.MAX_MIDDLE_TEXT_LENGTH)
                .HasColumnName("surname");

            nb.Property(n => n.Patronymic)
                .IsRequired()
                .HasMaxLength(Shared.SharedKernel.Constants.MAX_MIDDLE_TEXT_LENGTH)
                .HasColumnName("patronymic");
        });


        builder.ComplexProperty(p => p.Description, db =>
        {
            db.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(Shared.SharedKernel.Constants.MAX_HIGH_TEXT_LENGTH)
                .HasColumnName("description");
        });

        builder.Property(v => v.Experience)
            .IsRequired()
            .HasColumnName("experience");

        builder.ComplexProperty(p => p.PhoneNumber, pb =>
        {
            pb.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(PhoneNumber.MAX_PHONE_NUMBER_LENGTH)
                .HasColumnName("phone_number");
        });

        builder.Property(v => v.Requisites)
            .HasValueObjectsJsonConversion(
                r => new RequisiteDto(r.Title, r.Description),
                dto => Requisite.Create(dto.Title, dto.Description).Value)
            .HasColumnName("requisites");

        builder.Property(v => v.SocialMedias)
            .HasValueObjectsJsonConversion(
                sm => new SocialMediaDto(sm.Title, sm.Link),
                dto => SocialNetwork.Create(dto.Title, dto.Link).Value)
            .HasColumnName("social_networks");

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey(p => p.VolunteerId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(v => v.IsDeleted)
            .IsRequired();
    }
}