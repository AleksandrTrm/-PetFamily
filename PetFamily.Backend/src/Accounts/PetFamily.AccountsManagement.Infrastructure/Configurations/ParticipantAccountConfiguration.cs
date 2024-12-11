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
        builder.ToTable("participants");

        builder.HasKey(p => p.Id);
        
        builder.HasOne<User>(p => p.User)
            .WithOne()
            .IsRequired();
        
        builder.HasOne<User>(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId);
    }
}