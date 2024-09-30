using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.Commands.Volunteers.Delete;

public record DeleteVolunteerCommand(Guid VolunteerId) : ICommand;