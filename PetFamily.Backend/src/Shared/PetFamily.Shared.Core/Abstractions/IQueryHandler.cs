namespace PetFamily.Shared.Core.Abstractions;

public interface IQueryHandler<TResponse, in TCommand>
{
    Task<TResponse> Handle(TCommand query, CancellationToken cancellationToken);
}