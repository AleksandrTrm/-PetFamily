using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.AccountsManagement.Domain.Entities;

namespace PetFamily.AccountsManagement.Infrastructure.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permission");
        
        builder.HasKey(rolePermission => new { rolePermission.PermissionId, rolePermission.RoleId });

        builder.HasOne(rolePermission => rolePermission.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rolePermission => rolePermission.RoleId);
        
        builder.HasOne(rolePermission => rolePermission.Permission)
            .WithMany()
            .HasForeignKey(rolePermission => rolePermission.PermissionId);

        builder.Navigation(rolePermission => rolePermission.Permission).AutoInclude();
    }
}