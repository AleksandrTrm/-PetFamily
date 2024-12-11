using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.AccountsManagement.Domain.Entities.Accounts;

namespace PetFamily.AccountsManagement.Infrastructure.Configurations;

public class AdminAccountConfiguration : IEntityTypeConfiguration<AdminAccount>
{
    public void Configure(EntityTypeBuilder<AdminAccount> builder)
    {
        builder.ToTable("admin_accounts");

        builder.HasKey(a => a.Id);

        builder.HasOne<User>(a => a.User)
            .WithOne()
            .IsRequired();

        builder.HasOne<User>(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId);
    }
}