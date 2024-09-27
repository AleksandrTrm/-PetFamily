using System.Linq.Expressions;
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

        builder.Property(v => v.SocialMedias)
            .HasConversion(
                sm => JsonSerializer.Serialize(sm
                    .Select(s => new SocialMediaDto(s.Title, s.Link)), JsonSerializerOptions.Default),
                json => JsonSerializer
                    .Deserialize<IReadOnlyList<SocialMediaDto>>(json, JsonSerializerOptions.Default)!
                    .Select(sm => SocialMedia.Create(sm.Title, sm.Link).Value)
                    .ToList(),
                new ValueComparer<IReadOnlyList<SocialMediaDto>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c
                        .Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

        builder.Property(v => v.Requisites)
            .HasConversion(
                r => JsonSerializer.Serialize(r
                    .Select(r => new RequisiteDto(r.Title, r.Description.Value)), JsonSerializerOptions.Default),
                json => JsonSerializer
                    .Deserialize<IReadOnlyList<RequisiteDto>>(json, JsonSerializerOptions.Default)!
                    .Select(sm => Requisite.Create(sm.Title, Description.Create(sm.Description).Value).Value)
                    .ToList(),
                new ValueComparer<IReadOnlyList<SocialMediaDto>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c
                        .Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()))
            .HasJsonPropertyName("requisites");

        builder.HasMany(v => v.Pets)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}