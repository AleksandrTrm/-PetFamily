using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.AccountsManagement.Domain.Entities.Roles;
using PetFamily.Shared.SharedKernel.ValueObjects.Volunteers.Shared;

namespace PetFamily.AccountsManagement.Infrastructure.Configurations;

public class VolunteerAccountConfiguration : IEntityTypeConfiguration<VolunteerAccount>
{
    public void Configure(EntityTypeBuilder<VolunteerAccount> builder)
    {
        builder.ToTable("volunteer_account");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Experience).IsRequired();

        builder.Property(v => v.Requisites).HasConversion(
            r => JsonSerializer.Serialize(r, JsonSerializerOptions.Default),
            r => JsonSerializer.Deserialize<IReadOnlyList<Requisite>>(r, JsonSerializerOptions.Default)!,
            new ValueComparer<IReadOnlyList<Requisite>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => 
                    c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
        
        builder.Property(v => v.Certificates).HasConversion(
            r => JsonSerializer.Serialize(r, JsonSerializerOptions.Default),
            r => JsonSerializer.Deserialize<IReadOnlyList<string>>(r, JsonSerializerOptions.Default)!,
            new ValueComparer<IReadOnlyList<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => 
                    c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        builder.HasOne(v => v.User)
            .WithOne()
            .IsRequired();
    }
}