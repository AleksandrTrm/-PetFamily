using PetFamily.Application.Abstractions;
using PetFamily.Application.DTOs;

namespace PetFamily.Application.Features.Commands.Volunteers.Update.UpdateRequisites;

public record UpdateRequisitesCommand(Guid VolunteerId, IEnumerable<RequisiteDto> Requisites) : ICommand;