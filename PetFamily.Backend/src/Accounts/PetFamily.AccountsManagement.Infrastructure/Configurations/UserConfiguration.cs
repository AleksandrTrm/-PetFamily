using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.AccountsManagement.Domain.Entities;
using PetFamily.Shared.SharedKernel;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Volunteer;

namespace PetFamily.AccountsManagement.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.PasswordHash);

        builder.Property(u => u.UserName).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        builder.Property(u => u.SocialNetworks)
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<IReadOnlyList<SocialNetwork>>(v, JsonSerializerOptions.Default)!, 
                new ValueComparer<IReadOnlyList<SocialNetwork>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => 
                        c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()))
            .IsRequired(false);

        builder.Property(u => u.Email).IsRequired();

        builder.Property(u => u.Photo)
            .IsRequired(false);

        builder.ComplexProperty(u => u.FullName, fnb =>
        {
            fnb.Property(fn => fn.Name)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .IsRequired();

            fnb.Property(fn => fn.Surname)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .IsRequired();

            fnb.Property(fn => fn.Patronymic)
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .IsRequired(false);
        });
        
        builder.HasMany(u => u.Roles)
            .WithMany(u=> u.Users)
            .UsingEntity<IdentityUserRole<Guid>>();
    }
}