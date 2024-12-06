using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.AccountsManagement.Domain.Entities.Roles;
using PetFamily.Shared.Core.DTOs;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;

namespace PetFamily.AccountsManagement.Infrastructure.Configurations;

public class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
{
    public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
    {
        builder.ToTable("volunteer_account");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Experience).IsRequired();

        builder.Property(v => v.Requisites)
            .HasValueObjectsJsonConversion(
                requisite => new RequisiteDto(requisite.Title, requisite.Description),
                requisiteDto => Requisite.Create(requisiteDto.Title, requisiteDto.Description).Value);

        builder.HasOne(v => v.User)
            .WithOne()
            .IsRequired();
    }
}