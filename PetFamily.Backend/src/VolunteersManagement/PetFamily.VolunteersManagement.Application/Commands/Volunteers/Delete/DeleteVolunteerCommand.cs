using PetFamily.Shared.Core.Abstractions;

namespace PetFamily.VolunteersManagement.Application.Commands.Volunteers.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;