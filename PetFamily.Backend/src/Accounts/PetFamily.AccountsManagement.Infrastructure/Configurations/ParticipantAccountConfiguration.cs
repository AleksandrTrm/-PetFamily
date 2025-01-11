using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.AccountsManagement.Domain.Entities.Accounts;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.AccountsManagement.Infrastructure.Configurations;

public class ParticipantAccountConfiguration : IEntityTypeConfiguration<ParticipantAccount>
{
    public void Configure(EntityTypeBuilder<ParticipantAccount> builder)
    {
        builder.ToTable("participant_accounts");

        builder.HasKey(p => p.Id);
    }
}