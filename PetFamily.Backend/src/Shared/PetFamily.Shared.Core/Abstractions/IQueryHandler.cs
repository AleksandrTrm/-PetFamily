namespace PetFamily.Shared.Core.Abstractions;

public interface IQueryHandler<TResponse, in TQuery>
{
    Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);
}