using PetFamily.Shared.SharedKernel;

namespace PetFamily.Shared.Core.Abstractions;

public class SoftDeletableEntity<TId> : Entity<TId> where TId : notnull
{
    public bool IsDeleted { get; private set; }

    public DateTime? DeletionTime { get; private set; }

    public virtual void Delete()
    {
        if (IsDeleted) return;

        IsDeleted = true;
        DeletionTime = DateTime.UtcNow;
    }

    public virtual void Restore()
    {
        if (!IsDeleted) return;

        IsDeleted = false;
        DeletionTime = null;
    }

    protected SoftDeletableEntity(TId id) : base(id)
    {
    }
}