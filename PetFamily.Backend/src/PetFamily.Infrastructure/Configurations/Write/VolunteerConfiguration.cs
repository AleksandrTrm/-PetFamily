using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.DTOs;
using PetFamily.Application.DTOs.VolunteerDtos;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.IDs;
using PetFamily.Domain.VolunteersManagement.AggregateRoot;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Shared;
using PetFamily.Domain.VolunteersManagement.ValueObjects.Volunteer;
using PetFamily.Infrastructure.Extensions;

namespace PetFamily.Infrastructure.Configurations.Write;

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

        builder.Property(v => v.Requisites.Values)
            .HasValueObjectsJsonConversion(
                r => new RequisiteDto(r.Title, r.Description.Value),
                dto => Requisite.Create(dto.Title, Description.Create(dto.Description).Value).Value);

        builder.Property(v => v.SocialMedias.Values)
            .HasValueObjectsJsonConversion(
                sm => new SocialMediaDto(sm.Title, sm.Link),
                dto => SocialMedia.Create(dto.Title, dto.Link).Value);

        builder.HasMany(v => v.Pets)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}