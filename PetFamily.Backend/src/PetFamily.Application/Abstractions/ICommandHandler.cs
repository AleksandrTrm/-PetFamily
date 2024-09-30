using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared.Error;

namespace PetFamily.Application.Abstractions;

public interface ICommandHandler<TResponse, in TCommand>
{
    Task<Result<TResponse, ErrorList>> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand>
{
    Task<UnitResult<ErrorList>> Handle(TCommand command, CancellationToken cancellationToken);
}