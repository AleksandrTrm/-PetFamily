using PetFamily.Application.DTOs;

namespace PetFamily.Application.Volunteers.Update.UpdateRequisites;

public record UpdateRequisitesCommand(Guid VolunteerId, IEnumerable<RequisiteDto> Requisites);