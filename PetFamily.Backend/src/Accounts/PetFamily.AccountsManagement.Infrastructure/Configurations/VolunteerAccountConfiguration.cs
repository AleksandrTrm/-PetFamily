using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.AccountsManagement.Domain.Entities.Accounts;
using PetFamily.Shared.Core.Extensions;
using PetFamily.Shared.SharedKernel.DTOs;
using PetFamily.Shared.SharedKernel.DTOs.VolunteerDtos;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;

namespace PetFamily.AccountsManagement.Infrastructure.Configurations;

public class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
{
    public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
    {
        builder.ToTable("volunteer_accounts");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Experience).IsRequired();
        
        builder.Property(v => v.Description).IsRequired();

        builder.Property(v => v.Requisites)
            .HasValueObjectsJsonConversion(
                requisite => new RequisiteDto(requisite.Title, requisite.Description),
                requisiteDto => Requisite.Create(requisiteDto.Title, requisiteDto.Description).Value);

        builder.Property(v => v.SocialNetworks)
            .HasValueObjectsJsonConversion(
                socialNetwork => new SocialNetworkDto(socialNetwork.Title, socialNetwork.Link),
                requisiteDto => SocialNetwork.Create(requisiteDto.Title, requisiteDto.Link).Value);
    }
}