using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.AccountsManagement.Domain.Entities.Roles;
using PetFamily.Shared.SharedKernel;

namespace PetFamily.AccountsManagement.Infrastructure.Configurations;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.ToTable("participants");

        builder.HasKey(p => p.Id);
        
        builder.HasOne<User>(p => p.User)
            .WithOne()
            .IsRequired();
    }
}